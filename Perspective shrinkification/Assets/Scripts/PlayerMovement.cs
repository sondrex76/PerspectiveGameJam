using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Serialized values
    
    // Movement speed
    [Range(1, 20)]
    [SerializeField]
    float movementSpeed = 10.0f;    
    
    // Runnning speed
    [Range(2, 30)]
    [SerializeField]
    float runningSpeed = 12.0f;
    [SerializeField]
    float minSize = 1.0f;
    [SerializeField]
    float maxSize = 21.0f;

    // Private values
    Rigidbody2D playerCollision;    // Player collission
    Camera cameraElement;           // Camera(for perspective changes)
    bool isRunning = false;
    float currentSize = 1.0f;       // Current player size

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Changes size to the specfied value
    void changeSize(float newSize)
    {

    }
}
