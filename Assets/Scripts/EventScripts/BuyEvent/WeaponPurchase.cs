using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPurchase : MonoBehaviour
{

    public GameObject saleItem;

    public GameObject saleModel; 

    public float pointsCost; 

    public string interactPrompt;

    public bool hasPlayer; 


    public TMPro.TextMeshProUGUI CST;

    private PlayerManager currentPlayer;
    private PlayerInputHandler playerInputHandler;
    private InventoryController inventoryController;

    private DefaultInput defaultInput;

    
    


    void OnTriggerEnter (Collider enterer)
    {
        if (enterer.tag == "Player") 
        {

            hasPlayer = true;
            CST.text = interactPrompt;

            collectPlayer(enterer);


        }
    }

    void OnTriggerExit (Collider Exiter)
    {
        if (Exiter.tag == "Player") 
        {
            hasPlayer = false;
            CST.text = "";

            discardPlayer(Exiter);

        }
    }

    // Update is called once per frame
    void Update()
    {


        

        
    }







    private void collectPlayer(Collider enterer)
    {

            currentPlayer = enterer.transform.root.GetComponent<PlayerManager>();
            playerInputHandler = currentPlayer.GetComponent<PlayerInputHandler>();
            inventoryController = currentPlayer.inventoryController;

            defaultInput = playerInputHandler.passInputs();

            defaultInput.Character.Interact.performed += buyFunction;
    }


    private  void discardPlayer(Collider exiter)
    {       defaultInput.Character.Interact.performed -= buyFunction;    

            defaultInput = null;
            currentPlayer = null;
            playerInputHandler = null;
            inventoryController = null;

    }


    public void buyFunction(InputAction.CallbackContext context)
    {
        Debug.Log("Input Test");    
        inventoryController.newWeaponGot(saleItem);
    }

    public void OnInteractPressed (InputAction.CallbackContext value)
    {

    }



}
