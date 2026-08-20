namespace Cantace;

using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

/// <summary>
/// Main form for the 66 card game.
/// Handles painting, mouse clicks, button events, score rendering, and card display.
/// </summary>
public partial class PlayScreen : Form
{
    private Game66 mGame66;
    private int mSelectedCardIndex = -1;
    private Timer mComputerPlayTimer;
    private const int sCOMPUTER_USER_SELECT_TIME = 2000;
    private SolidBrush mBrush;
    private Font mFont;
    private Pen mHighlightPen;

    public PlayScreen()
    {
        InitializeComponent();
        Height = System.Windows.Forms.Screen.GetWorkingArea(new Point(0, 0)).Height;
        mGame66 = new Game66();
        mSelectedCardIndex = -1;
        mBrush = new SolidBrush(Color.Black);
        mFont = new Font("Arial", 16, FontStyle.Bold);
        mHighlightPen = new Pen(Color.Yellow, 4);

        mComputerPlayTimer = new Timer();
        mComputerPlayTimer.Interval = sCOMPUTER_USER_SELECT_TIME;
        mComputerPlayTimer.Tick += (s, e) =>
        {
            mComputerPlayTimer.Stop();
            mGame66.SetComputerPlayerSelectedCard();
            Invalidate();
        };
    }

    private void ResetGameButton_Click(object sender, EventArgs e)
    {
        //mGame66.ResetGame();
        mGame66 = new Game66();
        mSelectedCardIndex = -1;
        ChangeTrumpCardButton.Enabled = true;
        CloseGameButton.Enabled = true;
        Invalidate();
    }

    private void SetCardsButton_Click(object sender, EventArgs e)
    {
        //mGame66.ResetGame();
        mGame66 = new Game66();
        mGame66.MixCards();
        mGame66.DistributeCards();
        mSelectedCardIndex = -1;
        ChangeTrumpCardButton.Enabled = true;
        CloseGameButton.Enabled = true;
        Invalidate();
    }

    private void ChangeTrumpCardButton_Click(object sender, EventArgs e)
    {
        if (mGame66.ChangeTrumpCard())
        {
            ChangeTrumpCardButton.Enabled = false;
        }
        Invalidate();
    }

    private void CloseGameButton_Click(object sender, EventArgs e)
    {
        if (mGame66.CloseTheGame())
        {
            CloseGameButton.Enabled = false;
        }
        Invalidate();
    }

    private void PlayScreen_MouseClick(object sender, MouseEventArgs e)
    {
        int cardWidth = 100;
        int cardHeight = 140;
        int cardSpacing = 105;
        int startX = 20;
        int startY = ClientSize.Height - 160;

        Card[] userPlayerCards = mGame66.GetUserPlayerCards().ToArray();
        for (int i = 0; i < userPlayerCards.Length; i++)
        {
            int cardX = startX + i * cardSpacing;
            if (e.X >= cardX && e.X <= cardX + cardWidth && e.Y >= startY && e.Y <= startY + cardHeight)
            {
                Card selectedCard = userPlayerCards[i];
                mGame66.SetUserPlayerSelectedCard(selectedCard);
                
                mSelectedCardIndex = i;
                Invalidate();

                if (mGame66.GetComputerPlayerScore() >= Game66.WinScore)
                {
                    MessageBox.Show("Computer Player won!");
                }
                else if (mGame66.GetUserPlayerScore() >= Game66.WinScore)
                {
                    MessageBox.Show("User player won!");
                }
                else if (mGame66.PlayCardCount > 0 && !mGame66.GetUserPlayer().LastRoundUserWon)
                {
                    mComputerPlayTimer.Start();
                }
                break;
            }
        }
    }

