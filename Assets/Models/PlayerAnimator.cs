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
        if (_movement == null)
            return;

        _animator.SetFloat(nameof(_movement.MoveVelocity), _movement.MoveVelocity);
        _animator.SetBool(nameof(_movement.IsFall), _movement.IsFall);
        _animator.SetFloat(nameof(_movement.FallTime), _movement.FallTime);
        _animator.SetBool(nameof(_movement.Cary), _movement.Cary);

        //float targetLayer = _animator.GetLayerWeight(1);
        //if (_movement.LayerWeight != targetLayer)
        //{
        //    DOTween.To(() => targetLayer, x => targetLayer = x, _movement.LayerWeight, Random.Range(0.25f, 0.375f)).OnUpdate(() =>
        //    {
        //        _animator.SetLayerWeight(1, targetLayer);
        //    });
        //}
    }
}
