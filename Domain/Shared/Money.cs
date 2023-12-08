namespace Domain.Shared;

public record Money
{
    private Money(string currency, decimal amount)
    {
        Currency = currency;
        Amount = amount;
    }

    public static Money Create(string currency, decimal amount)
    {
        return new Money(currency, amount);
    }

    public string Currency { get; set; }
    public decimal Amount { get; set; }
}
