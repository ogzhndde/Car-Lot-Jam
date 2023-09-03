using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EPOOutline;
using ParticleFactoryStatic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CarController : MonoBehaviour, ICar
{
    [Inject]
    GameManager manager;

    public bool _isCarReadyToLeave = false;

    [Header("Definitions")]
    public GameObject RightDoorGrid, LeftDoorGrid;
    public GameObject RightGridPoint, LeftGridPoint;
    public GameObject RightGetInPoint, LeftGetInPoint;
    public LayerMask GridLayermask;

    #region Main Variables
    [SerializeField] private CarType _carType;
    [SerializeField] private List<Renderer> CarRenderers;
    [SerializeField] private GameObject LeftDoor;
    [SerializeField] private GameObject RightDoor;
    public TeamType _teamType;
    public Animator anim;

    #endregion

    #region Interface Variables
    public CarType carType => _carType;
    public TeamType teamType => _teamType;
    #endregion

    public void SetCarProperties(CarType TypeCar, TeamType TypeTeam)
    {
        manager = GameManager.Instance;

        _carType = TypeCar;
        _teamType = TypeTeam;
        SetVisualProperties();
    }

    public void SetVisualProperties()
    {
        Material carMat = _teamType switch
        {
            TeamType.Purple => manager.SO.MaterialData.MAT_Purple,
            TeamType.Black => manager.SO.MaterialData.MAT_Black,
            TeamType.Blue => manager.SO.MaterialData.MAT_Blue,
            TeamType.Green => manager.SO.MaterialData.MAT_Green,
            TeamType.Orange => manager.SO.MaterialData.MAT_Orange,
            TeamType.Pink => manager.SO.MaterialData.MAT_Pink,
            TeamType.Red => manager.SO.MaterialData.MAT_Red,
            TeamType.Yellow => manager.SO.MaterialData.MAT_Yellow,
            _ => manager.SO.MaterialData.MAT_Black,
        };

        foreach (var item in CarRenderers)
        {
            item.material = carMat;
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        SetVisualProperties(); //Even you dont create car from factory, set visual properties

    }

    void FixedUpdate()
    {
        SetDoorGrids();
    }
    void SetDoorGrids()
    {
        float radius = 0.5f;

        Collider[] rightColliders = Physics.OverlapSphere(RightGridPoint.transform.position, radius, GridLayermask);
        Collider[] leftColliders = Physics.OverlapSphere(LeftGridPoint.transform.position, radius, GridLayermask);

        foreach (Collider collider in rightColliders)
        {
            if (collider.gameObject.GetComponent<GridController>() != null)
                RightDoorGrid = collider.gameObject;
        }

        foreach (Collider collider in leftColliders)
        {
            if (collider.gameObject.GetComponent<GridController>() != null)
                LeftDoorGrid = collider.gameObject;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(RightGridPoint.transform.position, 0.5f);
        Gizmos.DrawWireSphere(LeftGridPoint.transform.position, 0.5f);
    }

    void CheckOutline(bool active, Color color)
    {
        Outlinable outlinable = GetComponentInChildren<Outlinable>();

        outlinable.enabled = true;
        outlinable.OutlineParameters.color = color;

        DOTween.Sequence()
            .AppendInterval(0.5f)
            .OnComplete(() =>
            {
                outlinable.enabled = false;
            });
    }

    GameObject CheckCarAvailablity()
    {
        GridController leftGrid = null, rightGrid = null;
        bool leftGridAvailable = false, rightGridAvailable = false;

        if (LeftDoorGrid)
        {
            leftGrid = LeftDoorGrid.GetComponent<GridController>();
            leftGridAvailable = manager.IsThereAvailablePath(SelectionHandler.Instance.SelectedPlayer.transform, LeftDoorGrid.transform) && leftGrid._isGridAvailable;
        }
        if (RightDoorGrid)
        {
            rightGrid = RightDoorGrid.GetComponent<GridController>();
            rightGridAvailable = manager.IsThereAvailablePath(SelectionHandler.Instance.SelectedPlayer.transform, RightDoorGrid.transform) && rightGrid._isGridAvailable;
        }

        if (leftGridAvailable) return LeftDoorGrid;
        if (rightGridAvailable) return RightDoorGrid;
        return null;
    }

    public void IsCarReadyToLeave(bool state)
    {
        StartCoroutine(isCarReadyToLeave(state));
    }
    IEnumerator isCarReadyToLeave(bool state)
    {
        yield return new WaitForSeconds(2f);
        _isCarReadyToLeave = state;
    }

    //########################################    EVENTS    ###################################################################

    private void OnEnable()
    {
        EventManager.AddHandler(GameEvent.OnCarSelect, OnCarSelect);
    }

    private void OnDisable()
    {
        EventManager.RemoveHandler(GameEvent.OnCarSelect, OnCarSelect);
    }

    private void OnCarSelect(object car, object player)
    {
        GameObject selectedCar = (GameObject)car;
        GameObject selectedPlayer = (GameObject)player;
        PlayerController playerController = selectedPlayer.GetComponent<PlayerController>();

        if (selectedCar == gameObject)
        {
            if (playerController._teamType == _teamType)
            {
                GameObject targetGrid = CheckCarAvailablity();

                if (targetGrid == null)
                {
                    CheckOutline(false, Color.red);
                    EventManager.Broadcast(GameEvent.OnGridAvailableCheck, LeftDoorGrid ? LeftDoorGrid : null, false);
                    EventManager.Broadcast(GameEvent.OnGridAvailableCheck, RightDoorGrid ? RightDoorGrid : null, false);
                    EventManager.Broadcast(GameEvent.OnPlaySound, "SoundWrong");
                    ParticleFactory.SpawnParticle(ParticleType.Angry, selectedPlayer.transform.position, playerController.ParticlePoint);
                }
                else
                {
                    CheckOutline(true, Color.green);
                    GameObject leftOrRightDoor = null;
                    leftOrRightDoor = targetGrid == LeftDoorGrid ? LeftDoor : RightDoor;

                    EventManager.Broadcast(GameEvent.OnPlayerCarMatched, targetGrid, selectedPlayer, selectedCar);
                    EventManager.Broadcast(GameEvent.OnPlaySound, "SoundCorrect");
                    ParticleFactory.SpawnParticle(ParticleType.Happy, selectedPlayer.transform.position, playerController.ParticlePoint);
                }
            }
            else
            {
                CheckOutline(true, Color.red);
                EventManager.Broadcast(GameEvent.OnSelectionClear);
                    EventManager.Broadcast(GameEvent.OnPlaySound, "SoundWrong");

                ParticleFactory.SpawnParticle(ParticleType.Angry, selectedPlayer.transform.position, playerController.ParticlePoint);
            }
        }
    }
}
