using UnityEngine;

/// <summary>
/// SubClass of Player factory
/// </summary>

public abstract class PlayerProperties : IPlayer
{
    public TeamType teamType { get; }

    public PlayerType playerType { get; }

    public abstract void SpawnPlayer(PlayerType playerType, TeamType teamType, Vector3 spawnPosition, ref GameManager manager);

}
