using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object that holds data of obstacle
/// </summary>

[CreateAssetMenu(fileName = "ObstacleData", menuName = "SO/Obstacle Data", order = 1)]
public class ObstacleSO : ScriptableObject
{
    public GameObject Barrier;
    public GameObject ExitBarrier;
    public GameObject Cone;
}
