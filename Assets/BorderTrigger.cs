using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.position = new Vector3(0, 15, 0);
        Debug.Log($"|Enter| Object {other.gameObject} Teleportating To Spawn");
    }
    private void OnTriggerStay(Collider other)
    {
        other.gameObject.transform.position = new Vector3(0, 15, 0);
        Debug.Log($"|Stay| Object {other.gameObject} Teleportating To Spawn");
    }
}
