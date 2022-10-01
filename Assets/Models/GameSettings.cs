using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    private GameObject content;

    private PlayerMovement playerMovement;

    private List<Joystick> joysticks = new List<Joystick>();

    private void Initialization()
    {
        joysticks.AddRange(content.GetComponentsInChildren<Joystick>());
        if(playerMovement!=null)
            playerMovement.joys
    }



    public void OpenPanel()
    {
        content.SetActive(true);
    }

    public void ClosedPanel()
    {
        content.SetActive(false);
    }
}
