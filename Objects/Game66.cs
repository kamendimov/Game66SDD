namespace Cantace;

/// <summary>
/// Core game engine for the 66 card game.
/// Manages the 24-card deck, shuffling, distribution, card replacement, play logic, scoring, and trump changes.
/// </summary>
public class Game66
{
    private const int PLAYER_CARDS = 3;
    private const int TRUMP_CARD_INDEX = 12;

    public Card? TrumpCard = null;
    private int PlayCardCount { get; set; } = 0;
    private List<Card> Cards;
    private UserPlayer UserPlayer;
    private ComputerPlayer ComputerPlayer;

    public Game66()
    {
        UserPlayer = new UserPlayer();
        ComputerPlayer = new ComputerPlayer();

        Cards = new List<Card>();
        Cards.Add(new Card { CardName = "AsoPika", CardValue = 11, Index = 1, ImagePath = "Images\\AsoPika.png" });
        Cards.Add(new Card { CardName = "AsoKupa", CardValue = 11, Index = 2, ImagePath = "Images\\AsoKupa.png" });
        Cards.Add(new Card { CardName = "AsoKare", CardValue = 11, Index = 3, ImagePath = "Images\\AsoKare.png" });
        Cards.Add(new Card { CardName = "AsoSpatia", CardValue = 11, Index = 4, ImagePath = "Images\\AsoSpatia.png" });

        Cards.Add(new Card { CardName = "TenPika", CardValue = 10, Index = 5, ImagePath = "Images\\TenPika.png" });
        Cards.Add(new Card { CardName = "TenKupa", CardValue = 10, Index = 6, ImagePath = "Images\\TenKupa.png" });
        Cards.Add(new Card { CardName = "TenKare", CardValue = 10, Index = 7, ImagePath = "Images\\TenKare.png" });
        Cards.Add(new Card { CardName = "TenSpatia", CardValue = 10, Index = 8, ImagePath = "Images\\TenSpatia.png" });

        Cards.Add(new Card { CardName = "PoupPika", CardValue = 4, Index = 9, ImagePath = "Images\\PoupPika.png" });
        Cards.Add(new Card { CardName = "PoupKupa", CardValue = 4, Index = 10, ImagePath = "Images\\PoupKupa.png" });
        Cards.Add(new Card { CardName = "PoupKare", CardValue = 4, Index = 11, ImagePath = "Images\\PoupKare.png" });
        Cards.Add(new Card { CardName = "PoupSpatia", CardValue = 4, Index = 12, ImagePath = "Images\\PoupSpatia.png" });

        Cards.Add(new Card { CardName = "DamaPika", CardValue = 3, Index = 13, ImagePath = "Images\\DamaPika.png" });
        Cards.Add(new Card { CardName = "DamaKupa", CardValue = 3, Index = 14, ImagePath = "Images\\DamaKupa.png" });
        Cards.Add(new Card { CardName = "DamaKare", CardValue = 3, Index = 15, ImagePath = "Images\\DamaKare.png" });
        Cards.Add(new Card { CardName = "DamaSpatia", CardValue = 3, Index = 16, ImagePath = "Images\\DamaSpatia.png" });

        Cards.Add(new Card { CardName = "ValePika", CardValue = 2, Index = 17, ImagePath = "Images\\ValePika.png" });
        Cards.Add(new Card { CardName = "ValeKupa", CardValue = 2, Index = 18, ImagePath = "Images\\ValeKupa.png" });
        Cards.Add(new Card { CardName = "ValeKare", CardValue = 2, Index = 19, ImagePath = "Images\\ValeKare.png" });
        Cards.Add(new Card { CardName = "ValeSpatia", CardValue = 2, Index = 20, ImagePath = "Images\\ValeSpatia.png" });

        Cards.Add(new Card { CardName = "NinePika", CardValue = 0, Index = 21, ImagePath = "Images\\NinePika.png" });
        Cards.Add(new Card { CardName = "NineKupa", CardValue = 0, Index = 22, ImagePath = "Images\\NineKupa.png" });
        Cards.Add(new Card { CardName = "NineKare", CardValue = 0, Index = 23, ImagePath = "Images\\NineKare.png" });
        Cards.Add(new Card { CardName = "NineSpatia", CardValue = 0, Index = 24, ImagePath = "Images\\NineSpatia.png" });
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
        TrumpCard = null;
        UserPlayer.ResetScore();
        ComputerPlayer.ResetScore();
    }

