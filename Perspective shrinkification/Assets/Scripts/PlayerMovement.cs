using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Public variables
    public LayerMask groundLayer;

    [SerializeField]
    Camera cameraElement;           // Camera(for perspective changes)
    // Serialized values
    [SerializeField]
    float gravityScale = 0.8f;              // Gravity multiplier
    [SerializeField]
    float movementSpeed = 10.0f;            // Movement speed
    [SerializeField]
    float runningSpeed = 12.0f;             // Runnning speed
    [SerializeField]
    float moveAcceleration = 1.0f;          // Walking acceleration
    [SerializeField]
    float runAcceleration = 1.2f;           // Running acceleration
    [SerializeField]
    float minSize = 1.0f;
    [SerializeField]
    float maxSize = 12.0f;
    [SerializeField]
    float minZoom = 1.0f;
    [SerializeField]
    float sizeSpeed = 1.0f;
    [SerializeField]
    float jumpHeight = 1.0f;                // Jump height(speed given to jump at jump button
    [SerializeField]
    float minSizeModifier = 4.58257569496f; // When at min size speed and jump height are both multiplied with this in addition to the size modifier
    [SerializeField]
    float maxSizeModifier = 1.0f;           // When at max size speed and jump height are both multiplied with this in addition to the size modifier
    [SerializeField]
    float minCameraSize = 5.0f;            // Minimum camera size
    [SerializeField]
    float maxCameraSize = 50.0f;             // Maximum camera size

    // Private values
    Rigidbody2D playerCollision;    // Player collission
    bool isRunning = false;         // Bool for running
    bool goDown = false;            // bool to check if you want to go downwards(if features utilizing this is implemented)
    float currentSize = 1.0f;       // Current player size
    float currentSizeGoal = 1.0f;   // Currnet size goal

    // Start is called before the first frame update
    void Start()
    {
        // Hides and locks cursor(commented out because I might need the mouse pointer for things later
        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.Locked;
        playerCollision = GetComponent<Rigidbody2D>();  // Gets the player's rigidbody for velocity
        cameraElement.orthographicSize = minCameraSize;
    }

    // Update is called once per frame
    void Update()
    {
        float sizeModifier = (currentSize - minSize) / (maxSize - minSize);
        // Sets the modifier to its actual value
        sizeModifier = sizeModifier * maxSizeModifier * maxSize + (1 - sizeModifier) * minSizeModifier * minSize;
        
        // Actual functions
        UpdateMovement(sizeModifier);   // Updates movement
        UpdateSize();                   // Updates size
    }

    // Detects key presses related to mvoement and acts on them
    void UpdateMovement(float sizeModifier)
    {
        Vector2 currentVelocity = playerCollision.velocity; // Gets current velocity

        // Checks if character is currently running
        isRunning = Input.GetKey(KeyCode.LeftShift);
        
        // Ensures speed cannot go over current max
        float currentAcceleration = sizeModifier, currentMaxSpeed = sizeModifier;
        if (isRunning)
        {
            currentAcceleration *= runAcceleration;
            currentMaxSpeed *= runningSpeed;
        }
        else
        {
            currentAcceleration *= moveAcceleration;
            currentMaxSpeed *= movementSpeed;
        }
        
        // Actual movement
        if (Input.GetKey(KeyCode.A))                                    // Left
        {
            // Ensures the speed does not go over max
            if (currentVelocity.x + Time.deltaTime * currentAcceleration < -currentMaxSpeed)
            {
                // Makes speed decreasement not be instantanious
                if (currentVelocity.x - Time.deltaTime * currentAcceleration < -currentMaxSpeed)
                    currentVelocity.x += Time.deltaTime * currentAcceleration;
                else
                    currentVelocity.x = -currentMaxSpeed;
            }
            else
            {
                currentVelocity.x -= Time.deltaTime * currentAcceleration;
            }
        }
        if (Input.GetKey(KeyCode.D))                                    // Right
        {
            // Ensures the speed does not go over max
            if (currentVelocity.x + Time.deltaTime * currentAcceleration > currentMaxSpeed)
            {
                // Makes speed decreasement not be instantanious
                if (currentVelocity.x - Time.deltaTime * currentAcceleration > currentMaxSpeed)
                    currentVelocity.x -= Time.deltaTime * currentAcceleration;
                else
                    currentVelocity.x = currentMaxSpeed;
            }
            else
            {
                currentVelocity.x += Time.deltaTime * currentAcceleration;
            }
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space))     // Jump
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, currentSize * 0.55f, groundLayer);
            if (hit.collider != null)
            {
                currentVelocity.y = jumpHeight * sizeModifier;
            }
        }
        goDown = Input.GetKey(KeyCode.S);                               // Down(grapple-hook, if added)

        // Updates velocity
        playerCollision.velocity = currentVelocity;
    }

    // Updates size
    void UpdateSize()
    {
        if (Input.GetKey(KeyCode.M))    // TEMP
        {
            currentSizeGoal = maxSize;
        }

        if (currentSize < currentSizeGoal)
        {
            ChangeSize(currentSizeGoal);
        }
    }

    // Changes size to the specfied value
    void ChangeSize(float newSize)
    {
        if (currentSize < newSize)
        {                           // Increase size
            if (currentSize + sizeSpeed * Time.deltaTime > maxSize)
            {
                currentSize = maxSize;
            }
            else
                currentSize += sizeSpeed * Time.deltaTime;
        }
        else
        {                           // Decrease size
            if (currentSize - sizeSpeed * Time.deltaTime < minSize)
            {
                currentSize = minSize;
            }
            else
                currentSize -= sizeSpeed * Time.deltaTime;
        }

        transform.localScale = new Vector3(currentSize, currentSize, currentSize);
        playerCollision.gravityScale = currentSize * gravityScale;
        
        // Sets the camera's size
        cameraElement.orthographicSize = minCameraSize + ((currentSize - minSize) / (maxSize - minSize)) * (maxCameraSize - minCameraSize);
    }
}
