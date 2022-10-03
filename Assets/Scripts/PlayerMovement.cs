using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(1, 15f)] private float speed = 5f;
    [SerializeField, Range(1, 20)] private float rotationPerFrame = 10f;

    private CharacterController characterController;

    [SerializeField] private PlayerAnimator playerAnimator;

    [SerializeField] private float minFallTime = 0.5f;
    [SerializeField] private float fallCooldownTime = 1f;
    [SerializeField] private float throwCooldownTime = 0.4f;

    [Header("Fall Debug")]
    private float _normalHeight;
    [SerializeField] private float fallHeight;

    private float _normalYCenter;
    [SerializeField] private float fallYCenter;

    [Header("Inventory")]
    [SerializeField] private Transform handPoint;
    [SerializeField] private BaseInteractionObject interactionObject;

    [Header("Throw")]
    [SerializeField] private float throwVelocity = 15f;
    [SerializeField] private float dropVelocity = 4f;
    [SerializeField] private float animationCooldown = 0.2f;

    [SerializeField] private float throwDropTime = 0.3f;

    public bool ButtonInteractable;
    
    public Joystick joystick;
    public float MoveVelocity => characterController.velocity.sqrMagnitude;
    public float LayerWeight;

    public bool IsMove { get; private set; }
    public bool IsFall { get; private set; }

    public bool TakenInHand => handPoint.childCount > 0;
    public float FallTime { get; private set; }
    public float CharacterCooldown { get; private set; }

    [Header("Multi")]
    [SerializeField] private string playerID;
    public string PlayerID => playerID;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        _normalHeight = characterController.height;
        _normalYCenter = characterController.center.y;
    }

    private void Update()
    {
        HandleGravity();
        Movement();
        Rotate();

        if (transform.position.y <= -50f)
            transform.position = new Vector3(0, 7f, 0);
    }


    private Vector3 moveDir;
    private void Movement()
    {
        if (joystick == null)
        {
            Debug.Log("Joystick Is Null");
            return;
        }

        if (CharacterCooldown > 0)
        {
            characterController.SimpleMove(Vector3.zero);
            CharacterCooldown -= Time.deltaTime;
            return;
        }

        moveDir.Set(joystick.Horizontal, 0, joystick.Vertical);
        //float speed = _speed;//_player.Inventory.HasTurret ? _speedWithTurret : _speed;
        //LayerWeight = _player.Inventory.HasTurret ? 1 : 0;

        //float animationSpeed = _player.Inventory.HasTurret ? _speedWithTurret * _speedWithTurretCoef : _speed * _speedCoef;
        //_player.PlayerAnimations.AnimationSpeed = animationSpeed;

        characterController.SimpleMove(moveDir * speed);
        IsMove = moveDir.magnitude > 0 && characterController.isGrounded;
    }

    private void Rotate()
    {
        if (moveDir.magnitude > 0)
        {
            Quaternion currentRot = transform.rotation;
            Quaternion targetRot = Quaternion.LookRotation(moveDir);

            transform.rotation = Quaternion.Slerp(currentRot, targetRot, rotationPerFrame * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        }
    }

    [ContextMenu("Take")]
    private void Take(BaseInteractionObject takeObject)
    {
        if (interactionObject != null || TakenInHand == true || takeObject == null)
            return;

        playerAnimator.TakeInHand(true);

        takeObject.TakeObject(handPoint, PlayerID);

        takeObject.transform.parent = handPoint.transform;

        takeObject.transform.DORotate(Vector3.zero, 0.6f);
        takeObject.transform.DOLocalMove(Vector3.zero, 0.5f);
    }

    [ContextMenu("Drop")]
    public void Throw()
    {
        if (interactionObject == null || TakenInHand == false)
            return;

        Debug.Log("Throw");
        StartCoroutine(ThrowObject(interactionObject, animationCooldown, throwDropTime, true));
    }

    private void Drop()
    {
        if (interactionObject == null || TakenInHand == false)
            return;

        StartCoroutine(ThrowObject(interactionObject, 0, throwDropTime, false));
    }

    private IEnumerator ThrowObject(BaseInteractionObject interactionObject, float animationCooldown, float cooldown, bool isVelocity)
    {
        //CharacterCooldown = throwCooldownTime;
        playerAnimator.TakeInHand(false);

        yield return new WaitForSeconds(animationCooldown);

        interactionObject.transform.parent = null;
        float newVelocity = isVelocity ? throwVelocity : dropVelocity;

        Debug.Log("New Velocity " + newVelocity);
        interactionObject._rigidbody.velocity = transform.forward * newVelocity;
        interactionObject._rigidbody.isKinematic = false;

        yield return new WaitForSeconds(cooldown);

        interactionObject.DropObject();

        this.interactionObject = handPoint.GetComponentInChildren<BaseInteractionObject>();
        Take(this.interactionObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BaseInteractionObject interactionObject) && CharacterCooldown <= 0 && TakenInHand == false)
        {
            Take(interactionObject);
            this.interactionObject = interactionObject;
        }
    }

    private void HandleGravity()
    {
        if (characterController.isGrounded)
        {
            if (IsFall)
            {
                IsFall = false;
                DOTween.To(x => characterController.height = x, characterController.height, _normalHeight, 0.2f);
                DOTween.To(x => characterController.center = new Vector3(0, x, 0), characterController.center.y, _normalYCenter, 0.2f).OnComplete(() =>
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
                DOTween.To(x => characterController.height = x, characterController.height, fallHeight, 0.2f);
                DOTween.To(x => characterController.center = new Vector3(0, x, 0), characterController.center.y, fallYCenter, 0.2f);
            }
        }
    }
}
