using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public bool gameIsPaused;

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
        Cursor.visible=true;
        Cursor.lockState = CursorLockMode.None;

    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.visible=false;
        Cursor.lockState = CursorLockMode.Locked;
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
