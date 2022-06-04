using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    private int waveCount;
    // private int currentWave;
    private int waveState;//1 = starting, 2 = ongoing, 3 = ending
    private int zedDead;
    private int zedAlive;
    private int zedToSpawn; //number of zed remaining to spawn this wave
    private int zedAwait;   //time since last spawn
    private int zedLimit; // zombie spawn limiter
    private int gameDiff;   //1, 2, 3 for holding game difficulty. Will currently only have a possible value of 1 for testing purposes.
    

//   private int specialDead;
//   private int specialAlive;
//   private int specialToSpawn;
//   private int specialAwait;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init()
    {
        waveCount = 0;
        zedLimit = 50;
        zedDead = 0;
        zedAlive = 0;
        zedToSpawn = 0;
        startWave();
    }

    public void startWave()
    {

    }

    public int getWaveCount()
    {
        return waveCount;
    }
  /* 






    calculateSpecial()
    //the most quick and dirty way I can think of doing this is just manually pre-constructing
    //  roll tables manually and selecting which one to use based on difficulty and currentWave value. 
    {
        switch(gameDiff)
        case 1 {
            switch(currentWave)
            case <5: just chest armor //but special to spawn will be 0 for the first 2 waves I think
            break;
            case <8: a few head armored are thrown in
            break
            case <=10
        }
    }


   
    */





}
