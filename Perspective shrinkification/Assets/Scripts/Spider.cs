using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField]
    Transform player;                   // The player's field, to know the player's current size
    [SerializeField]
    PauseMenu pause;                    // Pause menu
    [SerializeField]
    Rigidbody2D spiderBody;             // The rigidbody of spider
    [SerializeField]
    float sizeThresholdRun;             // Threshold where the spider starts running away(above this value)
    [SerializeField]
    float sizeThresholdFollow;          // Threshold where the spider starts running towards you(below this vlaue)
    [SerializeField]
    float spiderVelocity = 10.0f;       // Velocity of spider
    [SerializeField]
    float spiderAcceleration = 20.0f;   // Acceleration of spider

    private void Update()
    {
        spiderBody.simulated = !pause.returnPaused();   // Stops spider on pause
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // So, somehow this returns true but not the other one

        string thisIsTheTag = collision.gameObject.tag;

        // BUG: does not trigger in certain circomstances(when you jump up at a certain distance and am still within the range)
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 speedBase = (((Vector2)player.transform.position - (Vector2)transform.position).normalized);
            
            speedBase.y = 0;
            speedBase = speedBase.normalized;
            
            // -1, 0
            // 1 < 3 = true
            // Spider is definetly simulated
            // Speed is updated
            // The spider is not moving, reason: completely unknown
            // Unevenground, probably
            // Can't be that since it works the other way unless it somehow goes into the ground
            if (player.transform.localScale.x < sizeThresholdFollow)    // If it should attack
            {
                spiderBody.velocity = speedBase * spiderVelocity;
            }
            else                                                        // If it should run
            {
                spiderBody.velocity = -speedBase * spiderVelocity;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            spiderBody.velocity = new Vector2();
        }
    }

    // Collides with spider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player" && spiderBody.velocity.y < 0.01f)
        {
            if (collision.gameObject.transform.localScale.x < sizeThresholdFollow)      // Player dies
            {
                pause.EnableDeathScreen();
            }
            else if (collision.gameObject.transform.localScale.x > sizeThresholdRun)    // Spider dies
            {
                Destroy(gameObject);
            }
        }
    }
}

