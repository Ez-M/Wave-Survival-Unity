using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{

    public  List<GameObject> weaponsGot;
    public List<GameObject> weapons;
    public GameObject startWeapon; //weapon the player will start with

    public GameObject testWeapon; //currently only used for testing, plug and play for later when things are purchased

    // private GameObject tempWeapon; //variable for storing what weapon has been created during the weaponGot process



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
    [SerializeField] GameObject activeWeapon;

    private WeaponUIController WUIC;

    public InventoryController inventoryController;

    private TMPro.TextMeshProUGUI ScoreCard;
    private TMPro.TextMeshProUGUI AmmoCounter;
    private TMPro.TextMeshProUGUI CST; //centerscreentext

    // private Transform gunEnd;   
    // public GameObject gun, hipPosition, ADSPosition;
    

    private CharacterController characterController;
   
    #endregion

    public int currentWeaponInt;
    public bool isDone;


        #region -input bools-
    private DefaultInput defaultInput;
    public bool input_A1;
    public bool input_A2;
    public bool input_tempA;
        #endregion

    private bool weaponCheck;
    private PlayerShoot activeWeaponScript;

    private void Awake() 
     {

     }



    // Start is called before the first frame update
    void Start()
    {       


        
    }

    // Update is called once per frame
    void Update()
    {


//Inputs allowing for directly switching between weapons in a given slot
        // if (input_A1 && currentWeapon!=1)
        // {
        //     currentWeapon = 1;
        //     changeWeapon(0); //1 because list indexes from 0 - see changeWeapon
        // }

        // if (input_A2 && currentWeapon!=2 && weaponsGot.Count >= 2)
        // {
        //     currentWeapon = 2;
        //     changeWeapon(1); //1 because lst indexes from 0 - see changeWeapon
        // }


    }

    public void newWeaponGot(GameObject weaponIn)
    {//checks if the stated object is in weaponsGot, and if not it adds an instance of it
        
        if (weaponIn)  //confirm we have a valid weaponIn
        {



        //weaponCheck needs to return false to permit the weapon being added to the player
        // weaponCheck = false;
        // for(var i=0; i<weaponsGot.Count; i++)
        // {
        //     Debug.Log(weaponsGot[i].name);
        //     if (weaponsGot[i].name == weaponIn.name+"(Clone)")
        //     {
        //         Debug.Log(weaponsGot[i].name);
        //          weaponCheck = true ;
        //     }
        // }



            if (weaponsGot.Count < 2 ) // set this value to whatever your max inventory size is
            {
                GameObject tempWeapon;
                
                tempWeapon = Instantiate(weaponIn, gunCam.transform) as GameObject; //variable to track new item
           
                weaponsGot.Add(tempWeapon); //add the new weapon to our inventory list
                // tempWeapon.transform.GetChild(0).GetChild(1).GetComponent<PlayerShoot>().gunInit(); //run guninit
                
                changeWeapon(weaponsGot.IndexOf(tempWeapon));
                tempWeapon.transform.GetChild(0).GetChild(1).BroadcastMessage("gunInit"); //run guninit

            } else 
            {  // if (weaponCheck == false) //if weapon check is false, perform newWeaponGot
                 
            weaponLost(currentWeaponInt);


            GameObject tempWeapon;//variable to track new item
            tempWeapon = Instantiate(weaponIn, gunCam.transform) as GameObject; //create new weapon
           
            weaponsGot[currentWeaponInt] = tempWeapon; //add the new weapon to our inventory list
            // tempWeapon.transform.GetChild(0).GetChild(1).GetComponent<PlayerShoot>().gunInit(); //run guninit
            
            changeWeapon(currentWeaponInt);
            tempWeapon.transform.GetChild(0).GetChild(1).BroadcastMessage("gunInit"); //run guninit
            
             }
        
             


        } else {Debug.LogError("Tried Adding Invalid Weapon");}
 



    }
    public void weaponLost(int drop)
    {
        GameObject.Destroy(weaponsGot[drop]);    
    }

    public void changeWeapon( int weaponTo)
    {

        if (weaponsGot.Count>weaponTo){
        if (   weaponsGot[weaponTo].gameObject == true)
        {
            Debug.Log("Weapon change check" + weaponTo);
            if(activeWeapon == true)
            {
                activeWeaponScript.isReloading = false;
                activeWeapon.SetActive(false); //set activeweapon to inactive before switching
            }
            
            activeWeapon = weaponsGot[weaponTo]; //switch the active weapon
            activeWeapon.SetActive(true); //set the new active weapon to true
            activeWeaponScript = activeWeapon.transform.GetChild(0).GetComponentInChildren<PlayerShoot>();
            playerManager.setActiveWeapon(activeWeapon); //Sets the global active weapon
            WUIC.weaponChanged(weaponTo); //update relavant UI element
            currentWeaponInt = weaponTo;

        } else
        {
            Debug.LogError("Tried to change weapon to index that is Null");
        }
        } else {Debug.LogError("Tried to change weapon to index that does not exist");}
        
        
    }


    public void InventoryInit(PlayerManager initializer)
    {
        playerManager = initializer;
        playerCap = playerManager.playerCap;
        playerController = playerManager.playerController;
        leanPoint = playerManager.leanPoint;
        playerHead = playerManager.playerHead;
        playerCam = playerManager.playerCam;
        gunCam = playerManager.gunCam;   

        inventoryController = gameObject.GetComponent<InventoryController>();



        WeaponUI = playerManager.WeaponUI;
        WUIC = playerManager.WUIC;
        ScoreCard = playerManager.ScoreCard;
        AmmoCounter = playerManager.AmmoCounter;
        CST = playerManager.CST;

        weapons = WeaponManager.instance.allWeapons;     

        weaponsGot = new List<GameObject>(); 
    }

    public void postInit()
    {

                

        //sets all weapons in master weapons list to inactive
        for (var i=0; i<weapons.Count; i++)
        {
            weapons[i].SetActive(false);
        }

        //iniatiates starting weapon conditions        
        
        newWeaponGot(startWeapon);
        changeWeapon(0);

    }

    #region -InputFunctions-
    private void Alpha1isPressed()
    {
        input_A1 = true;
    }

    private void Alpha1isReleased()
    {
        input_A1 = false;
    }

    private void Alpha2isPressed()
    {
        input_A2 = true;
    }


    private void Alpha2isReleased()
    {
        input_A2 = false;

    }


    private void TempAction1isPressed()
    {
        input_tempA = true;
    }


    private void TempAction1isReleased()
    {
        input_tempA = false;
    }

    #endregion

}
