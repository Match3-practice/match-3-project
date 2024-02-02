using System;
using UnityEngine;

public sealed class CrystalShader : MonoBehaviour
{
    [SerializeField] private float dissolveProgress = 1f;
    [SerializeField] private Material _materials;

    private bool _isDissolving = false;

}
