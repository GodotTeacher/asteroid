using Godot;
using System.Linq;

public partial class SpawnEnemies : Node2D
{
    #region Exports
    [Export] private PackedScene[] enemyScene;

    [Export] private Timer spawnTimer;
    #endregion

    #region Signal
    [Signal] public delegate void SpawnEnemyEventHandler(Enemy enemy);
    #endregion

    #region Private members
    private RandomNumberGenerator rnd = new RandomNumberGenerator();
    #endregion

    #region Godot methods
    public override void _Ready()
    {
        spawnTimer.Timeout += OnTimerTimeout;
    }
    #endregion

    #region Private methods
    private void OnTimerTimeout()
    {
        int index = (int)rnd.RandfRange(0, enemyScene.Length);
        float screenWith = GetViewportRect().Size.X;
        float xPos = rnd.RandfRange(20, screenWith - 20);
        Enemy enemy = (Enemy)enemyScene[index].Instantiate();
        enemy.GlobalPosition = new Vector2(xPos, -20);
        AddChild(enemy);
        EmitSignal(SignalName.SpawnEnemy, enemy);
    }
    #endregion

    #region Public methods
    public void Stop()
    {
        spawnTimer.Stop();
        Node[] enemies = GetChildren().ToArray();
        foreach (var enemy in enemies)
        {
            enemy.SetPhysicsProcess(false);
            enemy.SetProcess(false);
        }
    } 
    #endregion
}
