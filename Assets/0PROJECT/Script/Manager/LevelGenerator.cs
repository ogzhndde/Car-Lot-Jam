using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using ObstacleFactoryStatic;
using PropFactoryStatic;
using CarFactoryStatic;
using PlayerFactoryStatic;

/// <summary>
/// The class that performs the level generate operations in the game. 
/// First, enter the number of grids you want, the data of players, obstacles and cars from the variables. 
/// Then, when you press the generate button, the level will be listed. 
/// After the level is lined up, assign the road paths from the RoadController object in the scene. 
/// Your level is ready. 
/// If you want to regenerate the level, you can clear and regenerate the objects in the scene.
/// </summary>

public class LevelGenerator : SingletonManager<LevelGenerator>
{
    [SerializeField] private GameManager manager;

    public int GridHorizontal;
    public int GridVertical;
    private readonly int GridOffset = 2;

    [Space(10)]
    [Header("Car Variables")]
    [SerializeField] private List<CarType> CarTypes;
    [SerializeField] private List<TeamType> CarTeam;
    [SerializeField] private List<Vector2> CarLocations;
    [SerializeField] private List<Direction> CarRotations;

    [Space(10)]
    [Header("Obstacle Variables")]
    [SerializeField] private List<ObstacleType> ObstacleTypes;
    [SerializeField] private List<Vector2> ObstacleLocations;
    [SerializeField] private List<Direction> ObstacleRotations;

    [Space(10)]
    [Header("Player Variables")]
    [SerializeField] private List<PlayerType> PlayerTypes;
    [SerializeField] private List<Vector2> PlayerLocation;
    [SerializeField] private List<TeamType> PlayerTeam;


    [Button]
    private void GenerateLevel()
    {
        PlacePlayers();
        PlaceCars();
        PlaceObstacles();
        PlaceGrids();
        PlaceRoads();
    }

    [Button]
    private void DestroyObjects()
    {
        #region FindObjects
        GameObject[] AllRoads = GameObject.FindGameObjectsWithTag("Road");
        GameObject[] AllGrids = GameObject.FindGameObjectsWithTag("Grid");
        GameObject[] AllPlayers = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] AllObstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject[] AllCars = GameObject.FindGameObjectsWithTag("Car");
        foreach (GameObject road in AllRoads)
        {
            DestroyImmediate(road);
        }

        foreach (GameObject grid in AllGrids)
        {
            DestroyImmediate(grid);
        }

        foreach (GameObject player in AllPlayers)
        {
            DestroyImmediate(player);
        }

        foreach (GameObject obstacle in AllObstacles)
        {
            DestroyImmediate(obstacle);
        }

