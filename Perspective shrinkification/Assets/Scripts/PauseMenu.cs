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
    [SerializeField]
    AudioSource deathSound;
    [SerializeField]
    AudioSource music;

    // Private values
    bool pausedGame = false;        // Is the game paused? 

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.enabled = deathScreen.enabled = false;
        ChangePaused(!(PlayerPrefs.GetInt("FirstTime", 0) == 1));
    }

    // Update is called once per frame
    void Update()
    {
        // Checks if you clicked escape and pauses/unpauses the game
        if (Input.GetKeyDown(KeyCode.Escape))// && hasStoppedEscaping)
        {
            ChangePaused(!pausedGame);
        }

        if (!deathScreen.enabled) // If the death screen is not enabled
        {
            pauseMenu.enabled = pausedGame;
            playerBody.simulated = !pausedGame;
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("FirstTime");
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
        if (!pause)
        {
            if (!music.isPlaying)  // Only start song if it is not already being played
                music.Play();
        }
        else
            music.Pause();
    }

    public void EnableDeathScreen()
    {
        deathSound.Play();
        ChangePaused(true);
        deathScreen.enabled = true;
        playerBody.simulated = false;
    }

    public void loadScene(string scene)
    {
        PlayerPrefs.SetInt("FirstTime", 1);
        SceneManager.LoadScene(scene);
    }
}
