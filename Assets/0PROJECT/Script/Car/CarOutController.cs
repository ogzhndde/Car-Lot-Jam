using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CarOutController : MonoBehaviour
{
    CarController carController;
    [SerializeField] private bool _isCarMoving = false;
    [SerializeField] private float moveSpeed;

    [SerializeField] private List<Transform> pathNodes = new List<Transform>();
    int currentNodeIndex = 0;

    [SerializeField] private Transform FrontRayPoint;
    [SerializeField] private Transform BackRayPoint;

    void Awake()
    {
        carController = GetComponent<CarController>();

        InvokeRepeating(nameof(CheckCarAvailability), 0.5f, 0.5f);
    }

    void CarMove()
    {
        _isCarMoving = true;

        if (currentNodeIndex < pathNodes.Count)
        {
            Vector3 nextNode = pathNodes[currentNodeIndex].position;

            //Rotate
            Quaternion targetRotation = Quaternion.LookRotation(nextNode - transform.position);
            if (currentNodeIndex != 0)
                transform.DORotateQuaternion(targetRotation, 0.3f);

            //Move
            transform.DOMove(nextNode, (nextNode - transform.position).magnitude / moveSpeed)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    currentNodeIndex++;
                    // Delay before moving to the next node
                    DOVirtual.DelayedCall(0f, CarMove);
                });
        }
        else
        {
            Destroy(gameObject);
        }
    }

    bool CheckCarAvailability()
    {
        if (!carController._isCarReadyToLeave) return false;
        if (_isCarMoving) return false;

        if (Physics.Raycast(FrontRayPoint.position, transform.forward, out RaycastHit frontHit, Mathf.Infinity))
        {
            if (frontHit.collider.gameObject.layer == LayerMask.NameToLayer("Road"))
            {
                pathNodes = FollowingPath(frontHit.transform);
                ReorganizePath(frontHit.transform);
                CarMove();
                return true;
            }
        }

        if (Physics.Raycast(BackRayPoint.position, -transform.forward, out RaycastHit backHit, Mathf.Infinity))
        {
            if (backHit.collider.gameObject.layer == LayerMask.NameToLayer("Road"))
            {
                pathNodes = FollowingPath(backHit.transform);
                ReorganizePath(backHit.transform);
                CarMove();
                return true;
            }
        }

        return false;
    }

    List<Transform> FollowingPath(Transform hittedTransform)
    {
        List<Transform> selectedPath = new List<Transform>();
        if (RoadController.Instance.ZPath.Contains(hittedTransform))
        {
            for (int i = 0; i < RoadController.Instance.ZPath.Count; i++)
            {
                selectedPath.Add(RoadController.Instance.ZPath[i]);
            }
            return selectedPath;
        }

        if (RoadController.Instance.LPath.Contains(hittedTransform))
        {
            for (int i = 0; i < RoadController.Instance.LPath.Count; i++)
            {
                selectedPath.Add(RoadController.Instance.LPath[i]);
            }
            return selectedPath;
        }

        return null;
    }

    void ReorganizePath(Transform hittedTransform)
    {
        for (int i = 0; i < pathNodes.Count; i++)
        {
            if (pathNodes[0] != hittedTransform)
                pathNodes.RemoveAt(0);
            else
                return;
        }
    }
}
