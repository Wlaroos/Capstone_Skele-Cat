    using System;
    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiExplodeZone : MonoBehaviour
{
    private PolygonCollider2D _col;
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private SpriteRenderer _srOutline;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _srOutline = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _srOutline.size = _sr.size + new Vector2(0.25f,0.25f);
        _srOutline.enabled = true;
        _sr.color *= new Color32(255, 255, 255, 5);
        _srOutline.color *= new Color32(255, 0, 0, 5);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        playerController?.SetCanPressExplode(false);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        
        playerController?.SetCanPressExplode(true);
    }
}
