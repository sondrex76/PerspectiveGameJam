using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // I really should have split this class into two or three
    // Serialized values
    [SerializeField]
    LayerMask groundLayer;                  // Ground
    [SerializeField]
    PauseMenu pauseMenu;                    // Pause menu
    [SerializeField]
    Camera cameraElement;                   // Camera(for perspective changes)

    [SerializeField]
    float stepOverrshoot = 0.01f;           // How muhc to overshoot
    [SerializeField]
    float stepHeight = 0.3f;                // Step height           
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
    float minSize = 1.0f;                   // Minimum size
    [SerializeField]
    float maxSize = 12.0f;                  // Maximum size
    [SerializeField]
    float sizeSpeed = 1.0f;                 // Speed of size changing
    [SerializeField]
    float jumpHeight = 1.0f;                // Jump height(speed given to jump at jump button
    [SerializeField]
    float minSizeModifier = 4.58257569496f; // When at min size speed and jump height are both multiplied with this in addition to the size modifier
    [SerializeField]
    float maxSizeModifier = 1.0f;           // When at max size speed and jump height are both multiplied with this in addition to the size modifier
    [SerializeField]
    float minCameraSize = 5.0f;             // Minimum camera size
    [SerializeField]
    float maxCameraSize = 50.0f;            // Maximum camera size
    [SerializeField]
    float ladderMin = 5.0f;                 // Minimum size to allow the player to move up ladders
    [SerializeField]
    float ladderSpeed = 1.0f;               // Ladder speed
    [SerializeField]
    float ladderAcceleration = 2.0f;        // Ladder speed


    // Private values
    Rigidbody2D playerCollision;    // Player collission
    bool isRunning = false;         // Bool for running
    bool goDown = false;            // bool to check if you want to go downwards(if features utilizing this is implemented)
    bool canGrow = true;            // Bool identifying if there are neough space to grow
    bool onLadder = false;          // Is on ladder
    float currentSize = 1.0f;       // Current player size
    float currentSizeGoal;          // Currnet size goal
    float sizeMod;                  // Current size modifier, decleared here to be returnable to other functions by a function

    // Start is called before the first frame update
    void Start()
    {
        playerCollision = GetComponent<Rigidbody2D>();  // Gets the player's rigidbody for velocity
        cameraElement.orthographicSize = minCameraSize;
        currentSizeGoal = minSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseMenu.returnPaused())  // Checks if the game is paused and does not run any game logic if it is
        {
            if (onLadder)   // is on ladder
            {
                if (Input.GetKey(KeyCode.Space))    // If they are hitting space
                {
                    Vector2 currentVel = playerCollision.velocity;

                    if (transform.localScale.x > ladderMin) // Move up ladder or stay still
                    {

                        if (currentVel.y + ladderSpeed * Time.deltaTime * transform.localScale.x > ladderSpeed * transform.localScale.x)
                            currentVel.y = ladderSpeed * transform.localScale.x;
                        else
                            currentVel.y += ladderAcceleration * transform.localScale.x;

                    }
                    else
                    {
                        if (currentVel.y < 0)
                            currentVel.y = 0;
                    }

                    playerCollision.velocity = currentVel;
                }
            }

            sizeMod = ReturnRelativeSizeMult();
            // Sets the modifier to its actual value
            sizeMod = sizeMod * maxSizeModifier * maxSize + (1 - sizeMod) * minSizeModifier * minSize;

            // Actual functions
            UpdateMovement(sizeMod);   // Updates movement
            UpdateSize();                   // Updates size
        }
    }

    private void LateUpdate()
    {
        if (!pauseMenu.returnPaused())  // Checks if the game is paused and does not run any game logic if it is
        {
            CameraUpdate();
        }
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

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))     // Jump
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
        if (Input.GetKeyDown(KeyCode.M))    // Set max size as the target
        {
            if (currentSizeGoal != maxSize)
                currentSizeGoal = maxSize;
            else
                currentSizeGoal = currentSize;
        }
        if (Input.GetKeyDown(KeyCode.N))    // Set min size as the target
        {
            if (currentSizeGoal != minSize)
                currentSizeGoal = minSize;
            else
                currentSizeGoal = currentSize;
        }

        if (currentSize != currentSizeGoal)
        {
            ChangeSize(currentSizeGoal);
            playerCollision.mass = Mathf.Pow(playerCollision.transform.localScale.x, 3);
        }
    }

    void CameraUpdate()
    {
        // Causes stuttering horizontally
        // I want it vertically but not horizontally
        cameraElement.transform.position = new Vector3(playerCollision.transform.position.x, Mathf.Lerp(cameraElement.transform.position.y, playerCollision.transform.position.y, Time.deltaTime * jumpHeight * 2), -10.0f);
    }

    // Changes size to the specfied value
    void ChangeSize(float newSize)
    {
        if (currentSize < newSize)
        {                           // Increase size
            if (canGrow)            // Are there enough space to grow?
            {
                if (currentSize + sizeSpeed * Time.deltaTime > maxSize)
                {
                    currentSize = maxSize;
                }
                else
                    currentSize += sizeSpeed * Time.deltaTime;
            }
            else
                currentSizeGoal = currentSize;
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

    // Returns size modifier
    public float ReturnSizeMod()
    {
        return sizeMod;
    }

    // returns a value between 0 and 1 where 0 is min size and 1 is max
    public float ReturnRelativeSizeMult()
    {
        return (currentSize - minSize) / (maxSize - minSize);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ladder"))   // If the object is a ladder
        {
            Debug.Log("here");
            onLadder = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")   // If the object is a ladder
        {
            onLadder = false;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))  // If colliding with ground
        {

            // Checks if there are something closeby up or if tehre aren't enough space to the left and right to grow
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, currentSize * 0.55f, groundLayer);
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, currentSize * 0.4f, groundLayer);
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, currentSize * 0.4f, groundLayer);

            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground") || 
                hitLeft.collider != null && hitLeft.collider.gameObject.layer == LayerMask.NameToLayer("Ground")
                && hitRight.collider != null && hitRight.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                canGrow = false;
            }
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
            // Checks if there are something closeby up or if tehre aren't enough space to the left and right to grow
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, currentSize * 0.55f, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, currentSize * 0.4f, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, currentSize * 0.4f, groundLayer);

        // If there are enough space to grow
        if (hitLeft.collider == null ||
             hitRight.collider == null &&
             hit.collider == null
             )
        {
            canGrow = true;
        }
    }
}
