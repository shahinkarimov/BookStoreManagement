using BookStore.Application;
using BookStore.Infrastructure;
using BookStore.Infrastructure.Persistence;
using BookStore.Presentation.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

// ─────────────────────────────────────────────────────────────
// Composition Root — bütün qatlar yalnız burada bir-birinə bağlanır.
// Menyular Infrastructure-u tanımır; onlara yalnız Application
// interfeysləri inject olunur.
// ─────────────────────────────────────────────────────────────

// "dotnet ef" əmrləri işləyəndə tətbiq özü başlamasın
if (Microsoft.EntityFrameworkCore.EF.IsDesignTime) return;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var services = new ServiceCollection()
    .AddApplication()
    .AddInfrastructure(configuration)
    .AddScoped<BooksMenu>()
    .AddScoped<AuthorsMenu>()
    .AddScoped<GenresMenu>()
    .AddScoped<OrdersMenu>();

await using var provider = services.BuildServiceProvider();

// Bazanı hazırla (migration + seed)
try
{
    await AnsiConsole.Status()
        .Spinner(Spinner.Known.Aesthetic)
        .SpinnerStyle(Style.Parse("green"))
        .StartAsync("Verilənlər bazası hazırlanır...", async _ =>
        {
            using var scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();
            await DbInitializer.InitializeAsync(context);
        });
}
catch (Exception ex)
{
    AnsiConsole.MarkupLine("[bold red]Verilənlər bazasına qoşulmaq mümkün olmadı![/]");
    AnsiConsole.MarkupLine($"[red]{Markup.Escape(ex.Message)}[/]");
    AnsiConsole.MarkupLine("\n[yellow]Yoxlayın:[/] PostgreSQL işləyirmi? ([grey]docker compose up -d[/])");
    AnsiConsole.MarkupLine("[yellow]və ya[/] appsettings.json-da [grey]Database:Provider[/] düzgün seçilibmi?");
    return;
}

// Əsas menyu dövrü
while (true)
{
    ConsoleUi.ShowBanner();
    var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[bold green]*** BOOKSTORE SYSTEM ***[/]")
            .HighlightStyle(new Style(Color.Black, Color.Green))
            .AddChoices(
                "1. Kitablar",
                "2. Müəlliflər",
                "3. Janrlar",
                "4. Müştərilər və Sifarişlər",
                "0. Çıxış"));

    using var menuScope = provider.CreateScope();
    var sp = menuScope.ServiceProvider;

    switch (choice[0])
    {
        case '1': await sp.GetRequiredService<BooksMenu>().RunAsync(); break;
        case '2': await sp.GetRequiredService<AuthorsMenu>().RunAsync(); break;
        case '3': await sp.GetRequiredService<GenresMenu>().RunAsync(); break;
        case '4': await sp.GetRequiredService<OrdersMenu>().RunAsync(); break;
        default:
            AnsiConsole.MarkupLine("\n[green]Sağ olun! BookStore bağlanır...[/]");
            return;
    }
}
