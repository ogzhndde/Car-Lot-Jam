using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class that produces all obstacles that can be produced.
/// </summary>
namespace ObstacleFactoryStatic
{
    public static class ObstacleFactory
    {
        //It stores all obstacle types and production classes in a dictionary.
        private static Dictionary<ObstacleType, Func<ObstacleProperties>> obstacleFactories = new Dictionary<ObstacleType, Func<ObstacleProperties>>
        {
            { ObstacleType.Barrier, () => new Barrier() },
            { ObstacleType.ExitBarrier, () => new ExitBarrier() },
            { ObstacleType.Cone, () => new Cone()}
        };

        //The class in which obstacle are spawned.
        public static ObstacleProperties SpawnObstacle(ObstacleType obstacleType, Vector3 spawnPosition, Quaternion spawnRotation, ref GameManager manager)
        {
            if (obstacleFactories.TryGetValue(obstacleType, out var factory))
            {
                var obstacle = factory.Invoke();
                obstacle.SpawnObstacle(obstacleType, spawnPosition, spawnRotation, ref manager);
                return obstacle;
            }
            else
            {
                return null;
            }
        }
    }

    //All necessary data for obstacle is drawn from scriptable objects and sent to the factory for production.
    public class Barrier : ObstacleProperties
    {
        public override void SpawnObstacle(ObstacleType obstacleType, Vector3 spawnPosition, Quaternion spawnRotation, ref GameManager manager)
        {
            var spawnedObstacle = ObjectPoolManager.SpawnObjects(manager.SO.ObstacleData.Barrier, spawnPosition, spawnRotation, PoolType.Gameobject);
        }
    }

    public class ExitBarrier : ObstacleProperties
    {
        public override void SpawnObstacle(ObstacleType obstacleType, Vector3 spawnPosition, Quaternion spawnRotation, ref GameManager manager)
        {
            var spawnedObstacle = ObjectPoolManager.SpawnObjects(manager.SO.ObstacleData.ExitBarrier, spawnPosition, spawnRotation, PoolType.Gameobject);
        }
    }

    public class Cone : ObstacleProperties
    {
        public override void SpawnObstacle(ObstacleType obstacleType, Vector3 spawnPosition, Quaternion spawnRotation, ref GameManager manager)
        {
            var spawnedObstacle = ObjectPoolManager.SpawnObjects(manager.SO.ObstacleData.Cone, spawnPosition, spawnRotation, PoolType.Gameobject);
        }
    }

}
