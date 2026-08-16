namespace Cantace;

using System;

/// <summary>
/// Abstract base class for a player in the 66 game.
/// Manages the player's hand of cards, score, and current card.
/// </summary>
public abstract class Player
{
    protected List<Card> Cards;
    private int mScore;
    private const int sTWENTY = 20;
    private const int sFORTY = 40;

    public Card? CurrentCard { get; set; }
    public bool GameClosed { get; set; }
    public bool LastRoundUserWon { get; set; }
    public List<CatchPoupDama> CatchPoupDamaList { get; set; }

    protected Player()
    {
        Cards = new List<Card>();
        CatchPoupDamaList = new List<CatchPoupDama>();
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
        mScore += value;
    }

    public int GetScore()
    {
        return mScore;
    }

    public void ResetScore()
    {
        mScore = 0;
    }

    public abstract void CloseTheGame();
    public abstract Card? ChangeTrumpCard(int computerPlayerScore, Card? trumpCard);

    public virtual Card? PlayTwenty(Card? trumpCard)
    {
        if (!LastRoundUserWon)
        {
            return null;
        }
        
        Suit? trumpSymbol = trumpCard != null ? trumpCard.CardName : null;
        
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
                        IncrementScore(sTWENTY);
                        return cardWithValue4;
                    }
                }
            }
        }
        
        return null;
    }

    public virtual Card? PlayForty(Card? trumpCard)
    {
        if (!LastRoundUserWon)
        {
            return null;
        }
        
        if (trumpCard == null)
        {
            return null;
        }
        
        Card? cardWithValue4 = null;
        Card? cardWithValue3 = null;
        
        foreach (Card card in GetCards())
        {
            if (card.CardValue == Rank.Poup && card.CardName == trumpCard.CardName)
            {
                cardWithValue4 = card;
            }
            else if (card.CardValue == Rank.Dama && card.CardName == trumpCard.CardName)
            {
                cardWithValue3 = card;
            }
            if (cardWithValue4 != null && cardWithValue3 != null)
            {
                IncrementScore(sFORTY);
                return cardWithValue4;
            }
        }
        return null;
    }

    public void CreateCatchPoupDamaForTwenty(Card poupCard)
    {
        Card? damaCard = GetCards().FirstOrDefault(c => c.CardValue == Rank.Dama && c.CardName == poupCard.CardName);
        if (damaCard != null)
        {
            CatchPoupDamaList.Add(new CatchPoupDama
            {
                CardPoup = poupCard,
                CardDama = damaCard,
                IsForty = false
            });
        }
    }

    public void CreateCatchPoupDamaForForty(Card poupCard)
    {
        Card? damaCard = GetCards().FirstOrDefault(c => c.CardValue == Rank.Dama && c.CardName == poupCard.CardName);
        if (damaCard != null)
        {
            CatchPoupDamaList.Add(new CatchPoupDama
            {
                CardPoup = poupCard,
                CardDama = damaCard,
                IsForty = true
            });
        }
    }
}
