using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{

    #region -INPUTS IN-


    private DefaultInput defaultInput;
    public Vector2 input_Movement;
    public Vector2 input_View;
    public bool input_Jump;
    public bool input_Sprint;
    public bool input_Fire1;
    public bool input_Fire2;
    public bool input_Reload;
    public bool input_Interact;
    public bool input_A1;
    public bool input_A2;
    public bool input_tempA;
    public bool input_pause;
    public bool input_inventory;
    #endregion



    #region -InputActions-
    public InputAction Jump;
    public InputAction Sprint;
    public InputAction Interact;
    public InputAction Fire1;
    public InputAction Fire2;
    public InputAction Reload;
    public InputAction Weapon1;
    public InputAction Weapon2;
    public InputAction TempAction1;
    public InputAction togglePause;
    public InputAction toggleInventory;
    #endregion

    #region -Init Connections-
    private bool gunisInit;
    private PlayerManager playerManager;
    private GameObject playerCap;
    private GameObject leanPoint;
    private GameObject playerHead;
    private Camera playerCam;
    private Camera gunCam;
    private PlayerController playerController; 
    private GameObject WeaponUI;
    private GameObject activeWeapon;
    private WeaponUIController WUIC;
    public InventoryController inventoryController;
    private TMPro.TextMeshProUGUI ScoreCard;
    private TMPro.TextMeshProUGUI AmmoCounter;
    private TMPro.TextMeshProUGUI CST; //centerscreentext
    private PauseMenuController pauseMenuController;

    // private Transform gunEnd;   
    // public GameObject gun, hipPosition, ADSPosition;
    

    private CharacterController characterController;
   
    #endregion



    public void inputInit(PlayerManager initializer) 
    {
        playerManager = initializer;
        playerCap = playerManager.playerCap;
        playerController = playerManager.playerController;
        leanPoint = playerManager.leanPoint;
        playerHead = playerManager.playerHead;
        playerCam = playerManager.playerCam;
        gunCam = playerManager.gunCam;   

        inventoryController = playerManager.GetComponent<InventoryController>();
        pauseMenuController = playerManager.pauseMenuController;


        WeaponUI = playerManager.WeaponUI;
        WUIC = playerManager.WUIC;
        ScoreCard = playerManager.ScoreCard;
        AmmoCounter = playerManager.AmmoCounter;
        CST = playerManager.CST;


        defaultInput = new DefaultInput();

        #region -INPUTS SET-



        Jump = defaultInput.Character.Jump;
        Sprint = defaultInput.Character.Sprint;
        Interact = defaultInput.Character.Interact;
        Fire1 = defaultInput.Weapon.Fire1;
        Fire2 = defaultInput.Weapon.Fire2;
        Reload = defaultInput.Weapon.Reload;
        Weapon1 = defaultInput.Inventory.Weapon1;
        Weapon2 = defaultInput.Inventory.Weapon2;
        TempAction1 = defaultInput.Inventory.TempAction1;
        toggleInventory = defaultInput.Inventory.OpenInventory;
        togglePause = defaultInput.UI.Pause;

        defaultInput.Character.Movement.performed += e => input_Movement = e.ReadValue<Vector2>();
        defaultInput.Character.View.performed += e =>  input_View =  e.ReadValue<Vector2>();

        Jump.performed += jumpisPressed;
        Jump.canceled += jumpisReleased;

        Sprint.performed += sprintisPressed;
        Sprint.canceled += SprintisReleased;

        Interact.performed += InteractisPressed;
        Interact.canceled += InteractisReleased;
        Fire1.performed += Fire1isPressed;
        Fire1.canceled += Fire1isReleased;
        Fire2.performed += Fire2isPressed;
        Fire2.canceled += Fire2isReleased;
        Reload.performed += ReloadisPressed;
        Reload.canceled += ReloadisReleased;

        defaultInput.Inventory.Weapon1.performed += Alpha1isPressed;
        defaultInput.Inventory.Weapon2.performed += Alpha2isPressed;
        defaultInput.Inventory.TempAction1.performed += TempAction1isPressed;

        defaultInput.UI.Pause.performed += togglePauseisPressed;
        defaultInput.Inventory.OpenInventory.performed += toggleInventoryisPressed;

        

        defaultInput.Enable();

        #endregion

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void disableInputs(){
        defaultInput.Disable();
    }
    public void enableInputs(){
        defaultInput.Enable();
    }

    public void pauseInputs(){
        Jump.Disable();
        Sprint.Disable();
        Interact.Disable();
        Fire1.Disable();
        Fire2.Disable();
        Reload.Disable();
        Weapon1.Disable();
        Weapon2.Disable();
        TempAction1.Disable();
        // toggleInventory.Disable();
        // togglePause.Disable();
    }
    public void unPauseInputs(){
        Jump.Enable();
        Sprint.Enable();
        Interact.Enable();
        Fire1.Enable();
        Fire2.Enable();
        Reload.Enable();
        Weapon1.Enable();
        Weapon2.Enable();
        TempAction1.Enable();
        // toggleInventory.Disable();
        // togglePause.Disable();
    }

    public DefaultInput passInputs()
    {
        return defaultInput;
    }

    #region  -INPUT FUNCTIONS-
    private void jumpisPressed(InputAction.CallbackContext value)
    {
        input_Jump = true;
    }

        private void jumpisReleased(InputAction.CallbackContext value)
    {
        input_Jump = false;
    }

    private void sprintisPressed(InputAction.CallbackContext value)
    {
        input_Sprint = true;
    }

    private void SprintisReleased(InputAction.CallbackContext value)
    {
        input_Sprint = false;
    }

    private void ReloadisPressed(InputAction.CallbackContext value)
    {
        input_Reload = true;

    }

    private void ReloadisReleased(InputAction.CallbackContext value)
    {
        input_Reload = false;

    }


    public void Fire1isPressed(InputAction.CallbackContext value)
    {
        input_Fire1 = true;  
        
    }

    private void Fire1isReleased(InputAction.CallbackContext value)
    {
        input_Fire1 = false;
        playerManager.activeShoot.hasFired = false;
    }


    private void Fire2isPressed(InputAction.CallbackContext value)
    {
        input_Fire2 = true;
    }

    private void Fire2isReleased(InputAction.CallbackContext value)
    {   
        input_Fire2 = false;
    }

    public void InteractisPressed(InputAction.CallbackContext value)
    {
        input_Interact = true;
    }

    private void InteractisReleased(InputAction.CallbackContext value)
    {
        input_Interact = false;
    }

     private void Alpha1isPressed(InputAction.CallbackContext value)
    {
        input_A1 = true;
        inventoryController.changeWeapon(0); //1 because list indexes from 0 - see changeWeapon
    }

    private void Alpha1isReleased(InputAction.CallbackContext value)
    {
        input_A1 = false;
    }

    private void Alpha2isPressed(InputAction.CallbackContext value)
    {
        input_A2 = true;
        inventoryController.changeWeapon(1); //1 because lst indexes from 0 - see changeWeapon
    }


    private void Alpha2isReleased(InputAction.CallbackContext value)
    {
        input_A2 = false;

    }


    private void TempAction1isPressed(InputAction.CallbackContext value)
    {
        input_tempA = true;
        inventoryController.isDone = true;
            
           inventoryController.newWeaponGot(inventoryController.testWeapon);    
    }


    private void TempAction1isReleased(InputAction.CallbackContext value)
    {
        input_tempA = false;
    }

    private void togglePauseisPressed(InputAction.CallbackContext value)
    {
        pauseMenuController.TogglePauseState(this);

    }
    private void toggleInventoryisPressed(InputAction.CallbackContext value)
    {
        pauseMenuController.TogglePauseState(this);

    }

    #endregion


}
