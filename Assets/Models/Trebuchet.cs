using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Trebuchet : MonoBehaviour
{
    [SerializeField] private Transform contentPlace;

    [Header("State Settings")]
    [SerializeField] private float reloadTime;
    [SerializeField] private float replaceTime;

    [Header("Place Settings")]
    [SerializeField] private float placeTime = 0.3f;
    [SerializeField] private float placeRotationTime = 0.35f;

    [Header("Throw Settings")]
    [SerializeField] private float throwAnimationCooldown;
    [SerializeField] private float throwVelocity;

    [SerializeField] private float throwDropTime = 0.5f;

    [SerializeField] private BaseInteractionObject baseInteractionObject;

    private Animator animator;
    private bool isCharged;

    public bool IsPlace => contentPlace.childCount > 0;
    public string SendlerID = "Trebuchet";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void PlaceObject(GameObject newObject)
    {
        if (newObject.TryGetComponent(out BaseInteractionObject interactionObject) && IsPlace == false)
        {
            interactionObject.transform.parent = contentPlace;

            interactionObject.TakeObject(contentPlace, SendlerID);
            interactionObject.transform.DOLocalMove(Vector3.zero, placeTime);
            interactionObject.transform.DOLocalRotate(Vector3.zero, placeRotationTime).OnComplete(()=>
            {
                baseInteractionObject = interactionObject;
            });
        }
    }

    [ContextMenu("Drop")]
    public void Throw()
    {
        if (baseInteractionObject == null || IsPlace == false || isCharged == false)
            return;

        Debug.Log("Throw");
        StartCoroutine(ThrowObject(baseInteractionObject, throwAnimationCooldown, throwDropTime, true));
    }

    private IEnumerator ThrowObject(BaseInteractionObject interactionObject, float animationCooldown, float cooldown, bool isVelocity)
    {
        animator.SetTrigger("ThrowTrigger");
        baseInteractionObject = null;

        yield return new WaitForSeconds(animationCooldown);

        interactionObject.transform.parent = null;
        interactionObject._rigidbody.velocity = contentPlace.forward * throwVelocity;

        interactionObject._rigidbody.isKinematic = false;

        yield return new WaitForSeconds(cooldown);

        Debug.Log("New Velocity " + throwVelocity);
        interactionObject.DropObject();

        //_interactionObject = handPoint.GetComponentInChildren<BaseInteractionObject>();
        //Take(_interactionObject);
    }

    private void Update()
    {
        if (IsPlace)
            ReloadTrebuchet();
        else
            ReplaceTrebuchet();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Throw();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlaceObject(other.gameObject);
    }

    private void ReloadTrebuchet()
    {
        if (isCharged)
            return;

        StartCoroutine(Action(transform, reloadTime));
        //isCharged = true;
    }

    private void ReplaceTrebuchet()
    {
        if (!isCharged)
            return;

        StartCoroutine(Action(false, replaceTime));
        //isCharged = false;
    }

    private IEnumerator Action(bool isState, float time)
    {
        animator.SetBool(nameof(isCharged), isState);
        yield return new WaitForSeconds(time);

        isCharged = isState;
    }
}
