using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Pathfinding;

public class GameManager : SingletonManager<GameManager>
{
    public ScriptableObjects SO;
    [SerializeField] private List<Vector3> tempNodes;

    public int LevelCarCounter = 0;

    [Serializable]
    public struct ScriptableObjects
    {
        public GameData data;
        public CarSO CarData;
        public PlayerSO PlayerData;
        public ObstacleSO ObstacleData;
        public PropSO PropData;
        public ParticleSO ParticleData;
        public MaterialSO MaterialData;
    }


    //Check is there any available path for selected player and return a bool value
    public bool IsThereAvailablePath(Transform player, Transform target)
    {
        // Find the closest path
        ABPath path = ABPath.Construct(player.position, target.position, null);
        AstarPath.StartPath(path);
        path.BlockUntilCalculated();

        for (int i = 0; i < path.vectorPath.Count; i++)
        {
            tempNodes.Add(path.vectorPath[i]);
        }

        //Check is there any available path
        if (Vector3.Distance(tempNodes[^1], target.position) > 0.5f)
        {
            return false;
        }
        return true;
    }


    //When a car touch the exit barrier
    public void LevelEndCheck()
    {
        LevelCarCounter--;
        SO.data.TotalMoney += 50;
        
        if (LevelCarCounter == 0)
        {
            EventManager.Broadcast(GameEvent.OnFinish);
        }
    }

}
