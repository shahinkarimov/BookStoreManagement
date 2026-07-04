namespace BookStore.Domain.Exceptions;

/// <summary>
/// Axtarılan entity bazada tapılmadıqda atılır.
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string entityName, int id)
        : base($"{entityName} (ID={id}) tapılmadı.")
    {
    }
}
