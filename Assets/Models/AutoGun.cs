using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoGun : MonoBehaviour
{
    [SerializeField] private Transform bodyGroup;
    [SerializeField] private float horizontalVelocity = 5f;
    [SerializeField] private float verticalVelocity = 5f;

    [Header("Shoot Settings")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform muzzlePoint;

    [SerializeField] private BaseInteractionObject interactionPrefab;
    [SerializeField] private BaseInteractionObject interactionObject;

    [SerializeField] private float newVelocity = 10f;

    [SerializeField] private bool isCharged;

    public bool IsAutomatic = true;
    public string SendlerID = "Gun";

    private void Awake()
    {

    }

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
        box.TakeObject(muzzlePoint, SendlerID);
        
        box.transform.parent = muzzlePoint;
        interactionObject = box;

        box.transform.DOLocalMove(Vector3.zero, 0.2f).OnComplete(() =>
        {
            isCharged = true;
        });
    }

    [ContextMenu("Shoot")]
    private void Shoot()
    {
        if (interactionObject == null || !isCharged)
            return;

        interactionObject.DropObject();
        interactionObject.transform.parent = null;
        interactionObject._rigidbody.velocity = muzzlePoint.forward * newVelocity;

        isCharged = false;
        interactionObject = null;
    }
}
