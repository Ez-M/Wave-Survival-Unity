using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUIController : MonoBehaviour
{
    
    public GameObject activeWeapon; //The currently active equipped weapon
    public PlayerShoot playerShoot; //the PlayerSHoot for that weapon

    // Start is called before the first frame update

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

    private WeaponUIController WUIC;

    public InventoryController inventoryController;

    private TMPro.TextMeshProUGUI ScoreCard;
    private TMPro.TextMeshProUGUI AmmoCounter;
    private TMPro.TextMeshProUGUI CST; //centerscreentext

    private Transform gunEnd;   
    public GameObject gun, hipPosition, ADSPosition;

    private CharacterController characterController;
   
    #endregion


    void Start()
    {
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        public void weaponChanged(int X)
        {
            activeWeapon = inventoryController.weaponsGot[X];
            playerShoot = activeWeapon.GetComponentInChildren<PlayerShoot>();
            updateAmmo();
        }

        public void updateAmmo()
    {
        Debug.Log(inventoryController);
        Debug.Log(activeWeapon);
        Debug.Log(playerShoot);
        AmmoCounter.text=(playerShoot.curMag.ToString()+" / "+playerShoot.maxMag+"   "+playerShoot.curAmmo);
    }


    public void WeaponUIInit(PlayerManager initializer)
    {

        playerManager = initializer;
        playerCap = playerManager.playerCap;
        playerController = playerManager.playerController;
        leanPoint = playerManager.leanPoint;
        playerHead = playerManager.playerHead;
        playerCam = playerManager.playerCam;
        gunCam = playerManager.gunCam;

        characterController = playerCap.GetComponent<CharacterController>();

        inventoryController = initializer.gameObject.GetComponent<InventoryController>();


        WeaponUI = playerManager.WeaponUI;
        WUIC = playerManager.WUIC;
        ScoreCard = playerManager.ScoreCard;
        AmmoCounter = playerManager.AmmoCounter;
        CST = playerManager.CST;


        gun = gameObject;

        gunEnd = gun.transform.Find("gunEnd");


        ScoreCard = GameObject.Find("ScoreCard").GetComponent<TMPro.TextMeshProUGUI>(); 

        AmmoCounter = GameObject.Find("AmmoCounter").GetComponent<TMPro.TextMeshProUGUI>(); 


    }





}
