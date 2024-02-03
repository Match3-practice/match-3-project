using System;
using UnityEngine;
using UnityEngine.UI;

public class Crystal : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private float crystalSpeed = .5f;

    public float dissloveValue = 1.0f;   
    public Interaction _interactionSystem;
    public RectTransform rectTransform;


    public Types Type { get; set; }
    public Vector2 Position { get => transform.localPosition; }

    public event Action OnComplete;
    private event Action<Direction> _interactAction;


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
    public void OnInteract(Direction direction)
    {
        if (direction != Direction.None)
            _interactAction?.Invoke(direction);
    }

    public void ChangePositionInBoard(Cell newCell)
    {
        if (gameObject != null && newCell != null)
            DOTweenCrystalAnimService.AnimatePosition(gameObject, newCell.transform, crystalSpeed);

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




    public void SetMaterialValue(float progress)
    {
        _material.SetFloat(Value, progress);
    }

    public void DestroyAction()
    {
        DOTweenCrystalAnimService.AnimateDestroy(this, CrystalDestroy);
        ScoreManager.InvokeOnCrystalDestroy(Type);


    }

    private void CrystalDestroy()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _interactionSystem.SwapAction -= OnInteract;
        OnComplete -= DestroyAction;
    }

    public Material GetMaterialInstance()
    {
        return _material;
    }
}