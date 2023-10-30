using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeTrigger : MonoBehaviour
{
    private PolygonCollider2D _col;
    private SpriteRenderer _sr;
    private SpriteRenderer _srOutline;
    private GameObject _particleHolder;
    private Transform _explodeLocation;
    private Transform _explodeAimAt;
    private Vector3 _rot;

    [SerializeField] private ParticleSystem _bloodParticle;
    [SerializeField] private int _bloodAmount = 50;
    [SerializeField] private int _uses = 1;
    [SerializeField] private bool _radial = true;
    [SerializeField] private int _arc = 45;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _srOutline = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _explodeLocation = transform.GetChild(1);
        _explodeAimAt = transform.GetChild(1).GetChild(0);
        
        _srOutline.size = _sr.size + new Vector2(0.25f,0.25f);
        _srOutline.enabled = true;
        
        _sr.color *= new Color32(255, 255, 255, 5);
        _srOutline.color *= new Color32(255, 0, 0, 5);
        
        _particleHolder = GameObject.Find("ParticleHolder");

        // Janky Code
        _explodeAimAt.LookAt(_explodeLocation);
        if (_explodeAimAt.position.x < 0)
        {
            _rot = new Vector3(0,180,_explodeAimAt.rotation.eulerAngles.x - (_arc / 2f));
        }
        else
        {
            _rot = new Vector3(0,0,_explodeAimAt.rotation.eulerAngles.x - (_arc / 2f));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (playerController != null && _uses > 0)
        {
            ParticleSystem bloodParticle = Instantiate(_bloodParticle, _explodeLocation.position, Quaternion.identity, _particleHolder.transform);
            bloodParticle.GetComponent<BloodParticles>().SetParticleAmount(_bloodAmount);
            if(!_radial) bloodParticle.GetComponent<BloodParticles>().SetShape(_arc, _rot);

            _uses -= 1;

            if (_uses <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!_radial)
        {
            Gizmos.color = Color.yellow;

            _explodeLocation = transform.GetChild(1);
            _explodeAimAt = transform.GetChild(1).GetChild(0);

            float rayRange = 2.0f;
            float halfFOV = _arc / 2.0f;
            Vector3 rot;
            rot = new Vector3(0, 0, _explodeAimAt.rotation.eulerAngles.x);

            _explodeAimAt.LookAt(_explodeLocation);

            float coneDirection = rot.z;

            Quaternion upRayRotation = Quaternion.AngleAxis(-halfFOV + coneDirection, Vector3.forward);
            Quaternion downRayRotation = Quaternion.AngleAxis(halfFOV + coneDirection, Vector3.forward);

            Vector3 upRayDirection = upRayRotation * _explodeLocation.right * rayRange;
            Vector3 downRayDirection = downRayRotation * _explodeLocation.right * rayRange;

            if (_explodeAimAt.position.x < 0)
            {
                upRayDirection.x *= -1;
                downRayDirection.x *= -1;
            }

            Gizmos.DrawRay(_explodeLocation.position, upRayDirection);
            Gizmos.DrawRay(_explodeLocation.position, downRayDirection);
            Gizmos.DrawLine(_explodeLocation.position + downRayDirection, _explodeLocation.position + upRayDirection);
        }
    }
}