using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable object that holds data of players
/// </summary>

[CreateAssetMenu(fileName = "PlayerData", menuName = "SO/Player Data", order = 1)]
public class PlayerSO : ScriptableObject
{
    public GameObject NormalPlayer;
}
