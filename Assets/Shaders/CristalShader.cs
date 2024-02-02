using UnityEngine;

public sealed class CristalShader : MonoBehaviour
{
    private float _dissolveTime;
    private bool _isDissolving;
    [SerializeField]private Material _materials;
    private static readonly int Value = Shader.PropertyToID("_Value");

    private void Update()
    {
        if (_isDissolving)
        {
            _dissolveTime = Mathf.Clamp01(_dissolveTime + Time.deltaTime);
            _materials.SetFloat(Value,_dissolveTime);
        }
        else
        {
            _dissolveTime = Mathf.Clamp01(_dissolveTime - Time.deltaTime);
            _materials.SetFloat(Value, _dissolveTime);
        }

        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _isDissolving = true;
        }

        if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            _isDissolving = false;
        }
    }
}
