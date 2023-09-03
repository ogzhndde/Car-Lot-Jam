using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public PlayerState playerState;
    private Animator anim;

    bool _hasEnteredcar = false;


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        SetAnimation();
    }

    void SetAnimation()
    {
        if (_hasEnteredcar) return;

        bool _isWalking = false;

        //Check animation by state enum
        switch (playerState)
        {
            case PlayerState.Idle:
                _isWalking = false;
                break;

            case PlayerState.Walk:
                _isWalking = true;
                break;

            case PlayerState.EnteringCar:
                _hasEnteredcar = true;
                break;
        }

        anim.SetBool("_isWalking", _isWalking);
    }

    public void EnterCar(string LeftOrRight)
    {
        _hasEnteredcar = true;
        anim.SetTrigger("_isEnteringCar");

        float EnterState = LeftOrRight == "LeftDoor" ? 1 : 2;
        anim.SetFloat("EnterState", EnterState);
    }

    void Waving()
    {
        anim.SetTrigger("_isWaving");

    }

    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnPlayerSelect, OnPlayerSelect);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnPlayerSelect, OnPlayerSelect);
    }

    private void OnPlayerSelect(object value)
    {
        GameObject selectedPlayer = (GameObject)value;

        if (selectedPlayer == gameObject)
        {
            Waving();
        }
    }

}
