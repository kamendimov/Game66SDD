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
    private Game66 game66;
    private int selectedCardIndex = -1;
    private Timer computerPlayTimer;
    private const int COMPUTER_USER_SELECT_TIME = 2000;

    public PlayScreen()
    {
        InitializeComponent();
        game66 = new Game66();
        selectedCardIndex = -1;

        computerPlayTimer = new Timer();
        computerPlayTimer.Interval = COMPUTER_USER_SELECT_TIME;
        computerPlayTimer.Tick += (s, e) =>
        {
            computerPlayTimer.Stop();
            game66.SetComputerPlayerSelectedCard();
            Invalidate();
        };
    }

    private void ResetGameButton_Click(object sender, EventArgs e)
    {
        game66.ResetGame();
        selectedCardIndex = -1;
        Invalidate();
    }

    private void SetCardsButton_Click(object sender, EventArgs e)
    {
        game66.MixCards();
        game66.DistributeCards();
        selectedCardIndex = -1;
        Invalidate();
    }

    private void ChangeTrumpCardButton_Click(object sender, EventArgs e)
    {
        game66.ChangeTrumpCard();
        Invalidate();
    }

    private void CloseGameButton_Click(object sender, EventArgs e)
    {
        game66.GetUserPlayer().CloseTheGame();
        Invalidate();
    }

    private void PlayScreen_MouseClick(object sender, MouseEventArgs e)
    {
        int cardWidth = 100;
        int cardHeight = 140;
        int cardSpacing = 105;
        int startX = 20;
        int startY = ClientSize.Height - 160;

        Card[] userPlayerCards = game66.GetUserPlayerCards().ToArray();
        for (int i = 0; i < userPlayerCards.Length; i++)
        {
            int cardX = startX + i * cardSpacing;
            if (e.X >= cardX && e.X <= cardX + cardWidth && e.Y >= startY && e.Y <= startY + cardHeight)
            {
                Card selectedCard = userPlayerCards[i];
                game66.SetUserPlayerSelectedCard(selectedCard);
                
                selectedCardIndex = i;
                Invalidate();

                if (game66.GetComputerPlayerScore() >= Game66.WIN_SCORE)
                {
                    MessageBox.Show("Computer Player won!");
                }
                else if (game66.GetUserPlayerScore() >= Game66.WIN_SCORE)
                {
                    MessageBox.Show("User player won!");
                }
                else if (game66.PlayCardCount > 0 && !game66.GetUserPlayer().LastRoundUserWon)
                {
                    computerPlayTimer.Start();
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
        int userScore = game66.GetUserPlayerScore();
        int computerScore = game66.GetComputerPlayerScore();

        string userScoreText = $"You: {userScore}";
        string computerScoreText = $"Computer: {computerScore}";

        using SolidBrush brush = new SolidBrush(Color.Black);
        using Font font = new Font("Arial", 16, FontStyle.Bold);

        SizeF userScoreSize = g.MeasureString(userScoreText, font);
        SizeF computerScoreSize = g.MeasureString(computerScoreText, font);

        float totalWidth = userScoreSize.Width + 20 + computerScoreSize.Width;
        float startX = (ClientSize.Width - totalWidth) / 2;

        g.DrawString(userScoreText, font, brush, startX, 10);
        g.DrawString(computerScoreText, font, brush, startX + userScoreSize.Width + 20, 10);

        Card? userCard = game66.GetUserPlayerCurrentCard();
        Card? computerCard = game66.GetComputerPlayerCurrentCard();

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
        foreach (CatchPoupDama catchItem in game66.GetComputerPlayer().CatchPoupDamaList)
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
        foreach (CatchPoupDama catchItem in game66.GetUserPlayer().CatchPoupDamaList)
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
        Card? trumpCard = game66.GetUserPlayer().TrumpCard;
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
        userPlayerCards.AddRange(game66.GetUserPlayerCards());
        foreach (Card card in userPlayerCards)
        {
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, card.ImagePath);
            if (File.Exists(imagePath))
            {
                Image cardImage = Image.FromFile(imagePath);
                g.DrawImage(cardImage, x, y, 100, 140);

                if (userPlayerCards.IndexOf(card) == selectedCardIndex)
                {
                    using Pen highlightPen = new Pen(Color.Yellow, 4);
                    g.DrawRectangle(highlightPen, x, y, 100, 140);
                }
            }
            x += 105;
        }
    }
}
