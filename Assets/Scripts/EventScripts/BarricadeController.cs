using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BarricadeController : MonoBehaviour
{

    private int barHealth;    
    private GameObject boardHolder;

    private GameObject triggerArea;

    private bool isAvailable;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        barHealth = 5;
        init();        
    }

    // Update is called once per frame
    void Update()
    { 

    }

    public void init()
    {
       gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
       boardHolder = gameObject.transform.Find("boards_Holder").gameObject;
       triggerArea = gameObject.transform.Find("triggerArea").gameObject;
       isAvailable = false;
    }

    public void updateBoards(bool check)
    {
        switch(check)
        {
            case true: 
            if (barHealth<5)
            {
            barHealth++;
            boardHolder.transform.GetChild(barHealth-1).gameObject.SetActive(true);
            }
            break;

            case false:
            if(barHealth>0)
            {
            boardHolder.transform.GetChild(barHealth-1).gameObject.SetActive(false);
            barHealth--;
            }
            break;
        }
    }


    public void repairBoard(InputAction.CallbackContext context)
    {
        updateBoards(true);
    }

        public void damageBoards(InputAction.CallbackContext context)
    {
        updateBoards(false);
    }

    public bool getIsAvailable()
    {
        return isAvailable;
    }

    public void setIsAvailable(bool to)
    {
        isAvailable = to;
    }

    public GameManager getGameManager()
    {
        return gameManager;
    }



}
