using BookStore.Application.DTOs.Genres;
using BookStore.Application.Services.Interfaces;
using Spectre.Console;

namespace BookStore.Presentation.UI;

/// <summary>
/// Janrlar bölməsi (3.1 – 3.2 + janra görə kitablar).
/// </summary>
public class GenresMenu
{
    private readonly IGenreService _genreService;

    public GenresMenu(IGenreService genreService)
    {
        _genreService = genreService;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            ConsoleUi.ShowBanner();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]JANRLAR[/]")
                    .HighlightStyle(new Style(Color.Black, Color.Green))
                    .AddChoices(
                        "3.1 Yeni Janr Əlavə Et",
                        "3.2 Janr Siyahısı",
                        "3.3 Janra Görə Kitablar",
                        "0 Geri"));

            try
            {
                switch (choice[..3])
                {
                    case "3.1": await AddGenreAsync(); break;
                    case "3.2": await ListGenresAsync(); break;
                    case "3.3": await ShowGenreBooksAsync(); break;
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

    private async Task AddGenreAsync()
    {
        var request = new CreateGenreRequestDto
        {
            Name = ConsoleUi.AskString("Janr adı")
        };

        var created = await _genreService.CreateAsync(request);
        ConsoleUi.ShowSuccess($"\"{created.Name}\" janrı əlavə olundu (ID={created.Id}).");
        ConsoleUi.Pause();
    }

    private async Task ListGenresAsync()
    {
        var genres = await _genreService.GetAllAsync();

        if (genres.Count == 0)
        {
            ConsoleUi.ShowWarning("Heç bir janr tapılmadı.");
            ConsoleUi.Pause();
            return;
        }

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Green)
            .Title("[bold green]Bütün Janrlar[/]");

        table.AddColumn(new TableColumn("[bold]ID[/]").Centered());
        table.AddColumn("[bold]Ad[/]");
        table.AddColumn(new TableColumn("[bold]Kitab sayı[/]").Centered());

        foreach (var g in genres)
        {
            table.AddRow(g.Id.ToString(), Markup.Escape(g.Name), $"[cyan]{g.BookCount}[/]");
        }

        AnsiConsole.Write(table);
        ConsoleUi.Pause();
    }

    private async Task ShowGenreBooksAsync()
    {
        var id = ConsoleUi.AskInt("Janrın ID-si");
        var books = await _genreService.GetBooksAsync(id);
        ConsoleUi.RenderBooks(books, "Janra Görə Kitablar");
        ConsoleUi.Pause();
    }
}
