using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField]
    Transform player;                   // The player's field, to know the player's current size
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
    
    // Update is called once per frame
    void Update()
    {
        
    }
    // Why does this not fricking trigger when the player enters? I am so confused
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector2 speedBase = (((Vector2)player.transform.position - (Vector2)transform.position).normalized);

            speedBase.y = 0;
            speedBase = speedBase.normalized;

            if (player.transform.localScale.x < sizeThresholdFollow)    // If it should attack
            {
                spiderBody.velocity = speedBase * spiderVelocity;
            }
            else if (player.transform.localScale.x > sizeThresholdRun)  // If it should run
            {
                spiderBody.velocity = -speedBase * spiderVelocity;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            spiderBody.velocity = new Vector2();
        }
    }
}

