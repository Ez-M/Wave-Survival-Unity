using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addSpawner : MonoBehaviour
{

    
    private SpawnManager spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.FindWithTag("GameManager").transform.Find("GameManager").gameObject.GetComponent<SpawnManager>();
        spawnManager.addSpawnerToList(this.gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
