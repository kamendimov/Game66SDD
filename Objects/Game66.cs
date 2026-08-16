namespace Cantace;

/// <summary>
/// Core game engine for the 66 card game.
/// Manages the 24-card deck, shuffling, distribution, card replacement, play logic, scoring, and trump changes.
/// </summary>
public class Game66
{
    private const int sPLAYER_CARDS = 3;
    private const int sTRUMP_CARD_INDEX = 12;
    public const int WIN_SCORE = 66;

    public int PlayCardCount { get; private set; } = 0;
    public Card? TrumpCard { get; set; } = null;
    private List<Card> mCards;
    private UserPlayer mUserPlayer;
    private ComputerPlayer mComputerPlayer;

    public Game66()
    {
        mUserPlayer = new UserPlayer();
        mComputerPlayer = new ComputerPlayer();

        mCards = new List<Card>();
        mCards.Add(new Card { CardName = Suit.Pika, CardValue = Rank.Aso, Index = 1, ImagePath = "Images\\AsoPika.png" });
        mCards.Add(new Card { CardName = Suit.Kupa, CardValue = Rank.Aso, Index = 2, ImagePath = "Images\\AsoKupa.png" });
        mCards.Add(new Card { CardName = Suit.Kare, CardValue = Rank.Aso, Index = 3, ImagePath = "Images\\AsoKare.png" });
        mCards.Add(new Card { CardName = Suit.Spatia, CardValue = Rank.Aso, Index = 4, ImagePath = "Images\\AsoSpatia.png" });

        mCards.Add(new Card { CardName = Suit.Pika, CardValue = Rank.Ten, Index = 5, ImagePath = "Images\\TenPika.png" });
        mCards.Add(new Card { CardName = Suit.Kupa, CardValue = Rank.Ten, Index = 6, ImagePath = "Images\\TenKupa.png" });
        mCards.Add(new Card { CardName = Suit.Kare, CardValue = Rank.Ten, Index = 7, ImagePath = "Images\\TenKare.png" });
        mCards.Add(new Card { CardName = Suit.Spatia, CardValue = Rank.Ten, Index = 8, ImagePath = "Images\\TenSpatia.png" });

        mCards.Add(new Card { CardName = Suit.Pika, CardValue = Rank.Poup, Index = 9, ImagePath = "Images\\PoupPika.png" });
        mCards.Add(new Card { CardName = Suit.Kupa, CardValue = Rank.Poup, Index = 10, ImagePath = "Images\\PoupKupa.png" });
        mCards.Add(new Card { CardName = Suit.Kare, CardValue = Rank.Poup, Index = 11, ImagePath = "Images\\PoupKare.png" });
        mCards.Add(new Card { CardName = Suit.Spatia, CardValue = Rank.Poup, Index = 12, ImagePath = "Images\\PoupSpatia.png" });

        mCards.Add(new Card { CardName = Suit.Pika, CardValue = Rank.Dama, Index = 13, ImagePath = "Images\\DamaPika.png" });
        mCards.Add(new Card { CardName = Suit.Kupa, CardValue = Rank.Dama, Index = 14, ImagePath = "Images\\DamaKupa.png" });
        mCards.Add(new Card { CardName = Suit.Kare, CardValue = Rank.Dama, Index = 15, ImagePath = "Images\\DamaKare.png" });
        mCards.Add(new Card { CardName = Suit.Spatia, CardValue = Rank.Dama, Index = 16, ImagePath = "Images\\DamaSpatia.png" });

        mCards.Add(new Card { CardName = Suit.Pika, CardValue = Rank.Vale, Index = 17, ImagePath = "Images\\ValePika.png" });
        mCards.Add(new Card { CardName = Suit.Kupa, CardValue = Rank.Vale, Index = 18, ImagePath = "Images\\ValeKupa.png" });
        mCards.Add(new Card { CardName = Suit.Kare, CardValue = Rank.Vale, Index = 19, ImagePath = "Images\\ValeKare.png" });
        mCards.Add(new Card { CardName = Suit.Spatia, CardValue = Rank.Vale, Index = 20, ImagePath = "Images\\ValeSpatia.png" });

        mCards.Add(new Card { CardName = Suit.Pika, CardValue = Rank.Nine, Index = 21, ImagePath = "Images\\NinePika.png" });
        mCards.Add(new Card { CardName = Suit.Kupa, CardValue = Rank.Nine, Index = 22, ImagePath = "Images\\NineKupa.png" });
        mCards.Add(new Card { CardName = Suit.Kare, CardValue = Rank.Nine, Index = 23, ImagePath = "Images\\NineKare.png" });
        mCards.Add(new Card { CardName = Suit.Spatia, CardValue = Rank.Nine, Index = 24, ImagePath = "Images\\NineSpatia.png" });
    }

