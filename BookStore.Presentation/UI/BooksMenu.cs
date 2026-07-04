using BookStore.Application.DTOs.Books;
using BookStore.Application.Services.Interfaces;
using Spectre.Console;

namespace BookStore.Presentation.UI;

/// <summary>
/// Kitablar bölməsi (1.1 – 1.5): əlavə, siyahı, axtarış, redaktə, silmə.
/// Yalnız IBookService interfeysi ilə işləyir — Infrastructure-a müraciət yoxdur.
/// </summary>
public class BooksMenu
{
    private readonly IBookService _bookService;
    private readonly IAuthorService _authorService;
    private readonly IGenreService _genreService;

    public BooksMenu(IBookService bookService, IAuthorService authorService, IGenreService genreService)
    {
        _bookService = bookService;
        _authorService = authorService;
        _genreService = genreService;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            ConsoleUi.ShowBanner();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]KİTABLAR[/]")
                    .HighlightStyle(new Style(Color.Black, Color.Green))
                    .AddChoices(
                        "1.1 Yeni Kitab Əlavə Et",
                        "1.2 Kitab Siyahısı",
                        "1.3 Kitab Axtar",
                        "1.4 Kitabı Redaktə Et",
                        "1.5 Kitabı Sil",
                        "0 Geri"));

            try
            {
                switch (choice[..3])
                {
                    case "1.1": await AddBookAsync(); break;
                    case "1.2": await ListBooksAsync(); break;
                    case "1.3": await SearchBooksAsync(); break;
                    case "1.4": await EditBookAsync(); break;
                    case "1.5": await DeleteBookAsync(); break;
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

    private async Task AddBookAsync()
    {
        // Mövcud müəllif və janrları göstəririk ki, istifadəçi ID seçə bilsin
        var authors = await _authorService.GetAllAsync();
        var genres = await _genreService.GetAllAsync();

        if (authors.Count == 0 || genres.Count == 0)
        {
            ConsoleUi.ShowWarning("Əvvəlcə ən az bir müəllif və bir janr əlavə edilməlidir.");
            ConsoleUi.Pause();
            return;
        }

        AnsiConsole.MarkupLine("[grey]Müəlliflər:[/] " +
            string.Join(", ", authors.Select(a => $"[cyan]{a.Id}[/]={Markup.Escape(a.FullName)}")));
        AnsiConsole.MarkupLine("[grey]Janrlar:[/] " +
            string.Join(", ", genres.Select(g => $"[cyan]{g.Id}[/]={Markup.Escape(g.Name)}")));
        AnsiConsole.WriteLine();

        var request = new CreateBookRequestDto
        {
            Title = ConsoleUi.AskString("Kitabın adı"),
            Price = ConsoleUi.AskDecimal("Qiymət (₼)"),
            Stock = ConsoleUi.AskInt("Stok sayı"),
            AuthorId = ConsoleUi.AskInt("Müəllif ID"),
            GenreId = ConsoleUi.AskInt("Janr ID")
        };

        var created = await _bookService.CreateAsync(request);
        ConsoleUi.ShowSuccess($"\"{created.Title}\" kitabı əlavə olundu (ID={created.Id}).");
        ConsoleUi.Pause();
    }

    private async Task ListBooksAsync()
    {
        var books = await _bookService.GetAllAsync();
        ConsoleUi.RenderBooks(books, "Bütün Kitablar");
        ConsoleUi.Pause();
    }

    private async Task SearchBooksAsync()
    {
        var term = ConsoleUi.AskString("Axtarış (ad / müəllif / janr)");
        var books = await _bookService.SearchAsync(term);
        ConsoleUi.RenderBooks(books, $"Axtarış nəticəsi: \"{term}\"");
        ConsoleUi.Pause();
    }

    private async Task EditBookAsync()
    {
        var id = ConsoleUi.AskInt("Redaktə ediləcək kitabın ID-si");
        var existing = await _bookService.GetByIdAsync(id);

        if (existing is null)
        {
            ConsoleUi.ShowWarning($"ID={id} olan kitab tapılmadı.");
            ConsoleUi.Pause();
            return;
        }

        ConsoleUi.RenderBooks(new[] { existing }, "Cari vəziyyət");

        var request = new UpdateBookRequestDto
        {
            Id = id,
            Title = ConsoleUi.AskString($"Yeni ad [{existing.Title}]") is var t && !string.IsNullOrWhiteSpace(t) ? t : existing.Title,
            Price = ConsoleUi.AskDecimal("Yeni qiymət (₼)"),
            Stock = ConsoleUi.AskInt("Yeni stok")
        };

        var updated = await _bookService.UpdateAsync(request);
        ConsoleUi.ShowSuccess($"\"{updated.Title}\" yeniləndi.");
        ConsoleUi.Pause();
    }

    private async Task DeleteBookAsync()
    {
        var id = ConsoleUi.AskInt("Silinəcək kitabın ID-si");

        if (!AnsiConsole.Confirm($"[red]ID={id} olan kitab silinsin?[/]", false))
        {
            ConsoleUi.ShowWarning("Silmə ləğv edildi.");
            ConsoleUi.Pause();
            return;
        }

        await _bookService.DeleteAsync(id);
        ConsoleUi.ShowSuccess($"ID={id} olan kitab silindi.");
        ConsoleUi.Pause();
    }
}
