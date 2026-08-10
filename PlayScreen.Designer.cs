namespace Cantace;

partial class PlayScreen
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        ResetGameButton = new Button();
        SetCardsButton = new Button();
        ChangeTrumpCardButton = new Button();
        SetTwentyButton = new Button();
        SetFortyButton = new Button();
        SuspendLayout();
        // 
        // ResetGameButton
        // 
        ResetGameButton.Location = new Point(10, 10);
        ResetGameButton.Name = "ResetGameButton";
        ResetGameButton.Size = new Size(120, 60);
        ResetGameButton.TabIndex = 0;
        ResetGameButton.Text = "Reset\r\nGame";
        ResetGameButton.UseVisualStyleBackColor = true;
        ResetGameButton.Click += ResetGameButton_Click;
        // 
        // SetCardsButton
        // 
        SetCardsButton.Location = new Point(10, 70);
        SetCardsButton.Name = "SetCardsButton";
        SetCardsButton.Size = new Size(120, 60);
        SetCardsButton.TabIndex = 1;
        SetCardsButton.Text = "Set\r\nCards";
        SetCardsButton.UseVisualStyleBackColor = true;
        SetCardsButton.Click += SetCardsButton_Click;
        // 
        // ChangeTrumpCardButton
        // 
        ChangeTrumpCardButton.Location = new Point(10, 130);
        ChangeTrumpCardButton.Name = "ChangeTrumpCardButton";
        ChangeTrumpCardButton.Size = new Size(120, 60);
        ChangeTrumpCardButton.TabIndex = 2;
        ChangeTrumpCardButton.Text = "Change\r\nTrump";
        ChangeTrumpCardButton.UseVisualStyleBackColor = true;
        ChangeTrumpCardButton.Click += ChangeTrumpCardButton_Click;
        // 
        // SetTwentyButton
        // 
        SetTwentyButton.Location = new Point(10, 190);
        SetTwentyButton.Name = "SetTwentyButton";
        SetTwentyButton.Size = new Size(120, 60);
        SetTwentyButton.TabIndex = 3;
        SetTwentyButton.Text = "Set\r\nTwenty";
        SetTwentyButton.UseVisualStyleBackColor = true;
        SetTwentyButton.Click += SetTwentyButton_Click;
        // 
        // SetFortyButton
        // 
        SetFortyButton.Location = new Point(10, 250);
        SetFortyButton.Name = "SetFortyButton";
        SetFortyButton.Size = new Size(120, 60);
        SetFortyButton.TabIndex = 4;
        SetFortyButton.Text = "Set\r\nForty";
        SetFortyButton.UseVisualStyleBackColor = true;
        SetFortyButton.Click += SetFourtyButton_Click;
        this.Paint += PlayScreen_Paint;
        this.MouseClick += PlayScreen_MouseClick;
        // 
        // PlayScreen
        // 
        AutoScaleMode = AutoScaleMode.None;
        ClientSize = new Size(1200, 800);
        Controls.Add(ResetGameButton);
        Controls.Add(SetCardsButton);
        Controls.Add(ChangeTrumpCardButton);
        Controls.Add(SetTwentyButton);
        Controls.Add(SetFortyButton);
        DoubleBuffered = true;
        Font = new Font("Arial", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "PlayScreen";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PlayScreen";
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Button ResetGameButton;
    private System.Windows.Forms.Button SetCardsButton;
    private System.Windows.Forms.Button ChangeTrumpCardButton;
    private System.Windows.Forms.Button SetTwentyButton;
    private System.Windows.Forms.Button SetFortyButton;
}
