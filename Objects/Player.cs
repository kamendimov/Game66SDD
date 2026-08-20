namespace Cantace;

using System;

/// <summary>
/// Abstract base class for a player in the 66 game.
/// Manages the player's hand of cards, score, and current card.
/// </summary>
public abstract class Player
{
    protected List<Card> mCards;
    private int mScore;
    private const int sTWENTY = 20;
    private const int sFORTY = 40;

    public Card? CurrentCard { get; set; }
    public bool GameClosed { get; set; }
    public bool LastRoundUserWon { get; set; }
    public List<CatchPoupDama> CatchPoupDamaList { get; set; }

    protected Player()
    {
        mCards = new List<Card>();
        CatchPoupDamaList = new List<CatchPoupDama>();
    }

    public void SetCard(Card card)
    {
        mCards.Add(card);
    }

    public void ClearCards()
    {
        mCards.Clear();
    }

    public void RemoveCard(Card card)
    {
        mCards.Remove(card);
    }

    public List<Card> GetCards()
    {
        return new List<Card>(mCards);
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

    public abstract bool CloseTheGame();
    public abstract Card? ChangeTrumpCard(int computerPlayerScore, Card? trumpCard);

    public virtual Card? PlayTwenty(Card? trumpCard)
    {
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
        CreateCatchPoupDama(poupCard, false);
    }

    public void CreateCatchPoupDamaForForty(Card poupCard)
    {
        CreateCatchPoupDama(poupCard, true);
    }

    private void CreateCatchPoupDama(Card poupCard, bool IsForty)
    {
        Card? damaCard = GetCards().FirstOrDefault(c => c.CardValue == Rank.Dama && c.CardName == poupCard.CardName);
        if (damaCard != null)
        {
            CatchPoupDamaList.Add(new CatchPoupDama
            {
                CardPoup = poupCard,
                CardDama = damaCard,
                IsForty = IsForty
            });
        }
    }
}
