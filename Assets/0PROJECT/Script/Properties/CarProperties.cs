using UnityEngine;

/// <summary>
/// SubClass of Car factory
/// </summary>

public abstract class CarProperties : ICar
{
    public CarType carType { get; }
    public TeamType teamType { get; }

    public abstract void SpawnCar(CarType carType, TeamType teamType, Vector3 spawnPosition, Quaternion spawnRotation, ref GameManager manager);

}
