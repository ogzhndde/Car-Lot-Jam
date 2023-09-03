using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// The class that arranges the exit path for the cars after generating the scene. 
/// It assigns two paths named Zpath and Lpath. 
/// Don't forget to assign these paths before playing the level!
/// </summary>

public class RoadController : SingletonManager<RoadController>
{
    [SerializeField] private LevelGenerator levelGenerator;
    public GameObject[] AllRoads;

    [SerializeField] private int GridHorizontal => levelGenerator.GridHorizontal;
    [SerializeField] private int GridVertical => levelGenerator.GridVertical;
    private readonly int GridOffset = 2;

    public List<Transform> ZPath;
    public List<Transform> LPath;

    [Button]
    void FindAllRoads()
    {
        AllRoads = GameObject.FindGameObjectsWithTag("Road");
    }

    [Button]
    void SetZPath()
    {
        int allRoadsCount = AllRoads.Length;

        List<Transform> FirstCheckList = new List<Transform>();
        for (int i = 0; i < allRoadsCount; i++)
        {
            if (AllRoads[i].transform.position.x == (GridHorizontal + 1) * GridOffset)
            {
                FirstCheckList.Add(AllRoads[i].transform);
            }
        }
        FirstCheckList = FirstCheckList.OrderBy(transform => transform.position.z).ToList();

        List<Transform> SecondCheckList = new List<Transform>();
        for (int i = 0; i < allRoadsCount; i++)
        {
            if (AllRoads[i].transform.position.z == (GridVertical + 1) * GridOffset)
            {
                SecondCheckList.Add(AllRoads[i].transform);
            }
        }
        SecondCheckList = SecondCheckList.OrderByDescending(transform => transform.position.x).ToList();

        List<Transform> ThirdCheckList = new List<Transform>();
        for (int i = 0; i < allRoadsCount; i++)
        {
            if (AllRoads[i].transform.position.z >= (GridVertical + 1) * GridOffset && AllRoads[i].transform.position.x == 0)
            {
                ThirdCheckList.Add(AllRoads[i].transform);
            }
        }
        ThirdCheckList = ThirdCheckList.OrderBy(transform => transform.position.z).ToList();

        // Add all 3 check list to ZPath list without dublicate
        HashSet<Transform> ZPathHashSet = new HashSet<Transform>(FirstCheckList);
        ZPathHashSet.UnionWith(SecondCheckList);
        ZPathHashSet.UnionWith(ThirdCheckList);

        ZPath = ZPathHashSet.ToList();
    }

    [Button]
    void SetLPath()
    {
        int allRoadsCount = AllRoads.Length;

        List<Transform> FirstCheckList = new List<Transform>();
        for (int i = 0; i < allRoadsCount; i++)
        {
            if (AllRoads[i].transform.position.z == 0)
            {
                FirstCheckList.Add(AllRoads[i].transform);
            }
        }
        FirstCheckList = FirstCheckList.OrderByDescending(transform => transform.position.x).ToList();


        List<Transform> SecondCheckList = new List<Transform>();
        for (int i = 0; i < allRoadsCount; i++)
        {
            if (AllRoads[i].transform.position.x == 0)
            {
                SecondCheckList.Add(AllRoads[i].transform);
            }
        }
        SecondCheckList = SecondCheckList.OrderBy(transform => transform.position.z).ToList();

        // Add 2 check list to ZPath list without dublicate
        HashSet<Transform> LPathHashSet = new HashSet<Transform>(FirstCheckList);
        LPathHashSet.UnionWith(SecondCheckList);

        LPath = LPathHashSet.ToList();
    }

    [Button]
    void ClearLists()
    {
        AllRoads = null;
        ZPath.Clear();
        LPath.Clear();
    }
}
