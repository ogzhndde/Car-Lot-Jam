using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using DG.Tweening;
using Zenject;

public class PlayerAI : MonoBehaviour
{
    [Inject]
    GameManager manager;

    [SerializeField] private Transform TargetDestination;
    [SerializeField] private bool _isPlayerMatchedWithCar = false;
    private GameObject TargetCar;


    [Header("Definitions")]
    private PlayerController controller;
    private PlayerAnimation playerAnimation;

    [SerializeField] private List<Vector3> pathNodes = new List<Vector3>();
    private int currentNodeIndex = 0;
    public float moveSpeed = 5f; // Movement speed
    public float rotationSpeed = 10f; // Rotation speed
    public float delayBetweenNodes = 0.5f; // Delay between moving to each node


    void Awake()
    {
        controller = GetComponent<PlayerController>();
        playerAnimation = GetComponent<PlayerAnimation>();
    }


    //Find Closest Path
    private void FindClosestPath(Transform target)
    {
        bool _isThereAvailablePath = manager.IsThereAvailablePath(transform, target);
        if (!_isThereAvailablePath)
        {
            EventManager.Broadcast(GameEvent.OnGridAvailableCheck, target.gameObject, false);
            return;
        }

        // Find the closest path
        ABPath path = ABPath.Construct(transform.position, target.position, null);
        AstarPath.StartPath(path);
        path.BlockUntilCalculated();

        // Store the positions of nodes 
        pathNodes.Clear();
        for (int i = 0; i < path.vectorPath.Count; i++)
        {
            pathNodes.Add(path.vectorPath[i]);
        }

        EventManager.Broadcast(GameEvent.OnGridAvailableCheck, target.gameObject, true);

        // Start moving along the path
        currentNodeIndex = 0;
        MoveToNextNodeInternal();
    }

    //Move the player with DoMove along the selected path
    private void MoveToNextNodeInternal()
    {
        if (currentNodeIndex < pathNodes.Count)
        {
            playerAnimation.playerState = PlayerState.Walk;

            Vector3 nextNode = pathNodes[currentNodeIndex];

            //Rotate
            Quaternion targetRotation = Quaternion.LookRotation(nextNode - transform.position);
            transform.DORotateQuaternion(targetRotation, rotationSpeed);

            //Move
            transform.DOMove(nextNode, (nextNode - transform.position).magnitude / moveSpeed)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    currentNodeIndex++;
                    // Delay before moving to the next node
                    DOVirtual.DelayedCall(delayBetweenNodes, MoveToNextNodeInternal);
                });
        }
        else
        {
            if (_isPlayerMatchedWithCar)
                GetInTheCar();
            else
                ResetMovementValues();
        }
    }

    void ResetMovementValues()
    {
        playerAnimation.playerState = PlayerState.Idle;
        currentNodeIndex = 0;
        pathNodes.Clear();
    }

    //Process of player getting in the car
    void GetInTheCar()
    {
        //Definitions
        CarController carController = TargetCar.GetComponent<CarController>();
        Collider collider = GetComponent<Collider>();
        string whichSideOfCar = string.Empty;
        Vector3 playerMovePos = Vector3.zero;

        transform.SetParent(TargetCar.transform);

        if (transform.localPosition.x > 0) //RightSide
        {
            whichSideOfCar = "RightDoor";
            playerMovePos = carController.RightGetInPoint.transform.position;
            carController.anim.SetTrigger("_rightDoor");
        }

        else if (transform.localPosition.x < 0) //LeftSide
        {
            whichSideOfCar = "LeftDoor";
            playerMovePos = carController.LeftGetInPoint.transform.position;
            carController.anim.SetTrigger("_leftDoor");
        }

        playerAnimation.playerState = PlayerState.EnteringCar;
        playerAnimation.EnterCar(whichSideOfCar);

        collider.isTrigger = true;

        //Move to door position
        transform.DOMove(playerMovePos, 0.25f).SetEase(Ease.Linear).OnComplete(() =>
        {
            transform.DOScale(transform.localScale * 0.7f, 0.5f);
            Destroy(gameObject, 2f);
            carController.IsCarReadyToLeave(true);
            EventManager.Broadcast(GameEvent.OnPlaySound, "SoundCarEngine");

        });

        //Rotate player 
        transform.DOLocalRotateQuaternion(Quaternion.Euler(0, whichSideOfCar == "LeftDoor" ? 90 : -90, 0), 0.25f);
    }


    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnGridSelect, OnGridSelect);
        EventManager.AddHandler(GameEvent.OnPlayerCarMatched, OnPlayerCarMatched);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnGridSelect, OnGridSelect);
        EventManager.RemoveHandler(GameEvent.OnPlayerCarMatched, OnPlayerCarMatched);
    }

    private void OnGridSelect(object grid, object player)
    {
        GameObject selectedGrid = (GameObject)grid;
        GameObject selectedPlayer = (GameObject)player;

        if (selectedPlayer == gameObject)
        {
            FindClosestPath(selectedGrid.transform);
        }

        EventManager.Broadcast(GameEvent.OnSelectionClear);
    }

    //The process when match the player and car correctly
    private void OnPlayerCarMatched(object grid, object player, object car)
    {
        GameObject targetGrid = (GameObject)grid;
        GameObject targetPlayer = (GameObject)player;
        TargetCar = (GameObject)car;

        if (targetPlayer == gameObject)
        {
            _isPlayerMatchedWithCar = true;
            FindClosestPath(targetGrid.transform);
        }
        EventManager.Broadcast(GameEvent.OnSelectionClear);
    }
}
