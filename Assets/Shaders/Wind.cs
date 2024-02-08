using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private Material _material;
    private float _amount;
    private float _opacity;
    private bool _blurActive;
    private bool _active;
    private static readonly int Amount = Shader.PropertyToID("_Amount");
    private static readonly int Opacity = Shader.PropertyToID("_Opacity");

    private void Start()
    {
        _amount = 0;
        _opacity = 1f;
    }

    private void Update()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _active = true;
            _blurActive = true;
        }
        if (Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            _active = false;
            _blurActive = false;
        }
        Blure();
        Opacity2();
    }

    private void Blure()
    {
    float speed = 10f;
        if (_blurActive)
    {
        _amount += speed * Time.deltaTime;
        
    }
    else
    {
        while (_amount > 1)
        {
            _amount -= speed * Time.deltaTime;
        }
    }

    _amount = Mathf.Clamp(_amount,0f,10f);
    _material.SetFloat(Amount,_amount);
    }
    private void Opacity2()
    {
        if (_active)
        {
            _opacity = Mathf.Clamp01(_opacity - Time.deltaTime);
            _material.SetFloat(Opacity, _opacity);
        }
        else
        {
            while (_opacity < 1)
            {
                _opacity = Mathf.Clamp01(_opacity + Time.deltaTime);
                _material.SetFloat(Opacity, _opacity);
                
            }
        }
    }
    
}
