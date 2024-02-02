using System;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    private event Action<Direction> _interactAction;
    public Interaction _interactionSystem;
    public Vector2 Position { get => transform.localPosition; }
    public Types Type { get; set; }
    public RectTransform rectTransform;

    public bool MustDestroy { get; set; } = false;
    private void Start()
    {
        _interactionSystem = gameObject.AddComponent<Interaction>();        
        _interactionSystem.SwapAction += OnInteract;
        rectTransform = GetComponent<RectTransform>();
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

    public void Destroy()
    {
        Destroy(gameObject);
    }
}