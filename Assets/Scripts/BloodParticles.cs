using System;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticles : MonoBehaviour
{
    private ParticleSystem _ps;
    [SerializeField] private GameObject _particleFloor;
    private List<ParticleCollisionEvent> _collisionEvents;
    public int ParticleAmount;

    private void Awake()
    {
        _ps = gameObject.GetComponent<ParticleSystem>();        // Get ParticleSystem reference
        _collisionEvents = new List<ParticleCollisionEvent>();   // List of each collision event
        var emission = _ps.emission;                                // Make emission a variable
        emission.burstCount = ParticleAmount;                      // Change burst count
    }
    
    [Obsolete("Obsolete")]
    private void OnParticleCollision(GameObject other)
    {
        // Initialize collision events
        int numCollisionEvents = _ps.GetCollisionEvents(other, _collisionEvents);

        int i = 0;
        while (i < numCollisionEvents)
        {
                // Collision position
                Vector3 pos = _collisionEvents[i].intersection;
                // Spawn new collision that player can walk on
                var rotation = other.transform.rotation;
                GameObject particleFloor = Instantiate(_particleFloor, pos,
                    Quaternion.EulerAngles(rotation.x, rotation.y, rotation.z * 2f));
                particleFloor.GetComponent<BoxCollider2D>().size = new Vector2(.2f, .2f);
                i++;
        }
    }

    public void SetParticleAmount(int amount)
    {
        var emission = _ps.emission;
        emission.SetBursts(new[]{ new ParticleSystem.Burst(0f, amount) });           
    }

}