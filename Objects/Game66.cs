namespace Cantace;

/// <summary>
/// Core game engine for the 66 card game.
/// Manages the 24-card deck, shuffling, distribution, card replacement, play logic, scoring, and trump changes.
/// </summary>
public class Game66
{
    private const int PLAYER_CARDS = 3;
    private const int TRUMP_CARD_INDEX = 12;
    public const int WIN_SCORE = 66;

    public int PlayCardCount { get; private set; } = 0;
    private List<Card> Cards;
    private UserPlayer UserPlayer;
    private ComputerPlayer ComputerPlayer;

    public Game66()
    {
        UserPlayer = new UserPlayer();
        ComputerPlayer = new ComputerPlayer();

        Cards = new List<Card>();
        Cards.Add(new Card { CardName = Suit.Pika, CardValue = Rank.Aso, Index = 1, ImagePath = "Images\\AsoPika.png" });
        Cards.Add(new Card { CardName = Suit.Kupa, CardValue = Rank.Aso, Index = 2, ImagePath = "Images\\AsoKupa.png" });
        Cards.Add(new Card { CardName = Suit.Kare, CardValue = Rank.Aso, Index = 3, ImagePath = "Images\\AsoKare.png" });
        Cards.Add(new Card { CardName = Suit.Spatia, CardValue = Rank.Aso, Index = 4, ImagePath = "Images\\AsoSpatia.png" });

        Cards.Add(new Card { CardName = Suit.Pika, CardValue = Rank.Ten, Index = 5, ImagePath = "Images\\TenPika.png" });
        Cards.Add(new Card { CardName = Suit.Kupa, CardValue = Rank.Ten, Index = 6, ImagePath = "Images\\TenKupa.png" });
        Cards.Add(new Card { CardName = Suit.Kare, CardValue = Rank.Ten, Index = 7, ImagePath = "Images\\TenKare.png" });
        Cards.Add(new Card { CardName = Suit.Spatia, CardValue = Rank.Ten, Index = 8, ImagePath = "Images\\TenSpatia.png" });

        Cards.Add(new Card { CardName = Suit.Pika, CardValue = Rank.Poup, Index = 9, ImagePath = "Images\\PoupPika.png" });
        Cards.Add(new Card { CardName = Suit.Kupa, CardValue = Rank.Poup, Index = 10, ImagePath = "Images\\PoupKupa.png" });
        Cards.Add(new Card { CardName = Suit.Kare, CardValue = Rank.Poup, Index = 11, ImagePath = "Images\\PoupKare.png" });
        Cards.Add(new Card { CardName = Suit.Spatia, CardValue = Rank.Poup, Index = 12, ImagePath = "Images\\PoupSpatia.png" });

        Cards.Add(new Card { CardName = Suit.Pika, CardValue = Rank.Dama, Index = 13, ImagePath = "Images\\DamaPika.png" });
        Cards.Add(new Card { CardName = Suit.Kupa, CardValue = Rank.Dama, Index = 14, ImagePath = "Images\\DamaKupa.png" });
        Cards.Add(new Card { CardName = Suit.Kare, CardValue = Rank.Dama, Index = 15, ImagePath = "Images\\DamaKare.png" });
        Cards.Add(new Card { CardName = Suit.Spatia, CardValue = Rank.Dama, Index = 16, ImagePath = "Images\\DamaSpatia.png" });

        Cards.Add(new Card { CardName = Suit.Pika, CardValue = Rank.Vale, Index = 17, ImagePath = "Images\\ValePika.png" });
        Cards.Add(new Card { CardName = Suit.Kupa, CardValue = Rank.Vale, Index = 18, ImagePath = "Images\\ValeKupa.png" });
        Cards.Add(new Card { CardName = Suit.Kare, CardValue = Rank.Vale, Index = 19, ImagePath = "Images\\ValeKare.png" });
        Cards.Add(new Card { CardName = Suit.Spatia, CardValue = Rank.Vale, Index = 20, ImagePath = "Images\\ValeSpatia.png" });

        Cards.Add(new Card { CardName = Suit.Pika, CardValue = Rank.Nine, Index = 21, ImagePath = "Images\\NinePika.png" });
        Cards.Add(new Card { CardName = Suit.Kupa, CardValue = Rank.Nine, Index = 22, ImagePath = "Images\\NineKupa.png" });
        Cards.Add(new Card { CardName = Suit.Kare, CardValue = Rank.Nine, Index = 23, ImagePath = "Images\\NineKare.png" });
        Cards.Add(new Card { CardName = Suit.Spatia, CardValue = Rank.Nine, Index = 24, ImagePath = "Images\\NineSpatia.png" });
    }

