using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    #region Exports
    [ExportGroup("Requiered Node")]
    [Export] private AnimatedSprite2D animatedSprite;

    [Export] private Area2D hitBox;

    [Export] private AudioStreamPlayer2D explodeSound;

    [Export] private VisibleOnScreenNotifier2D visibleOnScreenNotifier;

    [ExportGroup("Speed")]
    [Export] private float speed = 60;
    #endregion

    #region Signal
    [Signal] public delegate void OnEnemyDestroyedEventHandler(Enemy enemy);
    [Signal] public delegate void OnEnemyExitedEventHandler(Enemy enemy);
    #endregion

    #region Godot methods
    public override void _Ready()
    {
        animatedSprite.AnimationFinished += OnAnimationFinished;
        hitBox.BodyEntered += OnBodyEntered;
        visibleOnScreenNotifier.ScreenExited += OnScreenExited;
    }



    public override void _PhysicsProcess(double delta)
    {
        Velocity = new Vector2(0, speed);

        MoveAndSlide();
    }
    #endregion

    #region Public methods
    public void Destroy()
    {
        explodeSound.Play();
        SetPhysicsProcess(false);
        animatedSprite.Play(GameConstantes.ANIM_FLY_EXPLODE);
        EmitSignal(SignalName.OnEnemyDestroyed, this);
    }
    #endregion

    #region Private methods
    private void OnAnimationFinished()
    {
        QueueFree();
    }

    private void OnBodyEntered(Node2D body)
    {
        (body as Player).Died();
        Destroy();
    }

    private void OnScreenExited()
    {
        EmitSignal(SignalName.OnEnemyExited, this);
        QueueFree();
    } 
    #endregion
}
