namespace Cantace;

/// <summary>
/// Human player implementation for the 66 game.
/// Represents the user who interacts with the game through the UI.
/// </summary>
public class UserPlayer : Player
{
    public override bool CloseTheGame()
    {
        if (GetScore() >= 1)
        {
            GameClosed = true;
            return true;
        }
        return false;
    }

    public override Card? ChangeTrumpCard(int computerPlayerScore, Card? trumpCard)
    {
        if (trumpCard == null)
        {
            return null;
        }
        if (GameClosed)
        {
            return null;
        }
        if (GetScore() <= computerPlayerScore)
        {
            return null;
        }
        
        Card? zeroValueMatch = null;
        foreach (Card userCard in GetCards())
        {
            if (userCard.CardValue == Rank.Nine && userCard.CardName == trumpCard.CardName)
            {
                zeroValueMatch = userCard;
                break;
            }
        }
        
        if (zeroValueMatch != null)
        {
            Card oldTrumpCard = trumpCard;
            RemoveCard(zeroValueMatch);
            SetCard(oldTrumpCard);
            trumpCard = zeroValueMatch;
        }
        
        return trumpCard;
    }
}
