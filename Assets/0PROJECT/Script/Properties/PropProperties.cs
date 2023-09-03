using UnityEngine;

/// <summary>
/// SubClass of Prop factory
/// </summary>

public abstract class PropProperties : IProp
{
    public PropType propType { get; }

    public abstract void SpawnProp(PropType propType, Vector3 spawnPosition, Quaternion spawnRotation, ref GameManager manager);

}

