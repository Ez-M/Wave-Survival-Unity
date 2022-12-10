using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public static bool gameIsPaused;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;

    }

    public void TogglePauseState()
    {
        if(gameIsPaused == false)
        {
            Pause();
        }        
        else 
        {
            Resume();
        }

    }

    public void ResetGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("Restart game does not currently work");
    }
}
