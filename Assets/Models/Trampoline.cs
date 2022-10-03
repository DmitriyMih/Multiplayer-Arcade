using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{ 
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float directionForce = 2f;

    [SerializeField] private Vector3 throwDirection = Vector3.zero;

    private void ThrowObject(Rigidbody newObject)
    {
        Debug.Log("Throw");
        Vector3 vec = new Vector3(throwDirection.x, 1f, throwDirection.y);

        newObject.velocity = transform.up * throwForce;
        newObject.velocity = vec * directionForce;
    }

    private void ThrowUpCharacter(CharacterController character)
    {
        Debug.Log("Throw Character");
        Vector3 direct = new Vector3(throwDirection.x * directionForce, throwDirection.y * directionForce, throwDirection.z * throwForce);

        direct.y = throwForce;
        character.Move(direct);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody newObject))
        {
            ThrowObject(newObject);
        }

        if (other.TryGetComponent(out CharacterController character))
        {
            ThrowUpCharacter(character);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody newObject))
        {
            ThrowObject(newObject);
        }

        if (other.TryGetComponent(out CharacterController character))
        {
            ThrowUpCharacter(character);
        }
    }
}
