using System;
using UnityEngine;
using DG.Tweening;

namespace GUI.MainMenu
{
    public class AnimationTitle : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        private Vector3 _positionOriginal;
        private float _duration = 3f;
        private void Start()
        {
            _positionOriginal = transform.position;
            DOTween.Sequence()
                .Append(transform.DOMove(_target.position, _duration))
                .Join(transform.DOScale(3, _duration))
                .Append(transform.DOMove(_positionOriginal, _duration))
                .Join(transform.DOScale(2, _duration))
                .SetLoops(-1);

        }
    }
}
