using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    // Serialized variables

    // Floats
    [SerializeField]
    float minPassiveLight = 0.1f;   // Light strength at min size
    [SerializeField]
    float maxPassiveLight = 0.5f;   // Light strength at max size
    [SerializeField]
    float minFlashLight;
    [SerializeField]
    float maxFlashLight;
    [SerializeField]
    float offsetFlashlight = 0.7f;
    [SerializeField]
    float multOffsetFlashlight = -0.7f;

    // Lights
    [SerializeField]
    Light flashlight;   // flashlight
    [SerializeField]
    Light passiveLight; // Passive light

    // Player object
    [SerializeField]
    Transform playerObject;
    [SerializeField]
    PlayerMovement playerMovementScript;

    // Functions


    // Update is called once per frame
    void Update()
    {
        UpdateLghts();
    }

    // Updates lights
    void UpdateLghts()
    {
        // Currently only works for down

        // Light intensity
        passiveLight.intensity = minPassiveLight + (maxPassiveLight - minPassiveLight) * playerMovementScript.ReturnRelativeSizeMult();


        // Currently does not do anything
        /*
        Vector3 newPosition = new Vector3(0, 0, 0);
        newPosition.y = offsetFlashlight + multOffsetFlashlight * (playerObject.localScale.x - 1.0f);
        
        flashlight.transform.localPosition = newPosition;
        */
    }

    // Returns the direction from the player to the mouse
    Vector3 returnDirectionMouse()
    {
        Vector3 lightDirection;
        lightDirection = Input.mousePosition;
        lightDirection.z = 0.0f;
        lightDirection = Camera.main.ScreenToWorldPoint(lightDirection);
        lightDirection = lightDirection - transform.position;


        return lightDirection;
    }
}
