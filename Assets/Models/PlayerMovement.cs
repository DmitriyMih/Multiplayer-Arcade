using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(1, 15f)] private float _speed = 5f;
    [SerializeField, Range(1, 20)] private float _rotationPerFrame = 10f;

    private CharacterController _characterController;

    [SerializeField] private float minFallTime = 0.5f;
    [SerializeField] private float fallCooldownTime = 1f;
    [SerializeField] private float throwCooldownTime = 0.4f;

    [Header("Fall Debug")]
    private float _normalHeight;
    [SerializeField] private float _fallHeight;

    private float _normalYCenter;
    [SerializeField] private float _fallYCenter;

    [Header("Inventory")]
    [SerializeField] private Transform handPoint;
    [SerializeField] private BaseInteractionObject _interactionObject;

    [SerializeField] private ShakeDetector shakeDetector;
    public bool ButtonInteractable;
    
    public Joystick _joystick;
    public float MoveVelocity => _characterController.velocity.sqrMagnitude;
    public float LayerWeight;

    public bool IsMove { get; private set; }
    public bool IsFall { get; private set; }
    //public bool Cary { get; private set; }

    public bool TakenInHand => handPoint.childCount > 0;
    public float FallTime { get; private set; }
    public float CharacterCooldown { get; private set; }

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

        if (CharacterCooldown > 0)
        {
            _characterController.SimpleMove(Vector3.zero);
            CharacterCooldown -= Time.deltaTime;
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

    [ContextMenu("Take")]
    private void Take(BaseInteractionObject takeObject)
    {
        if (_interactionObject != null || TakenInHand == true || takeObject == null)
            return;

        //Cary = true;
        takeObject.TakeObject(handPoint);

        takeObject.transform.parent = handPoint.transform;

        takeObject.transform.DORotate(Vector3.zero, 0.6f);
        takeObject.transform.DOLocalMove(Vector3.zero, 0.5f);
    }

    [SerializeField] private float throwVelocity = 15f;
    [SerializeField] private float dropVelocity = 4f;
    [SerializeField] private float _animationCooldown = 0.2f;

    [ContextMenu("Drop")]
    public void Throw()
    {
        if (_interactionObject == null || TakenInHand == false)
            return;

        Debug.Log("Throw");
        //Cary = false;
        StartCoroutine(ThrowObject(_interactionObject, _animationCooldown, 0.75f, true));
    }

    private void Drop()
    {
        if (_interactionObject == null || TakenInHand == false)
            return;

        //Cary = false;
        StartCoroutine(ThrowObject(_interactionObject, 0, 0.75f, false));
    }

    private IEnumerator ThrowObject(BaseInteractionObject interactionObject, float animationCooldown, float cooldown, bool isVelocity)
    {
        //CharacterCooldown = throwCooldownTime;
        interactionObject.transform.parent = null;

        yield return new WaitForSeconds(animationCooldown);

        float newVelocity = isVelocity ? throwVelocity : dropVelocity;
        Debug.Log("New Velocity " + newVelocity);
        interactionObject._rigidbody.velocity = transform.forward * newVelocity;
        interactionObject._rigidbody.isKinematic = false;

        yield return new WaitForSeconds(cooldown);

        interactionObject.DropObject();

        //if (TakenInHand == false)
        //    _interactionObject = null;
        //else
        _interactionObject = handPoint.GetComponentInChildren<BaseInteractionObject>();
        Take(_interactionObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BaseInteractionObject interactionObject) && CharacterCooldown <= 0 && TakenInHand == false)
        {
            Take(interactionObject);
            _interactionObject = interactionObject;
        }
    }

    private void HandleGravity()
    {
        if (_characterController.isGrounded)
        {
            if (IsFall)
            {
                IsFall = false;
                DOTween.To(x => _characterController.height = x, _characterController.height, _normalHeight, 0.2f);
                DOTween.To(x => _characterController.center = new Vector3(0, x, 0), _characterController.center.y, _normalYCenter, 0.2f).OnComplete(() =>
                {
                    moveDir.y = 2;
                });
            }

            if (FallTime > minFallTime)
            {
                CharacterCooldown = fallCooldownTime;
            }
            FallTime = 0;
        }
        else
        {
            moveDir.y = Physics.gravity.y;
            FallTime += Time.deltaTime;

            if (FallTime > minFallTime)
                Drop();

            if (!IsFall)
            {
                IsFall = true;
                DOTween.To(x => _characterController.height = x, _characterController.height, _fallHeight, 0.2f);
                DOTween.To(x => _characterController.center = new Vector3(0, x, 0), _characterController.center.y, _fallYCenter, 0.2f);
            }
        }
    }
}