    public void MixCards()
    {
        List<Card> shuffled = new List<Card>(mCards);
        mCards.Clear();
        Random random = new Random();

        for (int i = shuffled.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            Card temp = shuffled[i];
            shuffled[i] = shuffled[j];
            shuffled[j] = temp;
        }

        List<Card> result = new List<Card>(shuffled);
        List<Card> firstHalf = result.GetRange(0, 12);
        List<Card> secondHalf = result.GetRange(12, 12);

        result = new List<Card>();
        result.AddRange(secondHalf);
        result.AddRange(firstHalf);
        
        mCards.AddRange(result.ToArray());
    }

    public void ResetGame()
    {
        mCards.Sort((a, b) => a.Index.CompareTo(b.Index));
        mUserPlayer.ClearCards();
        mComputerPlayer.ClearCards();
        mUserPlayer.CurrentCard = null;
        mComputerPlayer.CurrentCard = null;
        TrumpCard = null;
        mUserPlayer.ResetScore();
        mComputerPlayer.ResetScore();
        mUserPlayer.LastRoundUserWon = false;
        mUserPlayer.GameClosed = false;
        mUserPlayer.CatchPoupDamaList.Clear();
        mComputerPlayer.CatchPoupDamaList.Clear();
    }

    public void DistributeCards()
    {
        mUserPlayer.ClearCards();
        mComputerPlayer.ClearCards();
        PlayCardCount = 0;
        mUserPlayer.LastRoundUserWon = false;
        mUserPlayer.GameClosed = false;
        for (int i = 0; i < sPLAYER_CARDS; i++)
        {
            mUserPlayer.SetCard(mCards[i]);
        }
        for (int i = 3; i < sPLAYER_CARDS + 3; i++)
        {
            mComputerPlayer.SetCard(mCards[i]);
        }
        for (int i = 6; i < sPLAYER_CARDS + 6; i++)
        {
            mUserPlayer.SetCard(mCards[i]);
        }
        for (int i = 9; i < sPLAYER_CARDS + 9; i++)
        {
            mComputerPlayer.SetCard(mCards[i]);
        }
        TrumpCard = mCards[sTRUMP_CARD_INDEX];
    }

    public UserPlayer GetUserPlayer()
    {
        return mUserPlayer;
    }

    public ComputerPlayer GetComputerPlayer()
    {
        return mComputerPlayer;
    }

    public Card[] GetUserPlayerCards()
    {
        return mUserPlayer.GetCards().ToArray();
    }

    public void SetUserPlayerSelectedCard(Card selectedCard)
    {
        mUserPlayer.RemoveCard(selectedCard);
        mUserPlayer.CurrentCard = selectedCard;

        Card? computerCard;
        if (PlayCardCount > 0 && !mUserPlayer.LastRoundUserWon)
        {
            computerCard = mComputerPlayer.CurrentCard;
        }
        else
        {
            computerCard = mComputerPlayer.SetNextComputerPlayerCard(selectedCard, TrumpCard);
        }
        if (computerCard != null)
        {
            mComputerPlayer.RemoveCard(computerCard);
            mComputerPlayer.CurrentCard = computerCard;

            bool userCardIsTrump = TrumpCard != null &&
                selectedCard.CardName == TrumpCard.CardName;
            bool computerCardIsTrump = TrumpCard != null &&
                computerCard.CardName == TrumpCard.CardName;

            if (userCardIsTrump && !computerCardIsTrump)
            {
                mUserPlayer.IncrementScore((int)selectedCard.CardValue + (int)computerCard.CardValue);
                mUserPlayer.LastRoundUserWon = true;
                
                Card? fortyCard = mUserPlayer.PlayForty(TrumpCard);
                if (fortyCard != null)
                {
                    SetUserPlayerSelectedCard(fortyCard);
                    mUserPlayer.CreateCatchPoupDamaForForty(fortyCard);
                }
                else
                {
                    Card? twentyCard = mUserPlayer.PlayTwenty(TrumpCard);
                    if (twentyCard != null)
                    {
                        SetUserPlayerSelectedCard(twentyCard);
                        mUserPlayer.CreateCatchPoupDamaForTwenty(twentyCard);
                    }
                }
            }
            else if (computerCardIsTrump && !userCardIsTrump)
            {
                mComputerPlayer.IncrementScore((int)selectedCard.CardValue + (int)computerCard.CardValue);
                mUserPlayer.LastRoundUserWon = false;
            }
            else
            {
                int roundScore = (int)selectedCard.CardValue + (int)computerCard.CardValue;
                if (selectedCard.CardName == computerCard.CardName)
                {
                    if ((int)selectedCard.CardValue > (int)computerCard.CardValue)
                    {
                        mUserPlayer.IncrementScore(roundScore);
                        mUserPlayer.LastRoundUserWon = true;
                        
                        Card? fortyCard = mUserPlayer.PlayForty(TrumpCard);
                        if (fortyCard != null)
                        {
                            SetUserPlayerSelectedCard(fortyCard);
                            mUserPlayer.CreateCatchPoupDamaForForty(fortyCard);
                        }
                        else
                        {
                            Card? twentyCard = mUserPlayer.PlayTwenty(TrumpCard);
                            if (twentyCard != null)
                            {
                                SetUserPlayerSelectedCard(twentyCard);
                                mUserPlayer.CreateCatchPoupDamaForTwenty(twentyCard);
                            }
                        }
                    }
                    else if ((int)computerCard.CardValue > (int)selectedCard.CardValue)
                    {
                        mComputerPlayer.IncrementScore(roundScore);
                        mUserPlayer.LastRoundUserWon = false;
                    }
                }
                else
                {
                    if(mUserPlayer.LastRoundUserWon)
                    {
                        mUserPlayer.IncrementScore(roundScore);
                        mUserPlayer.LastRoundUserWon = true;
                        Card? fortyCard = mUserPlayer.PlayForty(TrumpCard);
                        if (fortyCard != null)
                        {
                            SetUserPlayerSelectedCard(fortyCard);
                            mUserPlayer.CreateCatchPoupDamaForForty(fortyCard);
                        }
                        else
                        {
                            Card? twentyCard = mUserPlayer.PlayTwenty(TrumpCard);
                            if (twentyCard != null)
                            {
                                SetUserPlayerSelectedCard(twentyCard);
                                mUserPlayer.CreateCatchPoupDamaForTwenty(twentyCard);
                            }
                        }
                    }
                    else
                    {
                        mComputerPlayer.IncrementScore(roundScore);
                        mUserPlayer.LastRoundUserWon = false;
                    }
                }
            }

            if ((mUserPlayer.GameClosed && mUserPlayer.GetScore() >= Game66.WIN_SCORE) || 
                (mComputerPlayer.GameClosed && mComputerPlayer.GetScore() >= Game66.WIN_SCORE))
            {
                mUserPlayer.ClearCards();
                mComputerPlayer.ClearCards();
            }

            if (!mUserPlayer.GameClosed && !mComputerPlayer.GameClosed)
            {
                SetPlayersCards();
            }
        }
    }

