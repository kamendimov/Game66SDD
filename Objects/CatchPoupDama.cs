namespace Cantace;

/// <summary>
/// Represents the result of checking for a Poup and Dama card combination.
/// </summary>
public class CatchPoupDama
{
    public Card CardPoup { get; set; }
    public Card CardDama { get; set; }
    public bool IsForty { get; set; }

    public CatchPoupDama()
    {
        CardPoup = null!;
        CardDama = null!;
    }
}