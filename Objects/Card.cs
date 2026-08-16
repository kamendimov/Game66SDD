namespace Cantace;

using System.Drawing;

/// <summary>
/// Data model for a card in the 66 deck.
/// Represents an individual playing card with name, value, index, and image path.
/// </summary>
public class Card
{
    public Suit CardName { get; set; }
    public Rank CardValue { get; set; }
    public int Index { get; set; }
    public string ImagePath { get; set; } = string.Empty;
}
