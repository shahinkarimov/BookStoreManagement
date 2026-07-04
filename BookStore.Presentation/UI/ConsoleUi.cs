using BookStore.Application.DTOs.Books;
using BookStore.Application.DTOs.Orders;
using BookStore.Domain.Exceptions;
using FluentValidation;
using Spectre.Console;

namespace BookStore.Presentation.UI;

/// <summary>
/// Konsol UI köməkçiləri — daxiletmə, cədvəl render, xəta göstərimi.
/// Bütün istifadəçi qarşılıqlı əlaqəsi bu sinifdən keçir.
/// </summary>
public static class ConsoleUi
{
    public static void ShowBanner()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("BOOKSTORE")
                .Centered()
                .Color(Color.Green));
        AnsiConsole.Write(
            new Rule("[grey]Onion Architecture • .NET 8 • EF Core • AutoMapper • FluentValidation[/]")
                .RuleStyle("green dim")
                .Centered());
        AnsiConsole.WriteLine();
    }

    public static string AskString(string label)
        => AnsiConsole.Prompt(
            new TextPrompt<string>($"[green]?[/] {label}:")
                .PromptStyle("white")
                .AllowEmpty());

    public static int AskInt(string label)
        => AnsiConsole.Prompt(
            new TextPrompt<int>($"[green]?[/] {label}:")
                .PromptStyle("white")
                .ValidationErrorMessage("[red]Düzgün tam ədəd daxil edin.[/]"));

    public static decimal AskDecimal(string label)
        => AnsiConsole.Prompt(
            new TextPrompt<decimal>($"[green]?[/] {label}:")
                .PromptStyle("white")
                .ValidationErrorMessage("[red]Düzgün rəqəm daxil edin (məs. 12.50).[/]"));

    public static void Pause()
    {
        AnsiConsole.MarkupLine("\n[grey dim]Davam etmək üçün istənilən düyməni basın...[/]");
        Console.ReadKey(true);
    }

    public static void ShowSuccess(string message)
        => AnsiConsole.MarkupLine($"\n[bold green]✓[/] [green]{Markup.Escape(message)}[/]");

    public static void ShowWarning(string message)
        => AnsiConsole.MarkupLine($"\n[bold yellow]![/] [yellow]{Markup.Escape(message)}[/]");

    /// <summary>
    /// Bütün gözlənilən xəta növlərini istifadəçiyə oxunaqlı formada göstərir.
    /// </summary>
    public static void ShowError(Exception ex)
    {
        switch (ex)
        {
            case ValidationException validationEx:
                var errors = validationEx.Errors.Select(e => $"[red]•[/] {Markup.Escape(e.ErrorMessage)}");
                AnsiConsole.Write(new Panel(string.Join("\n", errors))
                    .Header("[bold red] Validasiya xətaları [/]")
                    .BorderColor(Color.Red));
                break;
            case NotFoundException or DomainException:
                AnsiConsole.MarkupLine($"\n[bold red]✗[/] [red]{Markup.Escape(ex.Message)}[/]");
                break;
            default:
                AnsiConsole.MarkupLine($"\n[bold red]✗ Gözlənilməz xəta:[/] [red]{Markup.Escape(ex.Message)}[/]");
                break;
        }
    }

    public static void RenderBooks(IReadOnlyList<BookResponseDto> books, string title)
    {
        if (books.Count == 0)
        {
            ShowWarning("Heç bir kitab tapılmadı.");
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Green)
            .Title($"[bold green]{Markup.Escape(title)}[/]");

        table.AddColumn(new TableColumn("[bold]ID[/]").Centered());
        table.AddColumn("[bold]Ad[/]");
        table.AddColumn("[bold]Müəllif[/]");
        table.AddColumn("[bold]Janr[/]");
        table.AddColumn(new TableColumn("[bold]Qiymət[/]").RightAligned());
        table.AddColumn(new TableColumn("[bold]Stok[/]").Centered());

        foreach (var b in books)
        {
            var stockColor = b.Stock == 0 ? "red" : b.Stock < 10 ? "yellow" : "green";
            table.AddRow(
                b.Id.ToString(),
                Markup.Escape(b.Title),
                Markup.Escape(b.AuthorName),
                $"[cyan]{Markup.Escape(b.GenreName)}[/]",
                $"{b.Price:0.00} ₼",
                $"[{stockColor}]{b.Stock}[/]");
        }

        AnsiConsole.Write(table);
    }

    public static void RenderOrder(OrderResponseDto order)
    {
        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Green)
            .Title($"[bold green]Sifariş #{order.Id}[/] [grey]— {order.CustomerName}, {order.OrderDate:dd.MM.yyyy HH:mm}[/]");

        table.AddColumn("[bold]Kitab[/]");
        table.AddColumn(new TableColumn("[bold]Say[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Vahid qiymət[/]").RightAligned());
        table.AddColumn(new TableColumn("[bold]Cəmi[/]").RightAligned());

        foreach (var item in order.Items)
        {
            table.AddRow(
                Markup.Escape(item.BookTitle),
                item.Quantity.ToString(),
                $"{item.UnitPrice:0.00} ₼",
                $"{item.LineTotal:0.00} ₼");
        }

        table.AddRow("", "", "[bold]YEKUN:[/]", $"[bold green]{order.TotalAmount:0.00} ₼[/]");
        AnsiConsole.Write(table);
    }
}
