using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    private int waveCount;
    // private int currentWave;
    private int waveState;//1 = starting, 2 = ongoing, 3 = ending
    private int zedDead;
    private int totalDead;
    private int zedAlive;
    private int zedToSpawn; //number of zed remaining to spawn this wave
    // private int zedAwait;   //time since last spawn
    private int zedLimit; // zombie spawn limiter
    private int gameDiff;   //1, 2, 3 for holding game difficulty. Will currently only have a possible value of 1 for testing purposes.
    
    private SpawnManager spawnManager;
    private GameManager gameManager;

    private AudioSource noiseMaker;

    [SerializeField]
    private GameObject zombie_basic;
    [SerializeField]
    private GameObject zombieCloneContainer;

    private List<GameObject> allZombieTypes = new List<GameObject>();

    private  List<GameObject> allSpawners = new List<GameObject>();
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
        gameManager = this.gameObject.GetComponent<GameManager>();        
        spawnManager = this.gameObject.GetComponent<SpawnManager>();
        noiseMaker = gameManager.gameObject.GetComponent<AudioSource>();
        allZombieTypes.Add(zombie_basic);
        waveCount = 0;
        zedLimit = 50;
        zedDead = 0;
        zedAlive = 0;
        zedToSpawn = 0;

        startWave();
    }

    public void startWave()
    {
        waveCount++;
        Debug.Log("Wave " +waveCount+" starting");
        initWaveSpawns();
        StartCoroutine(SpawnZeds());

    }

    public int getWaveCount()
    {
        return waveCount;
    }




    public void addSpawnerToList(GameObject spawner)
    {
        allSpawners.Add(spawner);
    }
    
    public void initWaveSpawns()
    {
        Debug.Log("initWave called");
        zedToSpawn = waveCount*5;
        // zedLimit = 50;
        zedAlive = 0;
        zedDead = 0;
        waveState = 2;

    }


    public void hasDied()
    {
        zedAlive--;
        zedDead++;
        totalDead++;
    }

    public void stopSpawnRoutine()
    {
        Debug.Log("SpawnZeds Stopped");
        StartCoroutine(CheckForEndOfRound());
        StopCoroutine("SpawnZeds");

    }

    public void stopCheckForEndOfRoundRoutine()
    {
        Debug.Log("CheckForEndOfRound stopped");        
        StopCoroutine("CheckForEndOfRound");
        StartCoroutine(RoundTransition()); //When there are no longer zedtospawn or zed alive, begin round transition
    }

    public void stopRoundTransitionRoutine()
    {
        Debug.Log("RoundTransition stopped");
        StopCoroutine("RoundTransition");
        startWave();
    }

    IEnumerator  SpawnZeds()
    {

        while(zedToSpawn > 0)
        {
            // Debug.Log("SpawnZeds Called");
            GameObject spawnAt;
            GameObject currentSpawn;
            GameObject tempZombie;
            if (zedAlive < zedLimit && zedToSpawn > 0)
             {
                Debug.Log("DEBUG FOR SPAWNZEDS");
                Debug.Log(allSpawners.Count);
                spawnAt = allSpawners[Random.Range(0, allSpawners.Count)];  //spawns tuff
                currentSpawn = allZombieTypes[Random.Range(0, allZombieTypes.Count)];
                tempZombie = Instantiate(currentSpawn,spawnAt.transform.position,  Quaternion.identity, zombieCloneContainer.transform);
                zedAlive++;
                zedToSpawn--;
                } 
                
            //     else  //when there are no more zed to spawn for this round, stop the spawn coroutine, and start the checkforendofround coroutine. 
            //     {                  
                  
            //  }

            

        yield return new WaitForSeconds(.1f);
        }

        if(zedToSpawn<1)
            {
                stopSpawnRoutine();
            }

     }



     IEnumerator CheckForEndOfRound()
     {
         Debug.Log("CheckforendofRound initiated");
         bool varTemp = true;
         while(varTemp)
         {
             
             if(zedAlive == 0)
             {
                 varTemp = false;                 
                 stopCheckForEndOfRoundRoutine();
             }
             yield return new WaitForSeconds(1f);
         }
     }

     IEnumerator RoundTransition()
     {
         Debug.Log("RoundTransition init");
         noiseMaker.Play();//play sound
         yield return new WaitForSeconds(3f);//yield for 3 seconds
         //transition round effect
        stopRoundTransitionRoutine();
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