        foreach (GameObject car in AllCars)
        {
            DestroyImmediate(car);
        }
        #endregion

    }

    [Button]
    private void ClearValues()
    {
        GridHorizontal = 0;
        GridVertical = 0;

        CarTypes.Clear();
        CarTeam.Clear();
        CarLocations.Clear();
        CarRotations.Clear();

        ObstacleTypes.Clear();
        ObstacleLocations.Clear();
        ObstacleRotations.Clear();

        PlayerTypes.Clear();
        PlayerLocation.Clear();
        PlayerTeam.Clear();
    }

    private void PlaceGrids()
    {
        for (int i = 1; i < GridHorizontal + 1; i++)
        {
            for (int j = 1; j < GridVertical + 1; j++)
            {
                PropFactory.SpawnProp(PropType.Grid, new Vector3(i * GridOffset, 0, j * GridOffset), Quaternion.identity, ref manager);
            }
        }
    }

    private void PlaceRoads()
    {
        // First Horizontal Placement  
        for (int i = 0; i <= GridHorizontal + 1; i++)
        {
            switch (i)
            {
                case 0:
                    PropFactory.SpawnProp(PropType.RoadCorner, new Vector3(i * GridOffset, 0, 0), Quaternion.Euler(new Vector3(0, -90, 0)), ref manager);
                    break;
                case int value when value == GridHorizontal + 1:
                    PropFactory.SpawnProp(PropType.RoadCorner, new Vector3(i * GridOffset, 0, 0), Quaternion.Euler(new Vector3(0, 180, 0)), ref manager);
                    break;
                default:
                    PropFactory.SpawnProp(PropType.RoadLinear, new Vector3(i * GridOffset, 0, 0), Quaternion.Euler(new Vector3(0, 90, 0)), ref manager);
                    break;
            }
        }

        // Second Horizontal Placement  
        for (int i = 0; i <= GridHorizontal + 1; i++)
        {
            switch (i)
            {
                case 0:
                    PropFactory.SpawnProp(PropType.RoadT, new Vector3(i * GridOffset, 0, (GridVertical + 1) * GridOffset), Quaternion.Euler(new Vector3(0, 0, 0)), ref manager);
                    break;
                case int value when value == GridHorizontal + 1:
                    PropFactory.SpawnProp(PropType.RoadCorner, new Vector3(i * GridOffset, 0, (GridVertical + 1) * GridOffset), Quaternion.Euler(new Vector3(0, 90, 0)), ref manager);
                    break;
                default:
                    PropFactory.SpawnProp(PropType.RoadLinear, new Vector3(i * GridOffset, 0, (GridVertical + 1) * GridOffset), Quaternion.Euler(new Vector3(0, 90, 0)), ref manager);
                    break;
            }
        }

        // First Vertical Placement  
        for (int i = 0; i <= GridVertical + 12; i++) //Place extra linear line to make exit path
        {
            switch (i)
            {
                case int value when value != 0 && value != GridVertical + 1:
                    PropFactory.SpawnProp(PropType.RoadLinear, new Vector3(0, 0, i * GridOffset), Quaternion.Euler(new Vector3(0, 0, 0)), ref manager);
                    break;
            }
        }

        // Second Vertical Placement  
        for (int i = 0; i <= GridVertical + 1; i++) //Place extra linear line to make exit path
        {
            switch (i)
            {
                case int value when value != 0 && value != GridVertical + 1:
                    PropFactory.SpawnProp(PropType.RoadLinear, new Vector3((GridHorizontal + 1) * GridOffset, 0, i * GridOffset), Quaternion.Euler(new Vector3(0, 0, 0)), ref manager);
                    break;
            }
        }
    }


    private void PlaceObstacles()
    {

        for (int i = 0; i < ObstacleTypes.Count; i++)
        {
            Quaternion rotation = ObstacleRotations[i] switch
            {
                Direction.Left => Quaternion.Euler(new Vector3(0, 180, 0)),
                Direction.Right => Quaternion.Euler(new Vector3(0, 0, 0)),
                Direction.Forward => Quaternion.Euler(new Vector3(0, -90, 0)),
                Direction.Back => Quaternion.Euler(new Vector3(0, 90, 0)),
                _ => Quaternion.Euler(new Vector3(0, 0, 0))
            };

            int HorizontaLGridIndex = (int)(ObstacleLocations[i].x * GridOffset);
            int VerticalGridIndex = (int)(ObstacleLocations[i].y * GridOffset);
            Vector3 location = new Vector3(HorizontaLGridIndex, 0, VerticalGridIndex);

            ObstacleFactory.SpawnObstacle(ObstacleTypes[i], location, rotation, ref manager);
        }

        ObstacleFactory.SpawnObstacle(ObstacleType.ExitBarrier, new Vector3(GridOffset, 0, (GridVertical + 3) * GridOffset), Quaternion.Euler(new Vector3(0, 180, 0)), ref manager);
    }

    private void PlaceCars()
    {
        for (int i = 0; i < CarTypes.Count; i++)
        {
            Quaternion rotation = CarRotations[i] switch
            {
                Direction.Left => Quaternion.Euler(new Vector3(0, -90, 0)),
                Direction.Right => Quaternion.Euler(new Vector3(0, 90, 0)),
                Direction.Forward => Quaternion.Euler(new Vector3(0, 0, 0)),
                Direction.Back => Quaternion.Euler(new Vector3(0, 180, 0)),
                _ => Quaternion.Euler(new Vector3(0, 0, 0))
            };

            int HorizontaLGridIndex = (int)(CarLocations[i].x * GridOffset);
            int VerticalGridIndex = (int)(CarLocations[i].y * GridOffset);
            Vector3 location = new Vector3(HorizontaLGridIndex, 0, VerticalGridIndex);

            CarFactory.SpawnCar(CarTypes[i], CarTeam[i], location, rotation, ref manager);
        }
    }
    private void PlacePlayers()
    {
        manager.LevelCarCounter = 0;
        for (int i = 0; i < PlayerTypes.Count; i++)
        {
            int HorizontaLGridIndex = (int)(PlayerLocation[i].x * GridOffset);
            int VerticalGridIndex = (int)(PlayerLocation[i].y * GridOffset);
            Vector3 location = new Vector3(HorizontaLGridIndex, 0, VerticalGridIndex);

            PlayerFactory.SpawnPlayer(PlayerTypes[i], PlayerTeam[i], location, ref manager);
        }
    }
}
