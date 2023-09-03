
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All interfaces that are using in the game
/// </summary>

public interface IPlayer
{
    PlayerType playerType { get; }
    TeamType teamType { get; }
}

public interface ICar
{
    CarType carType { get; }
    TeamType teamType { get; }
}

public interface IObstacle
{
    ObstacleType obstacleType { get; }
}

public interface IProp
{
    PropType propType { get; }
}


public interface IClickable
{

}


