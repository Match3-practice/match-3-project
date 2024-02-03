using System;
using UnityEngine;
using UnityEngine.UI;

public class Crystal : MonoBehaviour
{
    public Interaction _interactionSystem;
    public RectTransform rectTransform;

    public Types Type { get; set; }
    public Vector2 Position { get => transform.localPosition; }

    public event Action OnComplete;
    private event Action<Direction> _interactAction;


    private float dissolveProgress;
    private static readonly int Value = Shader.PropertyToID("_Value");
    private Material _materials;


    public bool MustDestroy { get; set; } = false;
    private void Start()
    {
        _interactionSystem = gameObject.AddComponent<Interaction>();
        rectTransform = GetComponent<RectTransform>();
        _materials = GetComponent<Image>()?.material;

        _interactionSystem.SwapAction += OnInteract;
        OnComplete += Destroy;
    }

    public void OnInteract(Direction direction)
    {
        if (direction != Direction.None)
            _interactAction?.Invoke(direction);
    }

    public void ChangePositionInBoard(Cell newCell)
    {
        transform.SetParent(newCell.transform);
        if (gameObject != null && newCell != null)
            DOTweenCrystalAnimService.AnimatePosition(gameObject, newCell.transform, 0.5f);

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



    public void CorrosionDissolve()
    {
        if (_isDissolving != false)
        {
            dissolveProgress = Mathf.Clamp01(dissolveProgress - Time.fixedDeltaTime);
            _materials.SetFloat(Value, dissolveProgress);

            if (dissolveProgress == 1)
            {
                OnComplete?.Invoke();
            }
        }
    }

    public void Destroy()
    {
        
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _interactionSystem.SwapAction -= OnInteract;
        OnComplete -= Destroy;
    }
}