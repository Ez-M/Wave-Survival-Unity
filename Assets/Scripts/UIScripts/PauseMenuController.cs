using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private PlayerInputHandler player;
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
        player.disableInputs();

    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.visible=false;
        Cursor.lockState = CursorLockMode.Locked;
        player.enableInputs();
    }

    public void TogglePauseState(PlayerInputHandler playerIn)
    {
        if(gameIsPaused == false)
        {
            player = playerIn;
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
