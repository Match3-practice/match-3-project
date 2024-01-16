using System;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    private event Action<Direction> _interactAction;
    public ICrystalAnimationService animationService = null;
    public Interaction _interactionSystem;
    public Vector2 Position { get => transform.localPosition; }
    public Types Type { get; set; }

    public bool MustDestroy { get; set; } = false;
    private void Start()
    {
        _interactionSystem = gameObject.AddComponent<Interaction>();        
        _interactionSystem.SwapAction += OnInteract;

        animationService = new DOTweenCrystalAnimService();
    }

    public void OnInteract(Direction direction)
    {
        if (direction != Direction.None) 
            _interactAction?.Invoke(direction);
    }

    public void ChangePositionInBoard(MonoCell newCell)
    {
        gameObject.transform.SetParent(newCell.Position);

        if(gameObject != null && newCell != null)
        animationService.AnimatePosition(gameObject, newCell.Position.position, 5f);

        UnsubscribeAll();
        SubscribeIntercationAction(newCell.TrySwap);
    }
    //public void ChangePosition(Vector3 newPosition)
    //{
    //    transform.localPosition = newPosition;
    //}
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