    private void SetPlayersCards()
    {
        if (mUserPlayer.LastRoundUserWon)
        {
            SetUserPlayerCard();
            SetComputerPlayerCard();
        }
        else
        {
            SetComputerPlayerCard();
            SetUserPlayerCard();
        }
    }

    private void SetUserPlayerCard()
    {
        PlayCardCount++;
        for (int i = sTRUMP_CARD_INDEX + PlayCardCount; i < mCards.Count; i++)
        {
            Card candidate = mCards[i];
            if (!mUserPlayer.GetCards().Contains(candidate) && !mComputerPlayer.GetCards().Contains(candidate))
            {
                mUserPlayer.SetCard(candidate);
                break;
            }
        }
    }

    private void SetComputerPlayerCard()
    {
        PlayCardCount++;
        for (int i = sTRUMP_CARD_INDEX + PlayCardCount; i < mCards.Count; i++)
        {
            Card candidate = mCards[i];
            if (!mUserPlayer.GetCards().Contains(candidate) && !mComputerPlayer.GetCards().Contains(candidate))
            {
                mComputerPlayer.SetCard(candidate);
                break;
            }
        }
    }

    public Card? GetUserPlayerCurrentCard()
    {
        return mUserPlayer.CurrentCard;
    }

    public Card? GetComputerPlayerCurrentCard()
    {
        return mComputerPlayer.CurrentCard;
    }

    public int GetUserPlayerScore()
    {
        return mUserPlayer.GetScore();
    }

    public int GetComputerPlayerScore()
    {
        return mComputerPlayer.GetScore();
    }

    public void SetComputerPlayerSelectedCard()
    {
        Card? selectedCard = null;

        if (TrumpCard != null)
        {
            foreach (Card card in mComputerPlayer.GetCards())
            {
                if (card.CardName == TrumpCard.CardName && card.CardValue == Rank.Aso)
                {
                    selectedCard = card;
                    break;
                }
            }
        }

        if (selectedCard == null)
        {
            Card? fortyCard = mComputerPlayer.PlayForty(TrumpCard);
            if (fortyCard != null)
            {
                selectedCard = fortyCard;
                mComputerPlayer.CreateCatchPoupDamaForForty(fortyCard);
            }
            else
            {
                Card? twentyCard = mComputerPlayer.PlayTwenty(TrumpCard);
                if (twentyCard != null)
                {
                    selectedCard = twentyCard;
                    mComputerPlayer.CreateCatchPoupDamaForTwenty(twentyCard);
                }
            }
        }

        if (selectedCard == null)
        {
            selectedCard = mComputerPlayer.GetComputerPlayerSmallestCard(null);
        }

        if (selectedCard != null)
        {
            mComputerPlayer.RemoveCard(selectedCard);
            mComputerPlayer.CurrentCard = selectedCard;
        }

        mUserPlayer.CurrentCard = null;
    }

    public void ChangeTrumpCard()
    {
        Card? newTrump = mUserPlayer.ChangeTrumpCard(mComputerPlayer.GetScore(), TrumpCard);
        if (newTrump != null)
        {
            TrumpCard = newTrump;
        }
    }
}
