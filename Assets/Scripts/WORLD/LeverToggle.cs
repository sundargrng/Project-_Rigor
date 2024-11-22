using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverToggle : Interactable
{
    public Sprite ToggleOn;
    public Sprite ToggleOff;

    private SpriteRenderer spriteRenderer;
    private bool isOn = false;

    public MovingPlatforms movingPlatforms; // Make the variable public

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetToggleOff(); // Set the initial state to closed
    }

    public override void Interact()
    {
        if (isOn)
        {
            Off();
        }
        else
        {
            On();
        }
    }

    private void On()
    {
        spriteRenderer.sprite = ToggleOn;
        isOn = true;

        // Stop the platform when On() is called
        if (movingPlatforms != null)
        {
            movingPlatforms.StopPlatform();
        }
    }

    private void Off()
    {
        spriteRenderer.sprite = ToggleOff;
        isOn = false;

        // Start the platform when Off() is called
        if (movingPlatforms != null)
        {
            movingPlatforms.StartPlatform();
        }
    }

    private void SetToggleOff()
    {
        spriteRenderer.sprite = ToggleOff;
        isOn = false;
    }
}