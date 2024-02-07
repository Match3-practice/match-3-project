
using System;
using UnityEngine;

public class FireCrystal : MonoBehaviour
{
    [SerializeField] private Material _material;
    private float _count;
    private bool _active;
    private static readonly int Edge = Shader.PropertyToID("_edge");

    private void Start()
    {
        _count = 56f;
    }

    private void Update()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _active = true;
        }
        if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            _active = false;
        }
        Fire();
    }

    private void Fire()
    {
        float speed = 10f;
        if (_active)
        {
            _count -= speed * Time.deltaTime;

        }
        else
        {
            _count = 56f;
            
        }

        _count = Mathf.Clamp(_count, -17f, 56f);
        _material.SetFloat(Edge, _count);
    }
}
