using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static scr_Models;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{

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


    // public LeanController leanController; 


    // public float movespeedS = 5f;moveSpeedF = 5f, moveSpeedB =1.0f,  moveSpeedL =1.0f, moveSpeedR = 1.0f,
    //set of floats that define the movement speed in each possible direction

    //run speed multiplier

    Vector3 moveDirection = Vector3.zero;

    float rotationX = 0;

       public bool canMove = true;
       public bool runInterrupt = false;
       public bool isRunning;

        


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




        public DefaultInput passInputs()
        {
            return defaultInput;
        }

       private Vector3 newCameraRotation;
       private Vector3 newCharacterRotation;


       [Header("References")]
       public Transform cameraHolder;

       [Header("Settings")]
       public PlayerSettingsModel playerSettings;
       public float viewClampYMin = -70;
       public float viewClampYMax = 80;
       public float  jumpSpeed= 3.0f;


        [Header("Gravity")]
        public float gravityAmount;
        public float gravityMin;
        public float playerGravity;

        public Vector3 jumpingForce;
        private Vector3 jumpingForceVelocity;

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




    private void Awake() 
    {
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

        Jump.performed += JumpFunction;

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
        // defaultInput.Inventory.OpenInventory += toggleInventoryisPressed;

        

        defaultInput.Enable();

        #endregion

        newCameraRotation = cameraHolder.localRotation.eulerAngles;
        newCharacterRotation = transform.localRotation.eulerAngles;

        characterController = GetComponent<CharacterController>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
        // leanController = GetComponent<LeanController>();

        // Lock Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible=false;    
        
    }

    // Update is called once per frame
    void Update()
    {
        //we are gounded, so recalculate move direction based on axes
        // Vector3 forward = transform.TransformDirection(Vector3.forward);
        // Vector3 right = transform.TransformDirection(Vector3.right);

        if (input_Sprint == true && runInterrupt == false)
        {
            isRunning = true;
        } else {isRunning = false;}

        

        // if (!characterController.isGrounded)
        // {
        //     moveDirection.y -= gravity * Time.deltaTime;
        // }

        // // //Move the controller
        // characterController.Move(moveDirection*Time.deltaTime);

        //Player and Camera rotation
        if(canMove)
        {
            CalculateView();    
            CalculateMovement();
            CalculateJumpChange();
        }

        // leanController.CalculateLeaning();

        // if(Input.GetKey(KeyCode.Q))
        // {
        //     leanController.isLL=true;
        // } else {leanController.isLL=false;}
 
        // if(Input.GetKey(KeyCode.E))
        // {
        //     leanController.isLR=true;
        // } else {leanController.isLR=false;}
        
    }

    //report variables for other scripts to use// 

       public bool isLook { 
        get {
        if(input_View != Vector2.zero)
        return true; 
        else
         return false;
    }
    }

       public bool isMoving {
            get {
        if(input_Movement != Vector2.zero)
        return true; 
        else
         return false;
    }
    }


        public void playerInit(PlayerManager initializer)
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
    }


    #region  -InputFunctions-
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
        pauseMenuController.TogglePauseState();

    }

    #endregion
    private void CalculateView()
    {
        newCameraRotation.x += playerSettings.ViewYSensitivity * (playerSettings.ViewYInverted ? input_View.y : -input_View.y) * Time.deltaTime; // swapping axis mid line because we are referring firs to unity axis and then input axis which are flipped
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);

        newCharacterRotation.y += playerSettings.ViewXSensitivity * (playerSettings.ViewXInverted ? -input_View.x : input_View.x) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(newCharacterRotation);


        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
    }

    private void CalculateMovement()
    {
        var runF = playerSettings.moveSpeedF*playerSettings.runMulti;
        var runS = playerSettings.moveSpeedS*playerSettings.runMulti;    

        
        float forwardSpeed = (isRunning == true ? runF : playerSettings.moveSpeedF) * input_Movement.y * Time.deltaTime;
        float horizontalSpeed = (isRunning == true ? runS: playerSettings.moveSpeedS) * input_Movement.x * Time.deltaTime;

        Vector3 newMovementSpeed = new Vector3(horizontalSpeed, 0, forwardSpeed);


        newMovementSpeed = transform.TransformDirection(newMovementSpeed);


        if(playerGravity > gravityMin)
        {
            playerGravity -= gravityAmount*Time.deltaTime;
        }
        else
        {

        }
            

        if (playerGravity < -0.1 && characterController.isGrounded)
        {
                playerGravity = -0.1f;
        }

        newMovementSpeed.y += playerGravity;
        newMovementSpeed += jumpingForce*Time.deltaTime;

        characterController.Move(newMovementSpeed);
    }


    private void CalculateJumpChange()
    {

        jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity, playerSettings.JumpingFalloff);

    }
    private void JumpFunction(InputAction.CallbackContext value)
    {
        if(characterController.isGrounded)
        {
            //Jump
        jumpingForce = Vector3.up*playerSettings.JumpingHeight;
        playerGravity = 0;

        }

        

    }






}
