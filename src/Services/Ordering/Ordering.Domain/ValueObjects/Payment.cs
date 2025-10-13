namespace Ordering.Domain.ValueObjects;

public record Payment
{
    public string? CardName { get; } = null!;
    public string CardNumber { get; } = null!;
    public string Expiration { get; } = null!;
    public string CVV { get; } = null!;
    public int PaymentMethod { get; } = 0;

}