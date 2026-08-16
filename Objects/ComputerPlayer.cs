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

    public override Card? ChangeTrumpCard(int computerPlayerScore, Card? trumpCard)
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
        if (selectedCard == null)
        {
            return null;
        }
        
        Card? bestMatch = null;
        foreach (Card computerCard in GetCards())
        {
            if (computerCard.CardName == selectedCard.CardName && (int)computerCard.CardValue > (int)selectedCard.CardValue)
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
        if (trumpCard == null)
        {
            return null;
        }
        
        Card? bestMatch = null;
        foreach (Card computerCard in GetCards())
        {
            if (computerCard.CardName == trumpCard.CardName)
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
        if (computerCard == null)
        {
            computerCard = GetComputerPlayerCardToPlayByTrumpCard(trumpCard);
        }
        if (computerCard == null)
        {
            computerCard = GetComputerPlayerSmallestCard(computerCard);
        }
        if (computerCard != null)
        {
            RemoveCard(computerCard);
        }
        return computerCard;
    }
}
