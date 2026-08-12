namespace Cantace;

/// <summary>
/// Abstract base class for a player in the 66 game.
/// Manages the player's hand of cards, score, and current card.
/// </summary>
public abstract class Player
{
    protected List<Card> Cards;
    private int Score;

    public Card? CurrentCard { get; set; }

    protected Player()
    {
        Cards = new List<Card>();
    }

    public void SetCard(Card card)
    {
        Cards.Add(card);
    }

    public void ClearCards()
    {
        Cards.Clear();
    }

    public void RemoveCard(Card card)
    {
        Cards.Remove(card);
    }

    public List<Card> GetCards()
    {
        return new List<Card>(Cards);
    }

    public void IncrementScore(int value)
    {
        Score += value;
    }

    public int GetScore()
    {
        return Score;
    }

    public void ResetScore()
    {
        Score = 0;
    }

    public static string GetCardSymbol(string cardName)
    {
        if (cardName.EndsWith("Spatia")) return "Spatia";
        if (cardName.EndsWith("Pika")) return "Pika";
        if (cardName.EndsWith("Kupa")) return "Kupa";
        if (cardName.EndsWith("Kare")) return "Kare";
        return string.Empty;
    }
}
