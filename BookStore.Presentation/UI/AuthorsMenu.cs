using BookStore.Application.DTOs.Authors;
using BookStore.Application.Services.Interfaces;
using Spectre.Console;

namespace BookStore.Presentation.UI;

/// <summary>
/// Müəlliflər bölməsi (2.1 – 2.3): əlavə, siyahı, müəllifin kitabları.
/// </summary>
public class AuthorsMenu
{
    private readonly IAuthorService _authorService;

    public AuthorsMenu(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            ConsoleUi.ShowBanner();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]MÜƏLLİFLƏR[/]")
                    .HighlightStyle(new Style(Color.Black, Color.Green))
                    .AddChoices(
                        "2.1 Yeni Müəllif Əlavə Et",
                        "2.2 Müəllif Siyahısı",
                        "2.3 Müəllifin Kitabları",
                        "0 Geri"));

            try
            {
                switch (choice[..3])
                {
                    case "2.1": await AddAuthorAsync(); break;
                    case "2.2": await ListAuthorsAsync(); break;
                    case "2.3": await ShowAuthorBooksAsync(); break;
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

    private async Task AddAuthorAsync()
    {
        var request = new CreateAuthorRequestDto
        {
            FullName = ConsoleUi.AskString("Müəllifin tam adı"),
            Country = ConsoleUi.AskString("Ölkə (boş buraxıla bilər)") is var c && string.IsNullOrWhiteSpace(c) ? null : c
        };

        var created = await _authorService.CreateAsync(request);
        ConsoleUi.ShowSuccess($"\"{created.FullName}\" müəllifi əlavə olundu (ID={created.Id}).");
        ConsoleUi.Pause();
    }

    private async Task ListAuthorsAsync()
    {
        var authors = await _authorService.GetAllAsync();

        if (authors.Count == 0)
        {
            ConsoleUi.ShowWarning("Heç bir müəllif tapılmadı.");
            ConsoleUi.Pause();
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Green)
            .Title("[bold green]Bütün Müəlliflər[/]");

        table.AddColumn(new TableColumn("[bold]ID[/]").Centered());
        table.AddColumn("[bold]Tam ad[/]");
        table.AddColumn("[bold]Ölkə[/]");
        table.AddColumn(new TableColumn("[bold]Kitab sayı[/]").Centered());

        foreach (var a in authors)
        {
            table.AddRow(
                a.Id.ToString(),
                Markup.Escape(a.FullName),
                Markup.Escape(a.Country ?? "—"),
                $"[cyan]{a.BookCount}[/]");
        }

        AnsiConsole.Write(table);
        ConsoleUi.Pause();
    }

    private async Task ShowAuthorBooksAsync()
    {
        var id = ConsoleUi.AskInt("Müəllifin ID-si");
        var books = await _authorService.GetBooksAsync(id);
        ConsoleUi.RenderBooks(books, "Müəllifin Kitabları");
        ConsoleUi.Pause();
    }
}
