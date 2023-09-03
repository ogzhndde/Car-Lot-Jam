using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object that holds data of props
/// </summary>

[CreateAssetMenu(fileName = "PropsData", menuName = "SO/Props Data", order = 1)]
public class PropSO : ScriptableObject
{
    public GameObject RoadLinear;
    public GameObject RoadT;
    public GameObject RoadCorner;
    public GameObject Grid;
}