    public void MixCards()
    {
        List<Card> shuffled = new List<Card>(Cards);
        Cards.Clear();
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
        
        Cards.AddRange(result.ToArray());
    }

    public void ResetGame()
    {
        Cards.Sort((a, b) => a.Index.CompareTo(b.Index));
        UserPlayer.ClearCards();
        ComputerPlayer.ClearCards();
        UserPlayer.CurrentCard = null;
        ComputerPlayer.CurrentCard = null;
        UserPlayer.TrumpCard = null;
        UserPlayer.ResetScore();
        ComputerPlayer.ResetScore();
        UserPlayer.LastRoundUserWon = false;
        UserPlayer.GameClosed = false;
    }

    public void DistributeCards()
    {
        UserPlayer.ClearCards();
        ComputerPlayer.ClearCards();
        PlayCardCount = 0;
        UserPlayer.LastRoundUserWon = false;
        UserPlayer.GameClosed = false;
        for (int i = 0; i < PLAYER_CARDS; i++)
        {
            UserPlayer.SetCard(Cards[i]);
        }
        for (int i = 3; i < PLAYER_CARDS + 3; i++)
        {
            ComputerPlayer.SetCard(Cards[i]);
        }
        for (int i = 6; i < PLAYER_CARDS + 6; i++)
        {
            UserPlayer.SetCard(Cards[i]);
        }
        for (int i = 9; i < PLAYER_CARDS + 9; i++)
        {
            ComputerPlayer.SetCard(Cards[i]);
        }
        UserPlayer.TrumpCard = Cards[TRUMP_CARD_INDEX];
    }

    public UserPlayer GetUserPlayer()
    {
        return UserPlayer;
    }

    public ComputerPlayer GetComputerPlayer()
    {
        return ComputerPlayer;
    }

    public Card[] GetUserPlayerCards()
    {
        return UserPlayer.GetCards().ToArray();
    }

    public void SetUserPlayerSelectedCard(Card selectedCard)
    {
        UserPlayer.RemoveCard(selectedCard);
        UserPlayer.CurrentCard = selectedCard;

        Card? computerCard;
        if (PlayCardCount > 0 && !UserPlayer.LastRoundUserWon)
        {
            computerCard = ComputerPlayer.CurrentCard;
        }
        else
        {
            computerCard = ComputerPlayer.SetNextComputerPlayerCard(selectedCard, UserPlayer.TrumpCard);
        }
        if (computerCard != null)
        {
            ComputerPlayer.RemoveCard(computerCard);
            ComputerPlayer.CurrentCard = computerCard;

            bool userCardIsTrump = UserPlayer.TrumpCard != null &&
                selectedCard.CardName == UserPlayer.TrumpCard.CardName;
            bool computerCardIsTrump = UserPlayer.TrumpCard != null &&
                computerCard.CardName == UserPlayer.TrumpCard.CardName;

            if (userCardIsTrump && !computerCardIsTrump)
            {
                UserPlayer.IncrementScore((int)selectedCard.CardValue + (int)computerCard.CardValue);
                UserPlayer.LastRoundUserWon = true;
                
                Card? fortyCard = UserPlayer.PlayForty();
                if (fortyCard != null)
                {
                    SetUserPlayerSelectedCard(fortyCard);
                    UserPlayer.CreateCatchPoupDamaForForty(fortyCard);
                }
                else
                {
                    Card? twentyCard = UserPlayer.PlayTwenty();
                    if (twentyCard != null)
                    {
                        SetUserPlayerSelectedCard(twentyCard);
                        UserPlayer.CreateCatchPoupDamaForTwenty(twentyCard);
                    }
                }
            }
            else if (computerCardIsTrump && !userCardIsTrump)
            {
                ComputerPlayer.IncrementScore((int)selectedCard.CardValue + (int)computerCard.CardValue);
                UserPlayer.LastRoundUserWon = false;
            }
            else
            {
                int roundScore = (int)selectedCard.CardValue + (int)computerCard.CardValue;
                if (selectedCard.CardName == computerCard.CardName)
                {
                    if ((int)selectedCard.CardValue > (int)computerCard.CardValue)
                    {
                        UserPlayer.IncrementScore(roundScore);
                        UserPlayer.LastRoundUserWon = true;
                        
                        Card? fortyCard = UserPlayer.PlayForty();
                        if (fortyCard != null)
                        {
                            SetUserPlayerSelectedCard(fortyCard);
                            UserPlayer.CreateCatchPoupDamaForForty(fortyCard);
                        }
                        else
                        {
                            Card? twentyCard = UserPlayer.PlayTwenty();
                            if (twentyCard != null)
                            {
                                SetUserPlayerSelectedCard(twentyCard);
                                UserPlayer.CreateCatchPoupDamaForTwenty(twentyCard);
                            }
                        }
                    }
                    else if ((int)computerCard.CardValue > (int)selectedCard.CardValue)
                    {
                        ComputerPlayer.IncrementScore(roundScore);
                        UserPlayer.LastRoundUserWon = false;
                    }
                }
                else
                {
                    if(UserPlayer.LastRoundUserWon)
                    {
                        UserPlayer.IncrementScore(roundScore);
                        UserPlayer.LastRoundUserWon = true;
                        Card? fortyCard = UserPlayer.PlayForty();
                        if (fortyCard != null)
                        {
                            SetUserPlayerSelectedCard(fortyCard);
                            UserPlayer.CreateCatchPoupDamaForForty(fortyCard);
                        }
                        else
                        {
                            Card? twentyCard = UserPlayer.PlayTwenty();
                            if (twentyCard != null)
                            {
                                SetUserPlayerSelectedCard(twentyCard);
                                UserPlayer.CreateCatchPoupDamaForTwenty(twentyCard);
                            }
                        }
                    }
                    else
                    {
                        ComputerPlayer.IncrementScore(roundScore);
                        UserPlayer.LastRoundUserWon = false;
                    }
                }
            }

            if ((UserPlayer.GameClosed && UserPlayer.GetScore() >= Game66.WIN_SCORE) || 
                (ComputerPlayer.GameClosed && ComputerPlayer.GetScore() >= Game66.WIN_SCORE))
            {
                UserPlayer.ClearCards();
                ComputerPlayer.ClearCards();
            }

            if (!UserPlayer.GameClosed && !ComputerPlayer.GameClosed)
            {
                SetPlayersCards();
            }
        }
    }

