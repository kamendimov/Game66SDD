namespace Cantace;

using System;

/// <summary>
/// Abstract base class for a player in the 66 game.
/// Manages the player's hand of cards, score, and current card.
/// </summary>
public abstract class Player
{
    protected List<Card> Cards;
    private int Score;

    public Card? CurrentCard { get; set; }
    public bool GameClosed { get; set; }
    public bool LastRoundUserWon { get; set; }
    public Card? TrumpCard { get; set; }

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

    public abstract void CloseTheGame();
    public abstract Card? ChangeTrumpCard(int computerPlayerScore);

    public virtual Card? PlayTwenty()
    {
        if (!LastRoundUserWon) return null;
        
        Suit? trumpSymbol = TrumpCard != null ? TrumpCard.CardName : null;
        
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            Card? cardWithValue4 = null;
            Card? cardWithValue3 = null;
            
            foreach (Card card in GetCards())
            {
                if (card.CardName == suit && suit != trumpSymbol)
                {
                    if (card.CardValue == Rank.Poup)
                    {
                        cardWithValue4 = card;
                    }
                    else if (card.CardValue == Rank.Dama)
                    {
                        cardWithValue3 = card;
                    }
                    if (cardWithValue4 != null && cardWithValue3 != null)
                    {
                        return cardWithValue3;
                    }
                }
            }
        }
        
        return null;
    }

    public virtual Card? PlayForty()
    {
        if (!LastRoundUserWon) return null;
        
        if (TrumpCard == null) return null;
        
        Suit trumpSymbol = TrumpCard.CardName;
        
        Card? cardWithValue4 = null;
        Card? cardWithValue3 = null;
        
        foreach (Card card in GetCards())
        {
            Suit symbol = card.CardName;
            
            if (card.CardValue == Rank.Poup && symbol == trumpSymbol)
            {
                cardWithValue4 = card;
            }
            else if (card.CardValue == Rank.Dama && symbol == trumpSymbol)
            {
                cardWithValue3 = card;
            }
        }
        
        return cardWithValue3;
    }

}
