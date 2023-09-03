using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class UIManager : SingletonManager<UIManager>
{
    [Inject]
    GameManager manager;
    private GameData data;

    [Header("Definitions")]
    [SerializeField] private TextMeshProUGUI TMP_LevelText;
    [SerializeField] private TextMeshProUGUI TMP_TotalMoney;
    [SerializeField] private Button BTN_NextLevel;
    [SerializeField] private Button BTN_Restart;
    [SerializeField] private GameObject OBJ_MainPanel;
    [SerializeField] private GameObject OBJ_FinishPanel;

    private void Awake()
    {
        data = manager.SO.data;
    }

    private void Start()
    {
        InvokeRepeating(nameof(TextCheck), 0f, 0.25f);
    }

    void TextCheck()
    {
        TMP_LevelText.text = "Lv " + data.UILevelCount.ToString();
        TMP_TotalMoney.text = data.TotalMoney.ToString("0");
    }

    //######################################################### BUTTONS ##############################################################

    void ButtonNextLevel()
    {
        //If you reach the last level, loop levels
        data.LevelCount = data.levels.Count - 1 == data.LevelCount ? 1 : data.LevelCount + 1;
        data.UILevelCount++;

        SaveManager.SaveData(data);

        SceneManager.LoadScene(data.LevelCount);
    }

    void ButtonRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        BTN_NextLevel.onClick.AddListener(ButtonNextLevel);
        BTN_Restart.onClick.AddListener(ButtonRestart);

        EventManager.AddHandler(GameEvent.OnFinish, OnFinish);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnFinish, OnFinish);
    }


    private void OnFinish()
    {
        OBJ_MainPanel.SetActive(false);
        OBJ_FinishPanel.SetActive(true);
    }

}
