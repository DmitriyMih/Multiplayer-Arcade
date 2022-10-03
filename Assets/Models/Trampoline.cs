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
        Debug.Log("Throw" + newObject);
        newObject.velocity = new Vector3(throwDirection.x * directionForce, throwForce, throwDirection.y * directionForce);
    }

    private void ThrowUpCharacter(CharacterController character)
    {
        Debug.Log("Throw Character");
        character.SimpleMove(new Vector3(throwDirection.x * directionForce, throwDirection.y * directionForce, throwDirection.z * throwForce));
    }

    private void CheckOther(Collider other)
    {
        if (other.TryGetComponent(out CharacterController character))
        {
            ThrowUpCharacter(character);
        }

        if (other.TryGetComponent(out Rigidbody newObject))
        {
            ThrowObject(newObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckOther(other);
    }

    private void OnTriggerStay(Collider other)
    {
        CheckOther(other);
    }
}
