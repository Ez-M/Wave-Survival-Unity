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
    private CharacterController characterController;
    private PlayerInputHandler playerInputHandler;
   
    #endregion


    Vector3 moveDirection = Vector3.zero;

    float rotationX = 0;

       public bool canMove = true;
       public bool runInterrupt = false;
       public bool isRunning;


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

        // public InputAction Jump;
        // public InputAction Sprint;
        // public InputAction Interact;
        // public InputAction Fire1;
        // public InputAction Fire2;
        // public InputAction Reload;
        // public InputAction Weapon1;
        // public InputAction Weapon2;
        // public InputAction TempAction1;
        // public InputAction togglePause;
        // public InputAction toggleInventory;




    private void Awake() 
    {

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

        if (playerInputHandler.input_Sprint == true && runInterrupt == false)
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
            if(PauseMenuController.gameIsPaused == false)
            {
            CalculateJumpChange();
            }
            CalculateMovement();
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
        if(playerInputHandler.input_View != Vector2.zero)
        return true; 
        else
         return false;
    }
    }

       public bool isMoving {
            get {
        if(playerInputHandler.input_Movement != Vector2.zero)
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
        playerInputHandler = playerManager.playerInputHandler;



        WeaponUI = playerManager.WeaponUI;
        WUIC = playerManager.WUIC;
        ScoreCard = playerManager.ScoreCard;
        AmmoCounter = playerManager.AmmoCounter;
        CST = playerManager.CST;

        playerInputHandler.Jump.performed += JumpFunction;
    }


    
    private void CalculateView()
    {
        newCameraRotation.x += playerSettings.ViewYSensitivity * (playerSettings.ViewYInverted ? playerInputHandler.input_View.y : -playerInputHandler.input_View.y) * Time.deltaTime; // swapping axis mid line because we are referring firs to unity axis and then input axis which are flipped
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);

        newCharacterRotation.y += playerSettings.ViewXSensitivity * (playerSettings.ViewXInverted ? -playerInputHandler.input_View.x : playerInputHandler.input_View.x) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(newCharacterRotation);


        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
    }

    private void CalculateMovement()
    {
        var runF = playerSettings.moveSpeedF*playerSettings.runMulti;
        var runS = playerSettings.moveSpeedS*playerSettings.runMulti;    

        
        float forwardSpeed = (isRunning == true ? runF : playerSettings.moveSpeedF) * playerInputHandler.input_Movement.y * Time.deltaTime;
        float horizontalSpeed = (isRunning == true ? runS: playerSettings.moveSpeedS) * playerInputHandler.input_Movement.x * Time.deltaTime;

        Vector3 newMovementSpeed = new Vector3(horizontalSpeed, 0, forwardSpeed);


        newMovementSpeed = transform.TransformDirection(newMovementSpeed);

        if(playerGravity > gravityMin)
        {
            playerGravity -= gravityAmount*Time.deltaTime;
        }
            

        if (playerGravity < -0.1 && characterController.isGrounded)
        {
                playerGravity = -0.1f;
        }   
        if(PauseMenuController.gameIsPaused == false){
        newMovementSpeed.y += playerGravity;    }
        newMovementSpeed += jumpingForce*Time.deltaTime;

        characterController.Move(newMovementSpeed);
    }


    private void CalculateJumpChange()
    {

        jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity, playerSettings.JumpingFalloff);
        if(jumpingForce.y <= 0.01f)
        {jumpingForce.y = 0f;}

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
