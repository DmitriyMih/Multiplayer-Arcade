using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private PlayerMovement _movement;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (_movement != null)
        {
            _animator.SetFloat("Movement", _movement.MoveVelocity);

            float targetLayer = _animator.GetLayerWeight(1);
            if (_movement.LayerWeight != targetLayer)
            {
                DOTween.To(() => targetLayer, x => targetLayer = x, _movement.LayerWeight, Random.Range(0.25f, 0.375f)).OnUpdate(() =>
                {
                    _animator.SetLayerWeight(1, targetLayer);
                });
            }
        }
    }
}
