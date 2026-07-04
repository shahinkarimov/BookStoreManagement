using BookStore.Application.DTOs.Customers;
using BookStore.Application.DTOs.Orders;
using BookStore.Application.Services.Interfaces;
using Spectre.Console;

namespace BookStore.Presentation.UI;

/// <summary>
/// Müştərilər və Sifarişlər bölməsi (4.1 – 4.3): qeydiyyat, yeni sifariş, sifarişlərə baxış.
/// </summary>
public class OrdersMenu
{
    private readonly ICustomerService _customerService;
    private readonly IOrderService _orderService;
    private readonly IBookService _bookService;

    public OrdersMenu(ICustomerService customerService, IOrderService orderService, IBookService bookService)
    {
        _customerService = customerService;
        _orderService = orderService;
        _bookService = bookService;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            ConsoleUi.ShowBanner();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]MÜŞTƏRİLƏR VƏ SİFARİŞLƏR[/]")
                    .HighlightStyle(new Style(Color.Black, Color.Green))
                    .AddChoices(
                        "4.1 Müştəri Qeydiyyatı",
                        "4.2 Yeni Sifariş",
                        "4.3 Sifarişlərə Baxış",
                        "0 Geri"));

            try
            {
                switch (choice[..3])
                {
                    case "4.1": await RegisterCustomerAsync(); break;
                    case "4.2": await CreateOrderAsync(); break;
                    case "4.3": await ViewOrdersAsync(); break;
                    default: return;
                }
            }
            catch (Exception ex)
            {
                ConsoleUi.ShowError(ex);
                ConsoleUi.Pause();
            }
        }
    }

    private async Task RegisterCustomerAsync()
    {
        var request = new CreateCustomerRequestDto
        {
            FullName = ConsoleUi.AskString("Müştərinin tam adı"),
            Email = ConsoleUi.AskString("Email"),
            Phone = ConsoleUi.AskString("Telefon (boş buraxıla bilər)") is var p && string.IsNullOrWhiteSpace(p) ? null : p
        };

        var created = await _customerService.RegisterAsync(request);
        ConsoleUi.ShowSuccess($"\"{created.FullName}\" qeydiyyatdan keçdi (ID={created.Id}).");
        ConsoleUi.Pause();
    }

    private async Task CreateOrderAsync()
    {
        var customers = await _customerService.GetAllAsync();
        if (customers.Count == 0)
        {
            ConsoleUi.ShowWarning("Əvvəlcə müştəri qeydiyyatdan keçirilməlidir.");
            ConsoleUi.Pause();
            return;
        }

        AnsiConsole.MarkupLine("[grey]Müştərilər:[/] " +
            string.Join(", ", customers.Select(c => $"[cyan]{c.Id}[/]={Markup.Escape(c.FullName)}")));

        var customerId = ConsoleUi.AskInt("Müştəri ID");

        var books = await _bookService.GetAllAsync();
        ConsoleUi.RenderBooks(books, "Mövcud Kitablar");

        var items = new List<OrderItemRequestDto>();
        while (true)
        {
            var bookId = ConsoleUi.AskInt("Kitab ID");
            var quantity = ConsoleUi.AskInt("Say");
            items.Add(new OrderItemRequestDto { BookId = bookId, Quantity = quantity });

            if (!AnsiConsole.Confirm("[green]Daha bir kitab əlavə edilsin?[/]", false))
                break;
        }

        var request = new CreateOrderRequestDto { CustomerId = customerId, Items = items };

        OrderResponseDto created = null!;
        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .SpinnerStyle(Style.Parse("green"))
            .StartAsync("Sifariş emal olunur...", async _ =>
            {
                created = await _orderService.CreateAsync(request);
            });

        ConsoleUi.ShowSuccess("Sifariş uğurla yaradıldı!");
        ConsoleUi.RenderOrder(created);
        ConsoleUi.Pause();
    }

    private async Task ViewOrdersAsync()
    {
        var scope = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Hansı sifarişlər göstərilsin?[/]")
                .HighlightStyle(new Style(Color.Black, Color.Green))
                .AddChoices("Konkret müştərinin sifarişləri", "Bütün sifarişlər"));

        IReadOnlyList<OrderResponseDto> orders;
        if (scope.StartsWith("Konkret"))
        {
            var customers = await _customerService.GetAllAsync();
            AnsiConsole.MarkupLine("[grey]Müştərilər:[/] " +
                string.Join(", ", customers.Select(c => $"[cyan]{c.Id}[/]={Markup.Escape(c.FullName)}")));
            var customerId = ConsoleUi.AskInt("Müştəri ID");
            orders = await _orderService.GetByCustomerAsync(customerId);
        }
        else
        {
            orders = await _orderService.GetAllAsync();
        }

        if (orders.Count == 0)
        {
            ConsoleUi.ShowWarning("Heç bir sifariş tapılmadı.");
        }
        else
        {
            foreach (var order in orders)
            {
                ConsoleUi.RenderOrder(order);
                AnsiConsole.WriteLine();
            }
        }

        ConsoleUi.Pause();
    }
}
