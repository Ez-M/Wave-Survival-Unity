using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int waveCount;
    private int currentWave;
    private int zedLimit; // zombie spawn limiter
    private int gameDiff;   //1, 2, 3 for holding game difficulty. Will currently only have a possible value of 1 for testing purposes.
    private WaveManager waveManager;
    private SpawnManager spawnManager;

    void awake()
    {
        zedLimit = 50;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        init();
        startGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void init()
    {
        Debug.Log("GameManager init start");
        waveManager = this.gameObject.GetComponent<WaveManager>();
        spawnManager = this.gameObject.GetComponent<SpawnManager>();
        Debug.Log("GameManager init end");
    }

    private void startGame()
    { 

        Debug.Log("GameManager startGame");
        waveManager.init();
        // spawnManager.init();
        waveCount = waveManager.getWaveCount(); 
        

    }

    // start game> set waveCount = 0 // startWave(){waveCount++; waveState = 2} update{}




}
