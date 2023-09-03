using System;
using System.Collections;
using System.Collections.Generic;
using EPOOutline;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayerController : MonoBehaviour, IPlayer
{
    [Inject]
    GameManager manager;

    public TeamType _teamType;
    [SerializeField] private PlayerType _playerType;
    public Transform ParticlePoint;


    #region Interface Variables
    public TeamType teamType => _teamType;
    public PlayerType playerType => _playerType;
    #endregion

    //Set all player properties
    public void SetPlayerProperties(PlayerType TypePlayer, TeamType TypeTeam)
    {
        _teamType = TypeTeam;
        _playerType = TypePlayer;
        SetVisualProperties();
    }

    //Set player visual properties
    public void SetVisualProperties()
    {
        Color playerColor = teamType switch
        {
            TeamType.Purple => new Color(0.5f, 0.0f, 0.5f),
            TeamType.Black => Color.black,
            TeamType.Blue => Color.blue,
            TeamType.Green => Color.green,
            TeamType.Orange => new Color(1.0f, 0.647f, 0.0f),
            TeamType.Pink => new Color(1.0f, 0.753f, 0.796f),
            TeamType.Red => Color.red,
            TeamType.Yellow => Color.yellow,
            _ => Color.white
        };

        GetComponentInChildren<Renderer>().material.color = playerColor;
    }

    void Start()
    {
        SetVisualProperties(); //Even you dont create player from factory, set visual properties
    }

    void CheckOutline(bool active, Color color)
    {
        Outlinable outlinable = GetComponentInChildren<Outlinable>();

        outlinable.enabled = active;
        outlinable.OutlineParameters.color = color;
    }


    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        //All events that using on the player
        EventManager.AddHandler(GameEvent.OnPlayerSelect, OnPlayerSelect);
        EventManager.AddHandler(GameEvent.OnCarSelect, OnCarSelect);
        EventManager.AddHandler(GameEvent.OnGridSelect, OnGridSelect);
        EventManager.AddHandler(GameEvent.OnSelectionClear, OnSelectionClear);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlayerSelect, OnPlayerSelect);
        EventManager.RemoveHandler(GameEvent.OnCarSelect, OnCarSelect);
        EventManager.RemoveHandler(GameEvent.OnGridSelect, OnGridSelect);
        EventManager.RemoveHandler(GameEvent.OnSelectionClear, OnSelectionClear);
    }

    private void OnPlayerSelect(object value)
    {
        GameObject selectedPlayer = (GameObject)value;

        if (selectedPlayer == gameObject)
        {
            CheckOutline(true, Color.green);
        }
        else
        {
            CheckOutline(false, Color.green);
        }
    }

    private void OnCarSelect(object car, object player)
    {
        CheckOutline(false, Color.green);
    }

    private void OnGridSelect(object grid, object player)
    {
        CheckOutline(false, Color.green);
    }

    private void OnSelectionClear()
    {
        CheckOutline(false, Color.green);
    }
}

