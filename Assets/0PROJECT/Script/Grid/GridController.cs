using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class GridController : MonoBehaviour
{
    public bool _isGridAvailable;
    public Collider[] collisions;
    private MeshRenderer renderer;

    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    void FixedUpdate()
    {
        CheckGridAvailability();
    }
    
    //Check grid has an object above it or not
    void CheckGridAvailability()
    {
        collisions = Physics.OverlapSphere(transform.position, 0.5f);

        _isGridAvailable = collisions.Length > 3 ? false : true; //Except A* collider, ground collider and own collider
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

    //Grid color control
    void ColorChange(Color targetColor)
    {
        Material material = renderer.materials[0];
        Color defaultColor = material.color;
        material.DOColor(targetColor, 0.25f).OnComplete(() =>
        {
            material.DOColor(defaultColor, 0.25f);
        });
    }



    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnGridAvailableCheck, OnGridAvailableCheck);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnGridAvailableCheck, OnGridAvailableCheck);
    }


    private void OnGridAvailableCheck(object grid, object isAvailable)
    {
        GameObject selectedGrid = (GameObject)grid;
        bool _isAvailable = (bool)isAvailable;

        //If selected grid is this gameobject, make process
        if (selectedGrid == gameObject)
        {
            ColorChange(_isAvailable ? Color.green : Color.red);
        }
    }
}
