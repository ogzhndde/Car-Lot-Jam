using UnityEngine;

/// <summary>
/// Scriptable object that holds data of particles
/// </summary>

[CreateAssetMenu(fileName = "ParticleData", menuName = "SO/Particle Data", order = 1)]
public class ParticleSO : ScriptableObject
{
    public GameObject HappyParticle;
    public GameObject AngryParticle;
    public GameObject ConfettiParticle;
}
