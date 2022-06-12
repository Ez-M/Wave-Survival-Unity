using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarricadeAutoAdd : MonoBehaviour
{

    private BarricadeController barricadeController;
    private GameManager gameManager;
    private EntryManager entryManager;    
    private bool isDone;

    [SerializeField]
    private bool startingEntry;
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
        barricadeController = this.GetComponent<BarricadeController>();

        gameManager = barricadeController.getGameManager();
        Debug.Log(gameManager);
        entryManager = gameManager.GetComponent<EntryManager>();
        isDone = false;

        if(startingEntry == true)
        {
            addBarricade();
        }

    }

    public void addBarricade()
    {
        if(isDone == false)
        {
            entryManager.availableEntries.Add(this.gameObject);
            isDone = true;        }
        
    }


}
