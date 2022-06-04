using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    private  List<GameObject> allSpawners = new List<GameObject>();
    private List<GameObject> activeSpawners = new List<GameObject>();


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

    }


    public void addSpawnerToList(GameObject spawner)
    {
        allSpawners.Add(spawner);
    }
    // if (zedAwait >0 && zedAlive < 50)
    // {   
    //     spawnZed()

    // }

    // spawnZed()
    // {
    //     if(specialAwait <= 0 && specialToSpawn > 0)      //first we'll need to determine if the current zed will be special or not
    //     {
    //         calculateSpecial();
    //     }
    // }



}
