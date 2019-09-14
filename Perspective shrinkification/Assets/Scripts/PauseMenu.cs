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
    [SerializeField]
    Rigidbody2D playerBody;

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
        if (Input.GetKeyDown(KeyCode.Escape))// && hasStoppedEscaping)
        {
            pausedGame = !pausedGame;
            hasStoppedEscaping = false;
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
            hasStoppedEscaping = true;              // Allows you to open/close the menu again
        
        pauseMenu.enabled = pausedGame;
        playerBody.simulated = !pausedGame;
    }

    public bool returnPaused()
    {
        return pausedGame;
    }
    public void QuitGame()
    {
        Application.Quit(); // Quits application
    }

    // Unpauses the game
    public void ChangePaused(bool pause)
    {
        pausedGame = pause;
    }
}
