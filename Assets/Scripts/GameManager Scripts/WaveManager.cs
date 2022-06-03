using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    private int waveCount;
    private int currentWave;
    private int waveState;//1 = starting, 2 = ongoing, 3 = ending
    private int zedDead;
    private int zedAlive;
    private int zedToSpawn; //number of zed remaining to spawn this wave
    private int zedAwait;   //time since last spawn
    private int zedLimit; // zombie spawn limiter
    private int gameDiff;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  /* 


  private list spawners<spawners>
  private int specialDead
  private specialAlive
  private specialToSpawn
  private specialAwait
    if (zedAwait >0 && zedAlive < 50)
    {   
        spawnZed()

    }

    spawnZed()
    {
        if(calculateSpecial() == true)      //first we'll need to determine if the current zed will be special or not
        {}
    }



    calculateSpecial()
    {
        if (specialAwait <= 0 && specialToSpawn > 0)
        {
            
        }
    }
    */





}
