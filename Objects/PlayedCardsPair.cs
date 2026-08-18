namespace Cantace;

/// <summary>
/// Represents a pair of cards played in a single round.
/// </summary>
public class PlayedCardsPair
{
    public Card? UserPlayerCard { get; set; }
    public Card? ComputerPlayerCard { get; set; }
    public int? UserPlayerResult { get; set; }
    public int? ComputerPlayerResult { get; set; }
}
