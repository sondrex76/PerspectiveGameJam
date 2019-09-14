using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Serialized variables
    [SerializeField]
    Canvas pauseMenu;
    [SerializeField]
    float minWaitTime;
    
    // Private values
    bool hasStoppedEscaping = true; // Bool to check if your escape click is a new one
    bool pausedGame = false;        // Is the game paused? 

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if you clicked escape and pauses/unpauses the game
        if (Input.GetKey(KeyCode.Escape) && hasStoppedEscaping)
        {
            pausedGame = !pausedGame;
            pauseMenu.enabled = pausedGame;
            hasStoppedEscaping = false;
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
            hasStoppedEscaping = true;              // Allows you to open/close the menu again
    }

    public bool returnPaused()
    {
        return pausedGame;
    }
}
