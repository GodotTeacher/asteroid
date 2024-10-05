using Godot;
using System;


public partial class Level : Node2D
{
    #region Exports
    [Export] private SpawnEnemies spawn;

    [Export] private Player player;

    [Export] private Hud hud;
    #endregion

    #region Private members
    private int score = 0;
    #endregion

    #region Godot methods
    public override void _Ready()
    {
        spawn.SpawnEnemy += OnSpawnEnemy;
        player.OnPlayerDied += PlayerDied;
    }
    #endregion

    #region Private methods
    private void PlayerDied()
    {
        spawn.Stop();
        hud.ShowButtonReplay();
    }

    private void OnSpawnEnemy(Enemy enemy)
    {
        enemy.OnEnemyDestroyed += EnemyDestroyed;
        enemy.OnEnemyExited += EnemyExited;
    }

    private void EnemyExited(Enemy enemy)
    {
        enemy.OnEnemyDestroyed -= EnemyDestroyed;
    }

    private void EnemyDestroyed(Enemy enemy)
    {
        score++;
        enemy.OnEnemyDestroyed -= EnemyDestroyed;
        hud.SetScore(score);
    } 
    #endregion
}
