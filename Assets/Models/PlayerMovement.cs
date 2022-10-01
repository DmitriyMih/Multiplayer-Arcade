using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(1, 15f)] private float _speed = 5f;
    [SerializeField, Range(1, 20)] private float _rotationPerFrame = 10f;

    [SerializeField] private Joystick _joystick;

    private CharacterController _characterController;

    [SerializeField] private float minFallTime = 0.5f;
    [SerializeField] private float fallCooldownTime = 1f;

    [Header("Fall Debug")]
    private float _normalHeight;
    [SerializeField] private float _fallHeight;

    private float _normalYCenter;
    [SerializeField] private float _fallYCenter;

    public float MoveVelocity => _characterController.velocity.sqrMagnitude;
    public float LayerWeight;

    public bool IsMove { get; private set; }
    public bool IsFall { get; private set; }
    public float FallTime { get; private set; }
    public float MovementCooldown { get; private set; }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _normalHeight = _characterController.height;
        _normalYCenter = _characterController.center.y;
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
        if (_joystick == null)
        {
            Debug.Log("Joystick Is Null");
            return;
        }

        if (MovementCooldown > 0)
        {
            _characterController.SimpleMove(Vector3.zero);
           MovementCooldown -= Time.deltaTime;
            return;
        }

        moveDir.Set(_joystick.Horizontal, 0, _joystick.Vertical);
        //float speed = _speed;//_player.Inventory.HasTurret ? _speedWithTurret : _speed;
        //LayerWeight = _player.Inventory.HasTurret ? 1 : 0;

        //float animationSpeed = _player.Inventory.HasTurret ? _speedWithTurret * _speedWithTurretCoef : _speed * _speedCoef;
        //_player.PlayerAnimations.AnimationSpeed = animationSpeed;

        _characterController.SimpleMove(moveDir * _speed);
        IsMove = moveDir.magnitude > 0 && _characterController.isGrounded;
    }

    private void Rotate()
    {
        if (moveDir.magnitude > 0)
        {
            Quaternion currentRot = transform.rotation;
            Quaternion targetRot = Quaternion.LookRotation(moveDir);

            transform.rotation = Quaternion.Slerp(currentRot, targetRot, _rotationPerFrame * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
    }

    private void HandleGravity()
    {
        if (_characterController.isGrounded)
        {
            //float groundedGravity = .05f;
            //moveDir.y = groundedGravity;
            if (IsFall)
            {
                IsFall = false;
                DOTween.To(x => _characterController.height = x, _characterController.height, _normalHeight, 0.2f);
                DOTween.To(x => _characterController.center = new Vector3(0, x, 0), _characterController.center.y, _normalYCenter, 0.2f).OnComplete(()=>
                {
                    moveDir.y = 2;
                });
            }

            if (FallTime > minFallTime)
            {
                MovementCooldown = fallCooldownTime;
            }
            FallTime = 0;
        }
        else
        {
            moveDir.y = Physics.gravity.y;
            FallTime += Time.deltaTime;

            if (!IsFall)
            {
                IsFall = true;
                DOTween.To(x => _characterController.height = x, _characterController.height, _fallHeight, 0.2f);
                DOTween.To(x => _characterController.center = new Vector3(0, x, 0), _characterController.center.y, _fallYCenter, 0.2f);
            }
        }
    }
}
