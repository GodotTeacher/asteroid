using Godot;
using System;

public partial class Player : CharacterBody2D
{
    #region Exports
    [ExportGroup("Requiered Node")]
    [Export] private AnimatedSprite2D animatedSprite2D;

    [Export] private Marker2D markerLeft;

    [Export] private Marker2D markerRight;

    [Export] private PackedScene laserScene;

    [Export] private Timer shootTimer;

    [Export] private AudioStreamPlayer2D shootSound;
    [Export] private AudioStreamPlayer2D explodeSound;
    [ExportGroup("Speed")]
    [Export] private float speed = 150;

    #endregion

    #region Signal
    [Signal] public delegate void OnPlayerDiedEventHandler();
    #endregion

    #region Private members
    private float direction;
    private bool canShoot = true;
    #endregion

    #region Godot methods
    public override void _Ready()
    {
        shootTimer.Timeout += OnTimerTimeOut;
        animatedSprite2D.AnimationFinished += OnAnimationFinished;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 newVelocity = new Vector2(direction * speed, 0);
        Velocity = newVelocity;
        MoveAndSlide();
        Teleporting();
    }

    public override void _Input(InputEvent @event)
    {
        direction = Input.GetAxis(GameConstantes.INPUT_LEFT, GameConstantes.INPUT_RIGHT);
        if (direction < 0) // Vers la gauche
        {
            animatedSprite2D.Play(GameConstantes.ANIM_FLY_LEFT);
        }
        else if (direction > 0)
        {
            animatedSprite2D.Play(GameConstantes.ANIM_FLY_RIGHT);
        }
        else
        {
            animatedSprite2D.Play(GameConstantes.ANIM_FLY_FORWARD);
        }

        if (Input.IsActionJustPressed(GameConstantes.INPUT_SHOOT) && canShoot)
        {
            SpawnLaser();
            canShoot = false;
            shootTimer.Start();
        }
    }
    #endregion

    #region Public methods
    public void Died()
    {
        explodeSound.Play();
        SetPhysicsProcess(false);
        animatedSprite2D.Play(GameConstantes.ANIM_FLY_EXPLODE);
        EmitSignal(SignalName.OnPlayerDied);
    } 
    #endregion

    #region Private methods
    private void SpawnLaser()
    {
        shootSound.Play();
        Laser laserLeft = (Laser)laserScene.Instantiate();
        Laser laserRight = (Laser)laserScene.Instantiate();

        laserLeft.Position = markerLeft.GlobalPosition;
        laserRight.Position = markerRight.GlobalPosition;


        GetParent().AddChild(laserLeft);
        GetParent().AddChild(laserRight);
    }

    private void Teleporting()
    {
        float screenWidth = GetViewportRect().Size.X;
        if (direction < 0 && Position.X < 0) // Gauche
        {
            Position = new Vector2(screenWidth, Position.Y);
        }
        if (direction > 0 && Position.X > screenWidth) // Droite
        {
            Position = new Vector2(0, Position.Y);
        }
    }

    private void OnTimerTimeOut()
    {
        canShoot = true;
    }

    private void OnAnimationFinished()
    {
        GD.Print("Animation finished");
        QueueFree();
    } 
    #endregion
}
