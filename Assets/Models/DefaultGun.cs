using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DefaultGun : MonoBehaviour
{
    [SerializeField] private Transform bodyGroup;
    [SerializeField] private float horizontalVelocity = 5f;

    [Header("Shoot Settings")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform muzzlePoint;

    [SerializeField] private BaseInteractionObject interactionPrefab;
    [SerializeField] private BaseInteractionObject interactionObject;

    [SerializeField] private float newVelocity = 10f;
    [SerializeField] private float timeToDrop = 0.3f;

    [SerializeField] private bool isCharged;

    public bool IsAutomatic = true;
    public string SendlerID = "Gun";

    private void Update()
    {
        if (IsAutomatic)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0 || vertical != 0)
            RotateBody(horizontal, vertical);

        if (Input.GetKeyUp(KeyCode.Q))
            InstatiateBox();

        if (Input.GetKeyUp(KeyCode.E))
            Shoot();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BaseInteractionObject interactionObject))
        {
            PlaceBox(interactionObject);
        }
    }

    private void RotateBody(float horizontalAngle, float verticalAngle)
    {
        Debug.Log(horizontalAngle);

        bodyGroup.Rotate(new Vector3(0f, horizontalAngle * horizontalVelocity, 0f));
    }

    [ContextMenu("Init Box")]
    private void InstatiateBox()
    {
        if (interactionObject != null || isCharged)
            return;

        BaseInteractionObject box = Instantiate(interactionPrefab, spawnPoint);
        PlaceBox(box);
    }

    private void PlaceBox(BaseInteractionObject baseInteraction)
    {
        if (interactionObject != null || isCharged)
            return;

        interactionObject = baseInteraction;

        interactionObject.TakeObject(muzzlePoint, SendlerID);
        interactionObject.transform.parent = muzzlePoint;

        interactionObject.transform.DOLocalRotate(Vector3.zero, 0.3f);
        interactionObject.transform.DOLocalMove(Vector3.zero, 0.2f).OnComplete(() =>
        {
            isCharged = true;
        });
    }

    [ContextMenu("Shoot")]
    private void Shoot()
    {
        if (interactionObject == null || !isCharged)
            return;

        StartCoroutine(Throw(timeToDrop));
    }

    private IEnumerator Throw(float time)
    {
        interactionObject._rigidbody.isKinematic = false;
        interactionObject.transform.parent = null;
        interactionObject._rigidbody.velocity = muzzlePoint.forward * newVelocity;

        isCharged = false;

        yield return new WaitForSeconds(time);
        
        interactionObject.DropObject();
        interactionObject = null;
    }
}
