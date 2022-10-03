using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject interactButton;

    [SerializeField] private GameObject closedButton;

    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private Image[] lightImage;

    [SerializeField] private Toggle joystickToggle;

    [SerializeField] private List<Joystick> joysticks = new List<Joystick>();

    private void Awake()
    {
        content.SetActive(false);
        closedButton.SetActive(false);
        Initialization();
    }

    private void Initialization()
    {
        if (playerMovement != null)
            SwitchJoystick(joystickToggle.isOn);

        SwitchControl(true);
    }

    public void SwitchControl(bool isButton)
    {
        switch (isButton)
        {
            case true:
                interactButton.SetActive(true);

                lightImage[0].gameObject.SetActive(true);
                lightImage[1].gameObject.SetActive(false);
                break;

            case false:
                interactButton.SetActive(false);

                lightImage[0].gameObject.SetActive(false);
                lightImage[1].gameObject.SetActive(true);
                break;
        }
        playerMovement.ButtonInteractable = isButton;
    }

    public void SwitchJoystick(bool togglState)
    {
        int id;
        switch (togglState)
        {
            case false:
                id = 0;
                joysticks[0].gameObject.SetActive(true);
                joysticks[1].gameObject.SetActive(false);
                break;

            case true:
                id = 1;
                joysticks[0].gameObject.SetActive(false);
                joysticks[1].gameObject.SetActive(true);
                break;
        }

        playerMovement.joystick = joysticks[id];
    }

    public void OpenPanel()
    {
        content.SetActive(true);
        closedButton.SetActive(true);
    }

    public void ClosedPanel()
    {
        content.SetActive(false);
        closedButton.SetActive(false);
    }
}