    private void PlayScreen_Paint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        DrawUserPlayerCards(g);
        DrawTrumpCard(g);
        DrawPlayScene(g);
        DrawCatchedPoupDama(g);
        DrawPlayedCards(g);
    }

    private void DrawPlayScene(Graphics g)
    {
        int userScore = mGame66.GetUserPlayerScore();
        int computerScore = mGame66.GetComputerPlayerScore();

        string userScoreText = $"You: {userScore}";
        string computerScoreText = $"Computer: {computerScore}";

        SizeF userScoreSize = g.MeasureString(userScoreText, mFont);
        SizeF computerScoreSize = g.MeasureString(computerScoreText, mFont);

        float totalWidth = userScoreSize.Width + 20 + computerScoreSize.Width;
        float startX = (ClientSize.Width - totalWidth) / 2;

        g.DrawString(computerScoreText, mFont, mBrush, startX, 10);
        g.DrawString(userScoreText, mFont, mBrush, startX + computerScoreSize.Width + 20, 10);

        Card? userCard = mGame66.GetUserPlayerCurrentCard();
        Card? computerCard = mGame66.GetComputerPlayerCurrentCard();

        int middleX = (ClientSize.Width - 220) / 2;
        int middleY = (ClientSize.Height - 140) / 2;

        if (userCard != null)
        {
            string userImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, userCard.ImagePath);
            if (File.Exists(userImagePath))
            {
                Image userImage = Image.FromFile(userImagePath);
                g.DrawImage(userImage, middleX, middleY, 100, 140);
            }
        }

        if (computerCard != null)
        {
            string computerImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, computerCard.ImagePath);
            if (File.Exists(computerImagePath))
            {
                Image computerImage = Image.FromFile(computerImagePath);
                g.DrawImage(computerImage, middleX + 120, middleY, 100, 140);
            }
        }
    }

    private void DrawCatchedPoupDama(Graphics g)
    {
        int catchCardWidth = 50;
        int catchCardHeight = 70;
        int catchCardSpacing = 10;
        int catchY = ClientSize.Height - 330 - catchCardHeight - 10;

        int catchX = 70;
        foreach (CatchPoupDama catchItem in mGame66.GetComputerPlayer().CatchPoupDamaList)
        {
            if (catchItem.CardPoup != null)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, catchItem.CardPoup.ImagePath);
                if (File.Exists(imagePath))
                {
                    Image cardImage = Image.FromFile(imagePath);
                    g.DrawImage(cardImage, catchX - catchCardWidth - catchCardSpacing, catchY, catchCardWidth, catchCardHeight);
                }
            }

            if (catchItem.CardDama != null)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, catchItem.CardDama.ImagePath);
                if (File.Exists(imagePath))
                {
                    Image cardImage = Image.FromFile(imagePath);
                    g.DrawImage(cardImage, catchX, catchY, catchCardWidth, catchCardHeight);
                }
            }

            catchY += catchCardHeight + catchCardSpacing;
        }
        
        catchX = ClientSize.Width - 10 - catchCardWidth;
        catchY = ClientSize.Height - 330 - catchCardHeight - 10;
        foreach (CatchPoupDama catchItem in mGame66.GetUserPlayer().CatchPoupDamaList)
        {
            if (catchItem.CardPoup != null)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, catchItem.CardPoup.ImagePath);
                if (File.Exists(imagePath))
                {
                    Image cardImage = Image.FromFile(imagePath);
                    g.DrawImage(cardImage, catchX - catchCardWidth - catchCardSpacing, catchY, catchCardWidth, catchCardHeight);
                }
            }

            if (catchItem.CardDama != null)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, catchItem.CardDama.ImagePath);
                if (File.Exists(imagePath))
                {
                    Image cardImage = Image.FromFile(imagePath);
                    g.DrawImage(cardImage, catchX, catchY, catchCardWidth, catchCardHeight);
                }
            }

            catchY += catchCardHeight + catchCardSpacing;
        }
    }

    private void DrawTrumpCard(Graphics g)
    {
        if (!mGame66.DoesTrumpCardPlay() && mGame66.TrumpCard != null)
        {
            string trumpImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, mGame66.TrumpCard.ImagePath);
            if (File.Exists(trumpImagePath))
            {
                Image trumpImage = Image.FromFile(trumpImagePath);
                g.DrawImage(trumpImage, ClientSize.Width - 120, 10, 100, 140);
            }
        }
        else if (mGame66.TrumpCard != null)
        {
            Image suitImage;
            if (mGame66.TrumpCard.CardName == Suit.Kare)
            {
                suitImage = Image.FromFile("Images\\Kare.png");
            }
            else if (mGame66.TrumpCard.CardName == Suit.Kupa)
            {
                suitImage = Image.FromFile("Images\\Kupa.png");
            }
            else if (mGame66.TrumpCard.CardName == Suit.Pika)
            {
                suitImage = Image.FromFile("Images\\Pika.png");
            }
            else
            {
                suitImage = Image.FromFile("Images\\Spatia.png");
            }
            g.DrawImage(suitImage, ClientSize.Width - 120, 10, 100, 140);
        }
    }

    private void DrawPlayedCards(Graphics g)
    {
        int cardWidth = 50;
        int cardHeight = 70;
        int leftX = 140;
        int rightX = ClientSize.Width - 240;
        int y = 10;

        foreach (PlayedCardsPair pair in mGame66.mPlayedCards)
        {
            if (pair.ComputerPlayerCard != null)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pair.ComputerPlayerCard.ImagePath);
                if (File.Exists(imagePath))
                {
                    Image cardImage = Image.FromFile(imagePath);
                    g.DrawImage(cardImage, leftX, y, cardWidth, cardHeight);
                }

                if (pair.ComputerPlayerResult != null)
                {
                    string resultText = $"+{pair.ComputerPlayerResult}";
                    g.DrawString(resultText, mFont, mBrush, leftX + cardWidth + 5, y + 5);
                }
            }

            if (pair.UserPlayerCard != null)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pair.UserPlayerCard.ImagePath);
                if (File.Exists(imagePath))
                {
                    Image cardImage = Image.FromFile(imagePath);
                    g.DrawImage(cardImage, rightX, y, cardWidth, cardHeight);
                }

                if (pair.UserPlayerResult != null)
                {
                    string resultText = $"+{pair.UserPlayerResult}";
                    g.DrawString(resultText, mFont, mBrush, rightX + cardWidth + 5, y + 5);
                }
            }

            y += cardHeight + 10;
        }
    }

    private void DrawUserPlayerCards(Graphics g)
    {
        int x = 20;
        int y = ClientSize.Height - 160;

        List<Card> userPlayerCards = new List<Card>();
        userPlayerCards.AddRange(mGame66.GetUserPlayerCards());
        foreach (Card card in userPlayerCards)
        {
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, card.ImagePath);
            if (File.Exists(imagePath))
            {
                Image cardImage = Image.FromFile(imagePath);
                g.DrawImage(cardImage, x, y, 100, 140);

                if (userPlayerCards.IndexOf(card) == mSelectedCardIndex)
                {
                    g.DrawRectangle(mHighlightPen, x, y, 100, 140);
                }
            }
            x += 105;
        }
    }
}
