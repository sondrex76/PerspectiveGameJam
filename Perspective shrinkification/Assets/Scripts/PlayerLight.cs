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
    float minSpotlightRadius;       // Minimum spotlight radius
    [SerializeField]
    float maxSpotlightRadius;       // Maximum spotlight radius
    [SerializeField]
    float maxFlashLight;
    [SerializeField]
    float flashLightRange;          // Base range of flashlight
    [SerializeField]
    float flashLightRangeMult;      // Multiplier of size the flashlight increases with
    [SerializeField]
    float offsetFlashlight = 0.7f;
    [SerializeField]
    float multOffsetFlashlight = -0.7f;
    [SerializeField]
    float minOffsetFlashlight;
    [SerializeField]
    float maxOffsetFlashlight;

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

    // Menu
    [SerializeField]
    PauseMenu pauseMenu;


    // Functions


    // Update is called once per frame
    void Update()
    {
        if (!pauseMenu.returnPaused())
        {
            UpdateLghts();
        }
    }

    // Updates lights
    void UpdateLghts()
    {
        // Currently only works for down
        float relSizeMult = playerMovementScript.ReturnRelativeSizeMult();
        
        // Light intensity
        passiveLight.intensity = minPassiveLight + (maxPassiveLight - minPassiveLight) * relSizeMult;       // Manages passive light intensity
        flashlight.intensity = minFlashLight + (maxFlashLight - minFlashLight) * relSizeMult;               // Manages flashlight intensity
        flashlight.spotAngle = minSpotlightRadius * (1 - relSizeMult) + maxSpotlightRadius * relSizeMult;   // Manages radius of flashlight
        flashlight.range = flashLightRange * playerObject.localScale.x;                                     // Manages range of flashlight

        // Rotates light based on mouse position
        flashlight.transform.parent.up = -returnDirectionMouse();

        // Corrects distance between player and light
        Vector3 flashlightCurrentPos = new Vector3(0, 0, 0);
        flashlightCurrentPos.y = minOffsetFlashlight * (1 - relSizeMult) + maxOffsetFlashlight * relSizeMult;
        flashlight.transform.localPosition = flashlightCurrentPos;           // Updates position

        // Needs to offset light based on size and make light able to be rotated around player

        // Currently does not do anything
        /*
        Vector3 newPosition = new Vector3(0, 0, 0);
        newPosition.y = offsetFlashlight + multOffsetFlashlight * (playerObject.localScale.x - 1.0f);
        
        flashlight.transform.localPosition = newPosition;
        */
    }

    // Returns the direction from the player to the mouse
    Vector2 returnDirectionMouse()
    {
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lightDirection = (mouseScreenPosition - (Vector2)transform.position).normalized;

        return lightDirection;
    }
}
