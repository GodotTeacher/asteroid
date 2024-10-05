using Godot;
using System;

public partial class Laser : Area2D
{
    #region Exports
    [ExportGroup("Requiered Node")]
    [Export] VisibleOnScreenNotifier2D visibleOnScreenNotifier2D;

    [ExportGroup("Speed")]
    [Export] private float speed = 100;
    #endregion

    #region Godot methods
    public override void _Ready()
    {
        visibleOnScreenNotifier2D.ScreenExited += OnVisibilityExited;
        BodyEntered += OnBodyEntered;
    }


    public override void _Process(double delta)
    {
        Vector2 newPosition = Position;
        newPosition.Y -= speed * (float)delta;
        Position = newPosition;
    }
    #endregion

    #region Private methods
    private void OnVisibilityExited()
    {
        QueueFree();
    }

    private void OnBodyEntered(Node2D body)
    {
        ((Enemy)body).Destroy();
        QueueFree();
    } 
    #endregion
}