    public void DistributeCards()
    {
        UserPlayer.ClearCards();
        ComputerPlayer.ClearCards();
        PlayCardCount = 0;
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
        TrumpCard = Cards[TRUMP_CARD_INDEX];
    }

    public Card[] GetUserPlayerCards()
    {
        return UserPlayer.GetCards().ToArray();
    }
    public void PlaySelectedCard(Card selectedCard)
    {
        UserPlayer.RemoveCard(selectedCard);
        UserPlayer.CurrentCard = selectedCard;

        Card? computerCard = GetComputerPlayerCardToPlayByCardType(selectedCard);
        if (computerCard != null)
        {
            ComputerPlayer.RemoveCard(computerCard);
        }
        else
        {
            computerCard = GetComputerPlayerCardToPlayByTrumpCard(TrumpCard);
        }
        if (computerCard != null)
        {
            ComputerPlayer.RemoveCard(computerCard);
        }
        else
        {
            computerCard = GetComputerPlayerSmallestCard(computerCard);
        }
        if (computerCard != null)
        {
            ComputerPlayer.RemoveCard(computerCard);
            ComputerPlayer.CurrentCard = computerCard;

            int roundScore = selectedCard.CardValue + computerCard.CardValue;
            if (selectedCard.CardValue > computerCard.CardValue)
            {
                UserPlayer.IncrementScore(roundScore);
            }
            else if (computerCard.CardValue > selectedCard.CardValue)
            {
                ComputerPlayer.IncrementScore(roundScore);
            }

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

    private Card? GetComputerPlayerSmallestCard(Card? computerCard)
    {
        Card? smallestCard = null;
        foreach (Card card in ComputerPlayer.GetCards())
        {
            if (smallestCard == null || card.CardValue < smallestCard.CardValue)
            {
                smallestCard = card;
            }
        }
        return smallestCard;
    }

    private Card? GetComputerPlayerCardToPlayByCardType(Card? selectedCard)
    {
        if (selectedCard == null) return null;
        
        string symbol = GetCardSymbol(selectedCard.CardName);
        
        Card? bestMatch = null;
        foreach (Card computerCard in ComputerPlayer.GetCards())
        {
            if (computerCard.CardName.EndsWith(symbol) && computerCard.CardValue > selectedCard.CardValue)
            {
                if (bestMatch == null || computerCard.CardValue < bestMatch.CardValue)
                {
                    bestMatch = computerCard;
                }
            }
        }
        
        return bestMatch;
    }

    private Card? GetComputerPlayerCardToPlayByTrumpCard(Card? trumpCard)
    {
        if (trumpCard == null) return null;
        
        string symbol = GetCardSymbol(trumpCard.CardName);
        
        Card? bestMatch = null;
        foreach (Card computerCard in ComputerPlayer.GetCards())
        {
            if (computerCard.CardName.EndsWith(symbol))
            {
                if (bestMatch == null || computerCard.CardValue < bestMatch.CardValue)
                {
                    bestMatch = computerCard;
                }
            }
        }
        
        return bestMatch;
    }

    private string GetCardSymbol(string cardName)
    {
        if (cardName.EndsWith("Spatia")) return "Spatia";
        if (cardName.EndsWith("Pika")) return "Pika";
        if (cardName.EndsWith("Kupa")) return "Kupa";
        if (cardName.EndsWith("Kare")) return "Kare";
        return string.Empty;
    }

    public void ChangeTrumpCard()
    {
        if (TrumpCard == null) return;
        
        if (UserPlayer.GetScore() <= ComputerPlayer.GetScore()) return;
        
        string trumpSymbol = GetCardSymbol(TrumpCard.CardName);
        
        Card? zeroValueMatch = null;
        foreach (Card userCard in UserPlayer.GetCards())
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
            UserPlayer.RemoveCard(zeroValueMatch);
            UserPlayer.SetCard(oldTrumpCard);
            TrumpCard = zeroValueMatch;
        }
    }
}
