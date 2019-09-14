using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    // Serialized variables

    // Floats
    [SerializeField]
    float minPassiveLight = 0.1f;       // Light strength at min size
    [SerializeField]
    float maxPassiveLight = 0.5f;       // Light strength at max size
    [SerializeField]
    float minFlashLight;
    [SerializeField]
    float minSpotlightRadius;           // Minimum spotlight radius
    [SerializeField]
    float maxSpotlightRadius;           // Maximum spotlight radius
    [SerializeField]
    float maxFlashLight;
    [SerializeField]
    float flashLightRange;              // Base range of flashlight
    [SerializeField]
    float flashLightRangeMult;          // Multiplier of size the flashlight increases with
    [SerializeField]
    float offsetFlashlight = 0.7f;
    [SerializeField]
    float multOffsetFlashlight = -0.7f; // 
    [SerializeField]
    float minOffsetFlashlight;          // Min offset of flash light
    [SerializeField]
    float modOffsetFlashlight;          // Moderate offset of flash light
    [SerializeField]
    float maxOffsetFlashlight;          // Max offset of flash light

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

    [SerializeField]
    SpriteRenderer flashlightSprite;

    // Menu
    [SerializeField]
    PauseMenu pauseMenu;

    // Local variables
    bool FlashlightOn = true;

    // Functions


    // Update is called once per frame
    void Update()
    {
        if (!pauseMenu.returnPaused())
        {
            UpdateLights();
        }
    }

    // Updates lights
    void UpdateLights()
    {
        if (Input.GetKeyDown(KeyCode.L))    // Turns light on/off if the "L" key is pressed
            FlashlightOn = !FlashlightOn;

        flashlight.enabled = flashlightSprite.enabled = FlashlightOn;

        if (FlashlightOn)
        {
            // Currently only works for down
            float relSizeMult = playerMovementScript.ReturnRelativeSizeMult();

            // Light intensity
            passiveLight.intensity = minPassiveLight + (maxPassiveLight - minPassiveLight) * relSizeMult;           // Manages passive light intensity
            flashlight.intensity = minFlashLight + (maxFlashLight - minFlashLight) * relSizeMult;                   // Manages flashlight intensity
            flashlight.spotAngle = minSpotlightRadius * (1 - relSizeMult) + maxSpotlightRadius * relSizeMult;       // Manages radius of flashlight
            flashlight.range = flashLightRange * playerObject.localScale.x;                                         // Manages range of flashlight

            // Rotates light based on mouse position
            flashlight.transform.parent.up = -returnDirectionMouse();

            // Corrects distance between player and light
            Vector3 flashlightCurrentPos = new Vector3(0, 0, 0);

            // Interpolation between three values, looked for the formulas but looks like I will just have to use an if statement
            if (relSizeMult > 0.5f)
            {
                flashlightCurrentPos.y = (modOffsetFlashlight * (1 - relSizeMult) + maxOffsetFlashlight * (relSizeMult - 0.5f)) * 2;
            }
            else
            {
                flashlightCurrentPos.y = (minOffsetFlashlight * (0.5f - relSizeMult) + modOffsetFlashlight * relSizeMult) * 2;
            }

            flashlight.transform.localPosition = flashlightCurrentPos;                                              // Updates position
        }
    }

    // Returns the direction from the player to the mouse
    Vector2 returnDirectionMouse()
    {
        Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lightDirection = (mouseScreenPosition - (Vector2)transform.position).normalized;

        return lightDirection;
    }
}
