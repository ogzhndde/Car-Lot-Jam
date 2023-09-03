using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHandler : SingletonManager<SelectionHandler>
{
    public GameObject SelectedPlayer;

    void Update()
    {
        Select();
    }

    //The action that will take place when clicked.
    private void Select()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;

                if (hitObject.GetComponent<PlayerController>() != null)
                {
                    ClickOnPlayer(hitObject);
                }

                else if (hitObject.GetComponent<CarController>() != null)
                {
                    ClickOnCar(hitObject);
                }

                else if (hitObject.GetComponent<GridController>() != null)
                {
                    ClickOnGrid(hitObject);
                }
            }
        }
    }

    private void ClickOnPlayer(GameObject clickedPlayer)
    {
        SelectedPlayer = clickedPlayer;
        EventManager.Broadcast(GameEvent.OnPlayerSelect, SelectedPlayer);
        EventManager.Broadcast(GameEvent.OnPlaySound, "SoundClick");

        if (TutorialManager.Instance)
            TutorialManager.Instance.TutorialNext();
    }

    private void ClickOnCar(GameObject selectedCar)
    {
        if (SelectedPlayer == null) return;

        EventManager.Broadcast(GameEvent.OnCarSelect, selectedCar, SelectedPlayer);
        EventManager.Broadcast(GameEvent.OnSelectionClear);

        if (TutorialManager.Instance)
            TutorialManager.Instance.TutorialNext();
    }

    private void ClickOnGrid(GameObject selectedGrid)
    {
        if (PlayerPrefs.GetInt("TutorialDone") == 0) return;
        if (SelectedPlayer == null) return;

        GridController gridController = selectedGrid.GetComponent<GridController>();
        if (!gridController._isGridAvailable) return;

        EventManager.Broadcast(GameEvent.OnGridSelect, selectedGrid, SelectedPlayer);
    }

    private void ClearSelection()
    {
        SelectedPlayer = null;
    }


    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnSelectionClear, OnSelectionClear);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnSelectionClear, OnSelectionClear);
    }

    private void OnSelectionClear()
    {
        ClearSelection();
    }
}
