namespace Cantace;

/// <summary>
/// Human player implementation for the 66 game.
/// Represents the user who interacts with the game through the UI.
/// </summary>
public class UserPlayer : Player
{
    public bool LastRoundUserWon { get; set; }
    public Card? TrumpCard { get; set; }
    public bool GameClosed { get; set; }

    public override void CloseTheGame()
    {
        if (GetScore() >= 1)
        {
            GameClosed = true;
        }
    }

    public override Card? ChangeTrumpCard(int computerPlayerScore)
    {
        if (TrumpCard == null) return null;
        if (GetScore() <= computerPlayerScore) return null;
        
        string trumpSymbol = Player.GetCardSymbol(TrumpCard.CardName);
        
        Card? zeroValueMatch = null;
        foreach (Card userCard in GetCards())
        {
            if (userCard.CardValue == 0 && userCard.CardName.EndsWith(trumpSymbol))
            {
                zeroValueMatch = userCard;
                break;
            }
        }
        
        if (zeroValueMatch != null)
        {
            Card oldTrumpCard = TrumpCard;
            RemoveCard(zeroValueMatch);
            SetCard(oldTrumpCard);
            TrumpCard = zeroValueMatch;
        }
        
        return TrumpCard;
    }

    public override void PlayTwenty()
    {
        if (!LastRoundUserWon) return;
        
        string? trumpSymbol = TrumpCard != null ? Player.GetCardSymbol(TrumpCard.CardName) : null;
        
        Card? cardWithValue4 = null;
        Card? cardWithValue3 = null;
        
        foreach (Card card in GetCards())
        {
            string symbol = Player.GetCardSymbol(card.CardName);
            
            if (card.CardValue == 4 && symbol != trumpSymbol)
            {
                cardWithValue4 = card;
            }
            else if (card.CardValue == 3 && symbol != trumpSymbol)
            {
                cardWithValue3 = card;
            }
        }
        
        if (cardWithValue4 != null && cardWithValue3 != null &&
            Player.GetCardSymbol(cardWithValue4.CardName) == Player.GetCardSymbol(cardWithValue3.CardName))
        {
            IncrementScore(20);
        }
    }

    public override void PlayForty()
    {
        if (!LastRoundUserWon) return;
        
        if (TrumpCard == null) return;
        
        string trumpSymbol = Player.GetCardSymbol(TrumpCard.CardName);
        
        Card? cardWithValue4 = null;
        Card? cardWithValue3 = null;
        
        foreach (Card card in GetCards())
        {
            string symbol = Player.GetCardSymbol(card.CardName);
            
            if (card.CardValue == 4 && symbol == trumpSymbol)
            {
                cardWithValue4 = card;
            }
            else if (card.CardValue == 3 && symbol == trumpSymbol)
            {
                cardWithValue3 = card;
            }
        }
        
        if (cardWithValue4 != null && cardWithValue3 != null &&
            Player.GetCardSymbol(cardWithValue4.CardName) == Player.GetCardSymbol(cardWithValue3.CardName))
        {
            IncrementScore(40);
        }
    }
}
