namespace Cantace;

/// <summary>
/// Computer player implementation for the 66 game.
/// AI strategy determines which card to play in response to the user's move.
/// </summary>
public class ComputerPlayer : Player
{
    public override void CloseTheGame()
    {
    }

    public override Card? ChangeTrumpCard(int computerPlayerScore)
    {
        return null;
    }

    public override void PlayTwenty()
    {
    }

    public override void PlayForty()
    {
    }
}
