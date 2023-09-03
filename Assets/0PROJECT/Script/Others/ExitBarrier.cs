using System.Collections;
using System.Collections.Generic;
using ParticleFactoryStatic;
using UnityEngine;
using Zenject;

public class ExitBarrier : MonoBehaviour
{
    [Inject]
    GameManager manager;

    Animator animator;
    [SerializeField] private Transform particlePoint;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Car":
                manager.LevelEndCheck();
                animator.SetTrigger("_barrierOpen");

                ParticleFactory.SpawnParticle(ParticleType.Confetti, particlePoint.position, particlePoint.transform);
                break;

        }
    }
}
