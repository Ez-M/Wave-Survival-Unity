using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{


/// NOTE: my intention with the PlayerManager is to act as a central
//Control point for the player character, and to avoid
// making the same references and declarations across many scripts
    public GameObject playerCap;
    public GameObject leanPoint;
    public GameObject playerHead;
    public Camera playerCam;
    public Camera gunCam;
    public PlayerController playerController; 
    private CharacterController characterController;
    public GameObject WeaponUI;
    public GameObject activeWeapon;
    public PlayerShoot activeShoot;

    public WeaponUIController WUIC;
    public InventoryController inventoryController;
    public TMPro.TextMeshProUGUI ScoreCard;
    public TMPro.TextMeshProUGUI AmmoCounter;
    public TMPro.TextMeshProUGUI CST; //centerscreentext
    
    public float score;
    private float playerHP;

    // [SerializeField] GameObject pauseMenuController;
    [SerializeField] GameObject UIParent;
    public PauseMenuController pauseMenuController;


    // Start is called before the first frame update
    void Start()
    {
        score = 0f;
        init();

        inventoryController.InventoryInit(this);        
        WUIC.WeaponUIInit(this);
        playerController.playerInit(this);
        
        inventoryController.postInit();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void init()
    {
        playerCap = gameObject.transform.GetChild(0).gameObject;
        playerController = playerCap.GetComponent<PlayerController>();
        leanPoint = playerCap.transform.GetChild(0).gameObject;
        playerHead = leanPoint.transform.GetChild(0).gameObject;
        playerCam = playerHead.transform.GetChild(0).gameObject.GetComponent<Camera>();
        gunCam = playerCam.transform.GetChild(0).gameObject.GetComponent<Camera>();
        playerHP = 10;

        characterController = playerCap.GetComponent<CharacterController>();

        inventoryController = gameObject.GetComponent<InventoryController>();
        pauseMenuController = gameObject.GetComponent<PauseMenuController>();
       
        WeaponUI = UIParent.transform.Find("weaponUI").gameObject;
        WUIC = WeaponUI.GetComponent<WeaponUIController>();
        ScoreCard = WeaponUI.transform.Find("ScoreCard").GetComponent<TMPro.TextMeshProUGUI>(); 
        AmmoCounter = WeaponUI.transform.Find("AmmoCounter").GetComponent<TMPro.TextMeshProUGUI>(); 
        CST = UIParent.transform.Find("CenterScreenText").GetComponent<TMPro.TextMeshProUGUI>();

    }


    public void scoreAdd(float input)
    {
        score += input;

        updateScore();
    }

    public void updateScore()
    {    
        ScoreCard.text = score.ToString();
    }

    public void setActiveWeapon(GameObject weapon)
    {
        activeWeapon = weapon;
        activeShoot = activeWeapon.GetComponentInChildren<PlayerShoot>();         
    }


    #region HP functions
    public void setPlayerHP(float to)
    {
        playerHP = to;
    }

    public float updatePlayerHP(bool hurt, float strength)
    {
        if(hurt == true)
        {
            playerHP -= strength;
            Debug.Log("Player Hurt! HP: "+playerHP);
            return playerHP;            
        } else 
        {
            playerHP += strength;
            Debug.Log("Player Heal! HP: "+playerHP);
            return playerHP;
        }
    }

    #endregion


}
