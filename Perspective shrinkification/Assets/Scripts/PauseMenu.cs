using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Serialized variables
    [SerializeField]
    Canvas pauseMenu;
    [SerializeField]
    Canvas deathScreen;
    [SerializeField]
    float minWaitTime;
    [SerializeField]
    Rigidbody2D playerBody;

    // Private values
    bool pausedGame = false;        // Is the game paused? 

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.enabled = deathScreen.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if you clicked escape and pauses/unpauses the game
        if (Input.GetKeyDown(KeyCode.Escape))// && hasStoppedEscaping)
        {
            pausedGame = !pausedGame;
        }
        
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

    public void loadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
