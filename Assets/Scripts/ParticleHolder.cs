using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHolder : MonoBehaviour
{
    [SerializeField] private GameObject _particlePrefab;
    [SerializeField] private int _poolingStartAmount;

    private List<GameObject> _particleList;
    private int _poolingNum;

    private CompositeCollider2D _cc;
    
    private void Awake()
    {
        _cc = transform.GetChild(0).GetComponent<CompositeCollider2D>();
        _particleList = new List<GameObject>();

        for (int i = 0; i < _poolingStartAmount; i++)
        {
            CreateParticle();
        }
    }

    public void SpawnParticle(Vector2 pos, Quaternion rot)
    {
        var rotation = Quaternion.EulerAngles(rot.x, rot.y, rot.z * 2f);
        if (_poolingNum < _poolingStartAmount)
        {
            _particleList[_poolingNum].transform.position = pos;
            _particleList[_poolingNum].transform.GetChild(0).rotation = rotation;
        }
        else
        {
            CreateParticle();
            _particleList[_poolingNum].transform.position = pos;
            _particleList[_poolingNum].transform.GetChild(0).rotation = rotation;
        }
        _poolingNum++;
    }
    
    public void SpawnParticleNoCollision(Vector2 pos, Quaternion rot)
    {
        var rotation = Quaternion.EulerAngles(rot.x, rot.y, rot.z * 2f);
        if (_poolingNum < _poolingStartAmount)
        {
            _particleList[_poolingNum].transform.position = pos;
            _particleList[_poolingNum].transform.GetComponentInChildren<BoxCollider2D>().enabled = false;
        }
        else
        {
            CreateParticle();
            _particleList[_poolingNum].transform.position = pos;
            _particleList[_poolingNum].transform.GetComponentInChildren<BoxCollider2D>().enabled = false;
        }
        _poolingNum++;
    }

    private void CreateParticle()
    {
        GameObject particle = Instantiate(_particlePrefab, new Vector3(-5000, 0, 0), Quaternion.identity, transform.GetChild(0));
           
        SpriteRenderer sr = particle.GetComponent<SpriteRenderer>();
        float size = Random.Range(0.2f, 0.4f);
        sr.size = new Vector2(size,size);
        sr.color = new Color32((byte)Random.Range(70f, 255f), 0, 0, 255);
           
        _particleList.Add(particle);
    }

    public void RegenerateCollider()
    {
        _cc.GenerateGeometry();
    }
}
