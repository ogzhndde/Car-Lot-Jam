using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadManager : MonoBehaviour
{
    public GameData data;

    void Start()
    {
#if !UNITY_EDITOR
        SaveManager.LoadData(data);
#endif
        LoadScene();
    }

    void LoadScene()
    {
        SceneManager.LoadScene(data.LevelCount);
    }
}
