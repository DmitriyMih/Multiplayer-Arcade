using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoGun : MonoBehaviour
{
    [SerializeField] private Transform bodyGroup;
    [SerializeField] private float angleVelocity = 5f;

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
        if (horizontal > 0)
            RotateBody(horizontal);
    }

    private void RotateBody(float angle)
    {
        transform.Rotate(new Vector3(0, angle * angleVelocity, 0));
    }

    [ContextMenu("Init Box")]
    private void InstatiateBox()
    {
        if (interactionObject != null || isCharged)
            return;

        BaseInteractionObject box = Instantiate(interactionPrefab, spawnPoint);
        box.transform.parent = muzzlePoint;

        interactionObject = box;
        interactionObject.TakeObject(muzzlePoint, SendlerID);

        box.transform.DOLocalMove(muzzlePoint.position, 0.2f).OnComplete(() =>
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
        interactionObject._rigidbody.velocity = transform.forward * newVelocity;
    }
}
