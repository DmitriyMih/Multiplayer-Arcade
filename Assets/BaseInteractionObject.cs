using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteractionObject : MonoBehaviour
{
    [SerializeField] private BoxCollider _collider;
    private Transform _hand;

    public Rigidbody _rigidbody;
    public float ObjectVelocity = 20f;

    [Header("Shoot Settings")]
    private float _age; 
    private float _gravity = 9.81f;

    private void Awake()
    {
        _collider.enabled = true;
        _rigidbody.isKinematic = false;
    }

    public virtual void TakeObject(Transform hand)
    {
        _collider.enabled = false;    
        _rigidbody.isKinematic = true;

        _hand = hand;
    }

    public virtual void DropObject(/*Transform target*/)
    {
        //Debug.Log("Drop");
        _collider.enabled = true;    
        _rigidbody.isKinematic = false;

        _age = 0;
    }
}
