namespace Cantace;

using System.Linq;

/// <summary>
/// Computer player implementation for the 66 game.
/// AI strategy determines which card to play in response to the user's move.
/// </summary>
public class ComputerPlayer : Player
{
    public override void CloseTheGame()
    {
    }

    public override Card? ChangeTrumpCard(int computerPlayerScore)
    {
        return null;
    }

    public override Card? PlayTwenty()
    {
        return null;
    }

    public override Card? PlayForty()
    {
        return null;
    }

    public Card? GetComputerPlayerSmallestCard(Card? computerCard)
    {
        Card? smallestCard = null;
        foreach (Card card in GetCards())
        {
            if (smallestCard == null || (int)card.CardValue < (int)smallestCard.CardValue)
            {
                smallestCard = card;
            }
        }
        return smallestCard;
    }

    public Card? GetComputerPlayerCardToPlayByCardType(Card? selectedCard)
    {
        if (selectedCard == null) return null;
        
        Suit symbol = selectedCard.CardName;
        
        Card? bestMatch = null;
        foreach (Card computerCard in GetCards())
        {
            if (computerCard.CardName == symbol && (int)computerCard.CardValue > (int)selectedCard.CardValue)
            {
                if (bestMatch == null || (int)computerCard.CardValue < (int)bestMatch.CardValue)
                {
                    bestMatch = computerCard;
                }
            }
        }
        
        return bestMatch;
    }

    public Card? GetComputerPlayerCardToPlayByTrumpCard(Card? trumpCard)
    {
        if (trumpCard == null) return null;
        
        Suit symbol = trumpCard.CardName;
        
        Card? bestMatch = null;
        foreach (Card computerCard in GetCards())
        {
            if (computerCard.CardName == symbol)
            {
                if (bestMatch == null || (int)computerCard.CardValue < (int)bestMatch.CardValue)
                {
                    bestMatch = computerCard;
                }
            }
        }
        
        return bestMatch;
    }

    public Card? SetNextComputerPlayerCard(Card selectedCard, Card? trumpCard)
    {
        Card? computerCard = GetComputerPlayerCardToPlayByCardType(selectedCard);
        if (computerCard != null)
        {
            RemoveCard(computerCard);
        }
        else
        {
            computerCard = GetComputerPlayerCardToPlayByTrumpCard(trumpCard);
        }
        if (computerCard != null)
        {
            RemoveCard(computerCard);
        }
        else
        {
            computerCard = GetComputerPlayerSmallestCard(computerCard);
        }
        return computerCard;
    }
}
