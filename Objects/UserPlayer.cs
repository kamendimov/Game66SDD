namespace Cantace;

/// <summary>
/// Human player implementation for the 66 game.
/// Represents the user who interacts with the game through the UI.
/// </summary>
public class UserPlayer : Player
{
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
        if (GameClosed) return null;
        if (GetScore() <= computerPlayerScore) return null;
        
        Suit trumpSymbol = TrumpCard.CardName;
        
        Card? zeroValueMatch = null;
        foreach (Card userCard in GetCards())
        {
            if (userCard.CardValue == Rank.Nine && userCard.CardName == trumpSymbol)
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
}
