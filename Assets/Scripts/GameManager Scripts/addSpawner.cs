using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class addSpawner : MonoBehaviour
{

    
    private WaveManager waveManager;
    // Start is called before the first frame update
    void Start()
    {
        waveManager = GameObject.FindWithTag("GameManager").GetComponent<WaveManager>(); // .transform.Find("GameManager")
        waveManager.addSpawnerToList(this.gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