    private void SetPlayersCards()
    {
        if (UserPlayer.LastRoundUserWon)
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
        for (int i = TRUMP_CARD_INDEX + PlayCardCount; i < Cards.Count; i++)
        {
            Card candidate = Cards[i];
            if (!UserPlayer.GetCards().Contains(candidate) && !ComputerPlayer.GetCards().Contains(candidate))
            {
                UserPlayer.SetCard(candidate);
                break;
            }
        }
    }

    private void SetComputerPlayerCard()
    {
        PlayCardCount++;
        for (int i = TRUMP_CARD_INDEX + PlayCardCount; i < Cards.Count; i++)
        {
            Card candidate = Cards[i];
            if (!UserPlayer.GetCards().Contains(candidate) && !ComputerPlayer.GetCards().Contains(candidate))
            {
                ComputerPlayer.SetCard(candidate);
                break;
            }
        }
    }

    public Card? GetUserPlayerCurrentCard()
    {
        return UserPlayer.CurrentCard;
    }

    public Card? GetComputerPlayerCurrentCard()
    {
        return ComputerPlayer.CurrentCard;
    }

    public int GetUserPlayerScore()
    {
        return UserPlayer.GetScore();
    }

    public int GetComputerPlayerScore()
    {
        return ComputerPlayer.GetScore();
    }

    public void SetComputerPlayerSelectedCard()
    {
        Card? selectedCard = null;

        if (UserPlayer.TrumpCard != null)
        {
            Suit trumpSymbol = UserPlayer.TrumpCard.CardName;
            foreach (Card card in ComputerPlayer.GetCards())
            {
                if (card.CardName == trumpSymbol && card.CardValue == Rank.Aso)
                {
                    selectedCard = card;
                    break;
                }
            }
        }

        if (selectedCard == null)
        {
            Card? fortyCard = ComputerPlayer.PlayForty();
            if (fortyCard != null)
            {
                selectedCard = fortyCard;
                ComputerPlayer.CreateCatchPoupDamaForForty(fortyCard);
            }
            else
            {
                    Card? twentyCard = ComputerPlayer.PlayTwenty();
                    if (twentyCard != null)
                    {
                        selectedCard = twentyCard;
                        ComputerPlayer.CreateCatchPoupDamaForTwenty(twentyCard);
                    }
            }
        }

        if (selectedCard == null)
        {
            selectedCard = ComputerPlayer.GetComputerPlayerSmallestCard(null);
        }

        if (selectedCard != null)
        {
            ComputerPlayer.RemoveCard(selectedCard);
            ComputerPlayer.CurrentCard = selectedCard;
        }

        UserPlayer.CurrentCard = null;
    }

    public void ChangeTrumpCard()
    {
        UserPlayer.ChangeTrumpCard(ComputerPlayer.GetScore());
    }
}
