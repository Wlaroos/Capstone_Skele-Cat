using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class BloodParticles : MonoBehaviour
{
    private ParticleSystem _ps;
    private ParticleHolder _particleHolder;
    
    // these lists are used to contain the particles which match
    // the trigger conditions each frame.
    private readonly List<ParticleSystem.Particle> _enter = new List<ParticleSystem.Particle>();
    private readonly List<ParticleSystem.Particle> _exit = new List<ParticleSystem.Particle>();
    
    [SerializeField] private GameObject _particleFloor;
    private List<ParticleCollisionEvent> _collisionEvents;
    public int ParticleAmount;

    [SerializeField] private ParticleSystem _killZonedParticles;

    private void Awake()
    {
        _ps = gameObject.GetComponent<ParticleSystem>(); // Get ParticleSystem reference
        _particleHolder = transform.parent.GetComponent<ParticleHolder>();
        _collisionEvents = new List<ParticleCollisionEvent>(); // List of each collision event
        var emission = _ps.emission; // Make emission a variable
        emission.burstCount = ParticleAmount; // Change burst count
    }
    
    private void Start()
    {
        // Find all kill zones and add them to the particle's list of colliders
        var test = Object.FindObjectsOfType<KillZone>();
        for (int i = 0; i < test.Length ; i++)
        {
            _ps.trigger.SetCollider(i, test[i].GetComponent<Collider2D>());
        }
    }
    
    // CREATES COLLISIONS ON PARTICLES THAT LAND ON "PARTICLE GROUND" LAYERS
    // Normal ground stops the particles, but doesn't create the new collisions
    [Obsolete("Obsolete")]
    private void OnParticleCollision(GameObject other)
    {
        // Initialize collision events
        int numCollisionEvents = _ps.GetCollisionEvents(other, _collisionEvents);

        int i = 0;
        while (i < numCollisionEvents)
        {
            // Test for the new collisions
            if (other.CompareTag("P_Collision"))
            {
                // Collision position
                Vector3 pos = _collisionEvents[i].intersection;
                // Spawn new collision that player can walk on
                var rot = other.transform.rotation;
                _particleHolder.SpawnParticle(pos, rot);
                //Instantiate(_particleFloor, pos, Quaternion.EulerAngles(rot.x, rot.y, rot.z * 2f), transform.parent.GetChild(0));
            }
            else
            {
                // Collision position
                Vector3 pos = _collisionEvents[i].intersection;
                // Spawn new collision that player can walk on
                var rot = other.transform.rotation;
                _particleHolder.SpawnParticleNoCollision(pos, rot);
                //Instantiate(_particleFloor, pos,Quaternion.EulerAngles(rot.x, rot.y, rot.z * 2f), transform.parent.GetChild(0)); 
            }
            i++;
        }
    }
    
    // DESTROYS PARTICLES -- ONLY FOR KILL ZONES
    void OnParticleTrigger()
    {
        // Get the particles which matched the trigger conditions this frame
        int numEnter = _ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, _enter);
        //int numExit = _ps.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, _exit);

        // Iterate through the particles which entered the trigger and destroy them
        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = _enter[i];
            p.remainingLifetime = 0; // Destroy
            Instantiate(_killZonedParticles, p.position, Quaternion.identity); // Spawn KillZoned Particle
            _enter[i] = p;
        }

        // Iterate through the particles which exited the trigger and make them green
        /*
        for (int i = 0; i < numExit; i++)
        {
            ParticleSystem.Particle p = _exit[i];
            p.startColor = new Color32(0, 255, 0, 255);
            _exit[i] = p;
        }
        */

        // Re-assign the modified particles back into the particle system
        _ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, _enter);
        //_ps.SetTriggerParticles(ParticleSystemTriggerEventType.Exit, _exit);
    }

    public void SetParticleAmount(int amount)
    {
        var emission = _ps.emission;
        emission.SetBursts(new[]{ new ParticleSystem.Burst(0f, amount) });           
    }
    
    public void SetShape(int arc, Vector3 rot)
    {
        var shape = _ps.shape;
        shape.arc = arc;
        shape.rotation = rot;
    }

}