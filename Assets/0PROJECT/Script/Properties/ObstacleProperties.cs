using UnityEngine;

/// <summary>
/// SubClass of Obstacle factory
/// </summary>

public abstract class ObstacleProperties : IObstacle
{
    public ObstacleType obstacleType { get; }

    public abstract void SpawnObstacle(ObstacleType obstacleType, Vector3 spawnPosition, Quaternion spawnRotation, ref GameManager manager);

}
