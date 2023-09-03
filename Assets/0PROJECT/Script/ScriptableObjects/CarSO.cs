using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object that holds data of cars
/// </summary>

[CreateAssetMenu(fileName = "CarData", menuName = "SO/Car Data", order = 1)]
public class CarSO : ScriptableObject
{
    public GameObject Sedan;
    public GameObject Limousine;
}
