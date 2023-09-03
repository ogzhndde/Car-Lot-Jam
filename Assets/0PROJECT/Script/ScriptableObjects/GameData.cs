using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [Header("Float & Int")]
    [SerializeField] private float totalMoney; // TOTAL MONEY
    public float TotalMoney
    {
        get { return totalMoney; }
        set
        {
            if (value < 0) totalMoney = 0;
            else totalMoney = value;
        }
    }

    public int LevelCount; //Normal Level counter
    public int UILevelCount; //UI Shown number in loop, always increasing 

    public List<Levels> levels = new List<Levels>();

    [Button]
    void ResetData()
    {
        TotalMoney = 0;
        LevelCount = 1;
        UILevelCount = 1;
    }

    [Button]
    void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    void ResetList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = default(T);
        }
    }

}



[System.Serializable]
public class Levels
{
    public string LevelType;
}