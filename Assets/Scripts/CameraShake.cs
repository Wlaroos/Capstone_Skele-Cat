using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    public static CameraShake Instance { get; set; }

    private Camera _cam;
    private float _camSize;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }

        _cam = GetComponent<Camera>();
        _camSize = _cam.orthographicSize;
    }
    
    public IEnumerator Shake (float duration, float magnitude)
    {
        float originalSize = _camSize;

        float elapsed = 0.0f;

        while (elapsed <= duration && Time.timeScale > 0)
        {
            float z = 0.0005f * magnitude * PlayerPrefs.GetFloat("Screen Shake");

            if (elapsed < duration / 5)
            {
                _cam.orthographicSize = _cam.orthographicSize - (z * 5);
            }
            if (elapsed >= duration / 5)
            {
                _cam.orthographicSize = _cam.orthographicSize + (z * 1.25f);
            }
            elapsed += Time.deltaTime;

            yield return null;

        }

        _cam.orthographicSize = originalSize;
    }

    public IEnumerator BigShake(float duration, float magnitude)
    {
        float originalSize = _camSize;

        float elapsed = 0.0f;

        while (elapsed < duration && Time.timeScale > 0)
        {
            float z = 0.0025f * magnitude * PlayerPrefs.GetFloat("Screen Shake");
            
            if (elapsed < duration / 7)
            {
                _cam.orthographicSize = _cam.orthographicSize + (z * 7);
            }
            if (elapsed >= duration / 7)
            {
                _cam.orthographicSize = _cam.orthographicSize - (z * 1.67f);
            }
            elapsed += Time.deltaTime;

            yield return null;

        }

        _cam.orthographicSize = originalSize;
    }
    
    public void CamShake()
    {
        StopAllCoroutines();
        StartCoroutine(Shake(.15f, 1));
    }

    public void CamShakeBig()
    {
        StopAllCoroutines();
        StartCoroutine(BigShake(.21f, 1));
    }

}
