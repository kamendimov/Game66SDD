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
        mGame66.ResetGame();
        mSelectedCardIndex = -1;
        Invalidate();
    }

    private void SetCardsButton_Click(object sender, EventArgs e)
    {
        mGame66.ResetGame();
        mGame66.MixCards();
        mGame66.DistributeCards();
        mSelectedCardIndex = -1;
        Invalidate();
    }

    private void ChangeTrumpCardButton_Click(object sender, EventArgs e)
    {
        mGame66.ChangeTrumpCard();
        Invalidate();
    }

    private void CloseGameButton_Click(object sender, EventArgs e)
    {
        mGame66.GetUserPlayer().CloseTheGame();
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

                if (mGame66.GetComputerPlayerScore() >= Game66.WIN_SCORE)
                {
                    MessageBox.Show("Computer Player won!");
                }
                else if (mGame66.GetUserPlayerScore() >= Game66.WIN_SCORE)
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

        g.DrawString(userScoreText, mFont, mBrush, startX, 10);
        g.DrawString(computerScoreText, mFont, mBrush, startX + userScoreSize.Width + 20, 10);

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
        int catchCardSpacing = 55;
        int catchY = ClientSize.Height - 160 - catchCardHeight - 10;

        int catchX = 10;
        foreach (CatchPoupDama catchItem in mGame66.GetComputerPlayer().CatchPoupDamaList)
        {
            if (catchItem.CardPoup != null)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, catchItem.CardPoup.ImagePath);
                if (File.Exists(imagePath))
                {
                    Image cardImage = Image.FromFile(imagePath);
                    g.DrawImage(cardImage, catchX, catchY, catchCardWidth, catchCardHeight);
                }
            }
            catchX += catchCardSpacing;
            if (catchItem.CardDama != null)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, catchItem.CardDama.ImagePath);
                if (File.Exists(imagePath))
                {
                    Image cardImage = Image.FromFile(imagePath);
                    g.DrawImage(cardImage, catchX, catchY, catchCardWidth, catchCardHeight);
                }
            }
            catchX += catchCardSpacing;
        }

        catchX = ClientSize.Width - 10 - catchCardWidth;
        foreach (CatchPoupDama catchItem in mGame66.GetUserPlayer().CatchPoupDamaList)
        {
            if (catchItem.CardPoup != null)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, catchItem.CardPoup.ImagePath);
                if (File.Exists(imagePath))
                {
                    Image cardImage = Image.FromFile(imagePath);
                    g.DrawImage(cardImage, catchX, catchY, catchCardWidth, catchCardHeight);
                }
            }
            catchX -= catchCardSpacing;
            if (catchItem.CardDama != null)
            {
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, catchItem.CardDama.ImagePath);
                if (File.Exists(imagePath))
                {
                    Image cardImage = Image.FromFile(imagePath);
                    g.DrawImage(cardImage, catchX, catchY, catchCardWidth, catchCardHeight);
                }
            }
            catchX -= catchCardSpacing;
        }
    }

    private void DrawTrumpCard(Graphics g)
    {
        Card? trumpCard = mGame66.TrumpCard;
        if (trumpCard != null)
        {
            string trumpImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, trumpCard.ImagePath);
            if (File.Exists(trumpImagePath))
            {
                Image trumpImage = Image.FromFile(trumpImagePath);
                g.DrawImage(trumpImage, ClientSize.Width - 120, 10, 100, 140);
            }
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
