using System;
using UnityEngine;
using UnityEngine.UI;

public class Crystal : MonoBehaviour
{
    [SerializeField] private Material material;

    public Interaction _interactionSystem;
    public RectTransform rectTransform;

    public Types Type { get; set; }
    public Vector2 Position { get => transform.localPosition; }

    public event Action OnComplete;
    private event Action<Direction> _interactAction;


    private bool _isDissolving = false;
    private float dissolveProgress = 0f;
    private static readonly int Value = Shader.PropertyToID("_Value");
    private Image image = null;
    private Material _material = null;

    public bool MustDestroy { get; set; } = false;
    private void Start()
    {
        _interactionSystem = gameObject.AddComponent<Interaction>();
        rectTransform = GetComponent<RectTransform>();

        image = gameObject.GetComponent<Image>();
        _material = Instantiate(material);

        image.material = _material;

        _interactionSystem.SwapAction += OnInteract;
        OnComplete += DestroyAction;
    }
    private void FixedUpdate()
    {
        CorrosionDissolve();
    }
    public void OnInteract(Direction direction)
    {
        if (direction != Direction.None)
            _interactAction?.Invoke(direction);
    }

    public void ChangePositionInBoard(Cell newCell)
    {
        if (gameObject != null && newCell != null)
            DOTweenCrystalAnimService.AnimatePosition(gameObject, newCell.transform, 0.5f);

        transform.SetParent(newCell.transform);

        UnsubscribeAll();
        SubscribeIntercationAction(newCell.TrySwap);
    }
    public void UnsubscribeAll()
    {
        _interactAction = null;
    }
    public void SubscribeIntercationAction(Action<Direction> action)
    {
        _interactAction += action;
    }

    public void DestroyCrystal()
    {
        _isDissolving = true;
    }
    public void CorrosionDissolve()
    {
        if (_isDissolving != false)
        {
            dissolveProgress = Mathf.Clamp01(dissolveProgress + Time.fixedDeltaTime);
            _material.SetFloat(Value, dissolveProgress);

            if (dissolveProgress == 1)
            {
                OnComplete?.Invoke();
            }
        }
    }

    public void DestroyAction()
    {
        ScoreManager.InvokeOnCrystalDestroy(Type);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _interactionSystem.SwapAction -= OnInteract;
        OnComplete -= DestroyAction;
    }
}