namespace BookStore.Domain.Exceptions;

/// <summary>
/// Biznes qaydası pozulduqda atılır (məs. stok çatışmazlığı).
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}
