using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField]
    Transform player;                   // The player's field, to know the player's current size
    [SerializeField]
    float sizeThresholdRun;             // Threshold where the spider starts running away(above this value)
    [SerializeField]
    float sizeThresholdFollow;          // Threshold where the spider starts running towards you(below this vlaue)
    [SerializeField]
    float spiderVelocity = 10.0f;       // Velocity of spider
    [SerializeField]
    float spiderAcceleration = 20.0f;   // Acceleration of spider
    
    // Update is called once per frame
    void Update()
    {
        
    }


    // When something enters their view
    private void OnCollisionStay(Collision collision)
    {
        // CHecks if it were the player
        if (collision.gameObject.tag == "Player")
        {

        }
    }
}
