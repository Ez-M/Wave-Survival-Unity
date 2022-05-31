using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BarricadeController : MonoBehaviour
{

    private int barHealth;    
    GameObject boardHolder;

    GameObject triggerArea;
    // Start is called before the first frame update
    void Start()
    {
        barHealth = 5;
       boardHolder = gameObject.transform.Find("boards_Holder").gameObject;
       triggerArea = gameObject.transform.Find("triggerArea").gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {   }

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

}
