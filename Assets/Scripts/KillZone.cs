using System;
using Unity.VisualScripting;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private BoxCollider2D _bc;

    private void Awake()
    {
        _bc = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.GetComponent<PlayerController>();

        playerController?.Explode();
    }
}