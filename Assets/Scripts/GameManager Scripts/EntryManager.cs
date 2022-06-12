using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static EntryManager instance = null;
    public  List<GameObject> availableEntries = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
       {
        instance = this;
       }
       else if (instance != this)
       Destroy(gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
