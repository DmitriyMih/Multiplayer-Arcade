using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(1, 15f)] private float _speed = 5f;
    [SerializeField, Range(1, 20)] private float _rotationPerFrame = 10f;

    [SerializeField] private Joystick _joystick;
    [SerializeField] private PlayerAnimator _playerAnimator;

    private CharacterController characterController;
    public float MoveVelocity => characterController.velocity.sqrMagnitude;
    public float LayerWeight;

    public bool IsMove { get; private set; }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleGravity();
        Movement();
        Rotate();
    }

    private Vector3 moveDir;
    private void Movement()
    {
        moveDir.Set(_joystick.Horizontal, 0, _joystick.Vertical);
        //float speed = _speed;//_player.Inventory.HasTurret ? _speedWithTurret : _speed;
        //LayerWeight = _player.Inventory.HasTurret ? 1 : 0;

        //float animationSpeed = _player.Inventory.HasTurret ? _speedWithTurret * _speedWithTurretCoef : _speed * _speedCoef;
        //_player.PlayerAnimations.AnimationSpeed = animationSpeed;

        characterController.SimpleMove(moveDir * _speed);

        IsMove = moveDir.magnitude > 0 && characterController.isGrounded;
    }

    private void Rotate()
    {
        if (moveDir.magnitude > 0)
        {
            Quaternion currentRot = transform.rotation;
            Quaternion targetRot = Quaternion.LookRotation(moveDir);

            transform.rotation = Quaternion.Slerp(currentRot, targetRot, _rotationPerFrame * Time.deltaTime);
        }
    }
    private void HandleGravity()
    {
        if (characterController.isGrounded)
        {
            float groundedGravity = .05f;
            moveDir.y = groundedGravity;
        }
        else
        {
            moveDir.y = Physics.gravity.y;
        }
    }
}
