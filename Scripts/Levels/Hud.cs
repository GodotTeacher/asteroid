using Godot;
using System;

public partial class Hud : Control
{
    #region Exports
    [Export] private Label scoreLabel;
    [Export] private Button replayButton;
    #endregion

    #region Godot methods
    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed(GameConstantes.INPUT_SHOOT))
        {
            OnButtonPressed();
        }
    }
    #endregion

    #region Private methods
    private void OnButtonPressed()
    {
        GetTree().ReloadCurrentScene();
    } 
    #endregion

    #region Public methods
    public void SetScore(int score)
    {
        scoreLabel.Text = $"Score :  {score}";
        replayButton.Pressed += OnButtonPressed;
    }


    public void ShowButtonReplay()
    {
        replayButton.Visible = true;
    } 
    #endregion
}
