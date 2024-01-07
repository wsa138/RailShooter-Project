using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] InputAction movement;
    [SerializeField] float xThrowTuning;
    [SerializeField] float yThrowTuning;
    [SerializeField] float xRange = 5f;
    [SerializeField] float yRange = 4.25f;

    [SerializeField] float positionPitchFactor = -3f;
    [SerializeField] float controlPitchFactor = -15f;
    [SerializeField] float rotationYawFactor = 4f;
    [SerializeField] float controlRollFactor = -10f;

    [SerializeField] InputAction firing;
    [SerializeField] GameObject[] lasers;

    float xThrow;
    float yThrow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        movement.Enable();
        firing.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        firing.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    private void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;

        float yawDueToPosition = transform.localPosition.x * rotationYawFactor;

        float rollDueToControlThrow = transform.localRotation.z + xThrow * controlRollFactor;

        float pitch = pitchDueToPosition + pitchDueToControlThrow;
        float yaw = yawDueToPosition;
        float roll = rollDueToControlThrow;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }


    private void ProcessTranslation()
    {
        xThrow = movement.ReadValue<Vector2>().x;
        yThrow = movement.ReadValue<Vector2>().y;

        // Left Right movement
        float xOffset = xThrow * Time.deltaTime * xThrowTuning;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        // Up Down movement
        float yOffset = yThrow * Time.deltaTime * yThrowTuning;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    void ProcessFiring()
    {
        // if pushing fire button
        // then print "shooting"
        // else don't print "shooting"
        if (firing.ReadValue<float>() > 0.5)
        {
            ActivateLasers();
        } else
        {
            DeactivateLasers();
        }
    }
    void ActivateLasers()
    {
        // for each of the lasers that we have, turn them on
        foreach (GameObject laser in lasers)
        {
            laser.SetActive(true);
        }
    }

    void DeactivateLasers()
    {
        // for each of the lasers that we have, turn them off
        foreach (GameObject laser in lasers)
        {
            laser.SetActive(false);
        }
    }
}
