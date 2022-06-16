using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BarricadeActions : MonoBehaviour
{

    BarricadeController barCon;
    // Start is called before the first frame update
    void Start()
    {
        barCon = gameObject.transform.root.GetComponent<BarricadeController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider enterer)
    {
        Debug.Log(enterer);
        if(enterer.tag == "Player")
        {
            collectPlayer(enterer);
        }

        if(enterer.gameObject.transform.parent.tag == "Zombie")
        {
            Debug.Log("Zombie detected in entry");
           GameObject zed = enterer.gameObject.transform.parent.gameObject;
           if(zed.GetComponent<ZombAI>().getIsInside() == false)
           {
           zed.GetComponent<ZombAI>().setTargetEntry(gameObject);
           zed.GetComponent<ZombAI>().setAtWindow(true);
           }

        }

    }

        void OnTriggerExit (Collider Exiter)
    {
        if (Exiter.tag == "Player") 
        {
            discardPlayer(Exiter);

        }
    }

    private void collectPlayer(Collider enterer)
    {
        DefaultInput defaultInput;
            // currentPlayer = enterer.transform.root.GetComponent<PlayerManager>();
            // playerController = currentPlayer.GetComponentInChildren<PlayerController>();
            defaultInput = enterer.GetComponentInChildren<PlayerController>().passInputs();

            defaultInput.Character.Interact.performed += barCon.repairBoard;
            defaultInput.Inventory.TempAction1.performed +=barCon.damageBoards;

    }


        private  void discardPlayer(Collider exiter)
    {       
        DefaultInput defaultInput;
        defaultInput = exiter.GetComponentInChildren<PlayerController>().passInputs();
        defaultInput.Character.Interact.performed -= barCon.repairBoard;
        defaultInput.Inventory.TempAction1.performed -=barCon.damageBoards;


    }
}
