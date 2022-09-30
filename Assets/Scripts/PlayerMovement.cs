using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(1, 15f)] private float _speed = 5f;

    private float _speedWithTurret = 2f;
    private float _speedWithBoxCoef = 0.25f;

    [SerializeField, Range(1, 20)] private float _rotationPerFrame = 10f;
  
    [SerializeField] private Joystick _joystick;
    private CharacterController _cc;

    public float MoveVelocity => _cc.velocity.sqrMagnitude;
    public float LayerWeight;
    public bool IsMove { get; private set; }

    public float _distance = 0;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
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
        float speed =  Player._player.Inventory.HasBox? _speedWithTurret : _speed;
        
        _cc.SimpleMove(moveDir * speed);

        IsMove = moveDir.magnitude > 0 && _cc.isGrounded;
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
        if (_cc.isGrounded)
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
