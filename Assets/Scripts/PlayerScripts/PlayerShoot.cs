using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerShoot : MonoBehaviour
{


    #region -gunStats-
    public int maxAmmo = 100, curAmmo = 100, maxMag = 10, curMag = 10;
    // ints handling ammo/mag capacity and current
    //bools for handling reload states and if you have ammo

    public float gunDam = 1.0f, gunRangeOptimal = 1.0f, gunRangeMax = 50f, gunRof = 0.25f, gunLoadTime = 1.0f, gunArmorPen = 1.0f, disRunMulti = 2f, gunHitForce = 100f, gunBurstMin = 1f, gunBurstMax = 1f, gunShotsPerShot = 1f;


    //values for calculating weapon dispersion

    public float gunDisBase = 15f; //degress of weapon spread at maximum, the following floats modify this value 
    public float gunDisStand = 1f, gunDisCrouch = 1f, gunDisProne = 1f, gunDisWalk = 2f, gunDisCrouchWalk = 1f, gunDisRun = 5f, gunDisSlide = 1f, gunDisJump = 1f;

    public int ADSSpeed = 10;

    public bool gunIsAuto;
    #endregion


    public bool hasFired = false;

    public bool isReloading = false, hasAmmo = true;

    private float totalDispersion;

    private Vector3 bulletTrajectory;


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
    private PlayerInputHandler playerInputHandler;
    [SerializeField] GameObject subGun;
    [SerializeField] Transform  subGunPosition;
    [SerializeField] Transform  subGunADSPosition;
    [SerializeField] Transform  subGunEnd;

    [SerializeField] List<Transform> allGunEnds;
    [SerializeField] int gunEndToFire;
    [SerializeField] bool gunEndResetFlag;
    [SerializeField] float gunEndResetTime;
    #endregion




    public AudioSource gunSound;
    public AudioClip bang, reload, click;
    public WaitForSeconds shotDuration = new WaitForSeconds(0.07f);    // WaitForSeconds object used by our ShotEffect coroutine, determines time laser line will remain visible
    public LineRenderer laserLine;                                        // Reference to the LineRenderer component which will display our laserline
    public float nextFire;                                                // Float to store the time the player will be allowed to fire again, after firing
    private GameObject hitDecal;


    private bool input_Fire1;
    private bool input_Fire2;
    private bool input_Reload;





    private void Awake()
    {

    }
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

        if (gunisInit == true) //checks if the weapon's owner has been established (gunInit can be called externally)
        {

            collectInputs();


            weaponSecondary();

            weaponFire();

            //  bool isAim = input_Fire2;
            // if (isAim)
            // {
            //     gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, ADSPosition.transform.localPosition, ADSSpeed * Time.deltaTime);
            // }else{
            //     gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, hipPosition.transform.localPosition, ADSSpeed * Time.deltaTime);
            // }




            //controls and allows for manual reloading
            if (input_Reload && curMag < maxMag && isReloading != true)
            {
                StartCoroutine(loadGun());
            }


            //    Vector3(0.228,-0.218999997,0.532999992) gun normal pos
            // Vector3(0,-0.150000006,0.532999992) gun ads pos
            // Vector3(0.228, -0.068999991, 0) pos difference
            // ^^ adsstandoff
            // Vector3 curGunPos = Transform.position(GameObject.gun) 
            // Vector3 adsGunPos = curGunPos-adsStandoff
            // while isAim Transform.position(GameObject.gun - adsStandoff)
            //

        }

    }



    #region -Functions-


    public void gunInit()
    {

        playerManager = transform.root.GetComponent<PlayerManager>();
        playerCap = playerManager.playerCap;
        playerController = playerManager.playerController;
        leanPoint = playerManager.leanPoint;
        playerHead = playerManager.playerHead;
        playerCam = playerManager.playerCam;
        gunCam = playerManager.gunCam;

        characterController = playerCap.GetComponent<CharacterController>();

        inventoryController = playerManager.gameObject.GetComponent<InventoryController>();
        playerInputHandler = playerManager.playerInputHandler;



        WeaponUI = playerManager.WeaponUI;
        WUIC = playerManager.WUIC;
        ScoreCard = playerManager.ScoreCard;
        AmmoCounter = playerManager.AmmoCounter;
        CST = playerManager.CST;


        gun = gameObject;

        // gunEnd = gun.transform.Find("gunEnd");
        // Get and store a reference to our LineRenderer component
        laserLine = gun.GetComponent<LineRenderer>();
        //
        gunSound = gun.GetComponent<AudioSource>();
        //
        hitDecal = GameObject.Find("HitDebug");
        // GameObject.

        ScoreCard = GameObject.Find("ScoreCard").GetComponent<TMPro.TextMeshProUGUI>();

        AmmoCounter = GameObject.Find("AmmoCounter").GetComponent<TMPro.TextMeshProUGUI>();



        gunisInit = true;

    }




    private void collectInputs()
    {
        input_Fire1 = playerInputHandler.input_Fire1;
        input_Fire2 = playerInputHandler.input_Fire2;
        input_Reload = playerInputHandler.input_Reload;
    }

    private IEnumerator ShotEffect()
    {
        //Turn on our line renderer
        laserLine.enabled = true;

        //wait for .07 seconds
        yield return shotDuration;

        //Deactivate our line renderer after waiting
        laserLine.enabled = false;
    }


    #region -inputFunctions-




    #endregion

    private void calculateDispersion()
    {



        totalDispersion = (gunDisBase * (characterController.isGrounded ? (playerController.isMoving ? (playerController.isRunning ? gunDisRun : gunDisWalk) : 1) : gunDisJump));
        Vector2 spreadDirection = Random.insideUnitCircle.normalized; //Get a random direction for the spread
        Vector3 offsetDirection = new Vector3(playerCam.transform.right.x * spreadDirection.x, playerCam.transform.up.y * spreadDirection.y, 0); //Align direction with fps cam direction

        float offsetMagnitude = Random.Range(0f, totalDispersion); //Get a random offset amount
        offsetMagnitude = Mathf.Tan(offsetMagnitude); //Convert to segment length so we get desired degrees value
        bulletTrajectory = playerCam.transform.forward + (offsetDirection * offsetMagnitude); //Add our offset to our forward vector



        // if(isRUnning == true){}


        //private canJump(){}
        //if (canJump >0 && jumpDelay <=0)
        //{JUMP}
        // JUMP(){jump; jumpDelay = 10}
        //per frame if(isGrounded == true){canJump = 10; jumpDelay--} else canJump--
    }

    public void weaponFire()
    {


        calculateDispersion();

        bool isFire = playerInputHandler.input_Fire1;
        if ((isFire == true && hasFired == false) || (isFire == true && gunIsAuto == true))
        {

            if (Time.time > nextFire && curMag >= 1 && isReloading != true)
            {
                //// gun loaded behavior check ////


                // Update the time when our player can fire next
                nextFire = Time.time + gunRof;

                // Start our ShotEffect coroutine to turn our laser line on and off
                StartCoroutine(ShotEffect());

                // Create a vector at the center of our camera's viewport
                Vector3 rayOrigin = playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
                RaycastHit hit;
                //Declare a raycast hit to store information about what our ractast has hit


                // Set the start position for our visual effect for our laser to the position of gunEnd
                laserLine.SetPosition(0, allGunEnds[gunEndToFire].position);

                int curShots = 0;
                // Check if our raycast has hit anything

                while (curShots < gunShotsPerShot)
                {
                    if (Physics.Raycast(rayOrigin, bulletTrajectory, out hit, gunRangeMax, 1, QueryTriggerInteraction.Ignore))
                    {




                        // Set the end position for our laser line
                        laserLine.SetPosition(1, hit.point);

                        GameObject newHit = GameObject.Instantiate(hitDecal);

                        newHit.transform.position = hit.point + new Vector3(0, 0, .01f);


                        // Get a refernce to a health script attached to the collider we hit
                        if (hit.collider.GetComponent<ShootableBox>())
                        {
                            ShootableBox health = (hit.collider.GetComponent<ShootableBox>());

                            // If there was a health script attached
                            if (health != null)
                            {
                                // Call the damage function of the script, passing in our gunDam variable
                                health.Damage(this);
                            }

                            // Check if the object we hit has a rigidbody attached
                            if (hit.rigidbody != null)
                            {
                                hit.rigidbody.AddForce(-hit.normal * gunHitForce);
                            }
                        }
                        else if (hit.collider.GetComponentInParent<ZombAI>())
                        {
                            ZombAI health = (hit.collider.GetComponentInParent<ZombAI>());
                            health.Damage(this, hit.collider.gameObject);
                            playerManager.updateScore();
                        }



                    }
                    else
                    {
                        //If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
                        laserLine.SetPosition(1, rayOrigin + (bulletTrajectory * gunRangeMax));
                    }

                    curShots++;
                    // //Play gunshot audio and post to debug log
                    // gunSound.PlayOneShot(bang);
                    // Debug.Log("BANG!");
                    // //reduces current mag contents
                    // curMag--;

                    // WUIC.updateAmmo();
                }

                                    //Play gunshot audio and post to debug log
                    gunSound.PlayOneShot(bang);
                    Debug.Log("BANG!");
                    //reduces current mag contents
                    curMag--;
                   if(gunEndToFire >= allGunEnds.Count-1){gunEndToFire=0;} else {gunEndToFire++;}

                    WUIC.updateAmmo();

            }
            else if (Time.time > nextFire && curMag <= 0 && isReloading != true && hasFired == false)
            {
                Debug.Log("click");
                gunSound.PlayOneShot(click);
                StartCoroutine(loadGun());


            }

            hasFired = true;
        }

    }


    private void weaponSecondary()
    {
    bool isAim=input_Fire2;
    if(isAim)
    {
        // Debug.Log("isAim");
    gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, ADSPosition.transform.localPosition, ADSSpeed * Time.deltaTime);
        }
        else
        {
            gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, hipPosition.transform.localPosition, ADSSpeed * Time.deltaTime);
        }
    }


    public void hitScore(float input)
    {
        playerManager.scoreAdd(input);
    }


    private IEnumerator loadGun()
    {

        Debug.Log("reloading");

        if (curAmmo == 0)
        {
            Debug.Log("No Ammo");
        }
        else if (curAmmo + curMag > maxMag)
        {
            gunSound.PlayOneShot(reload);
            isReloading = true; // important to prevent reloading bugs                   
            yield return new WaitForSeconds(gunLoadTime);
            curAmmo -= maxMag - curMag;
            curMag = maxMag;
            WUIC.updateAmmo();

            isReloading = false;
            gunSound.PlayOneShot(reload);
            Debug.Log(curAmmo + " ammo remaining");
        }
        else
        {
            gunSound.PlayOneShot(reload);
            isReloading = true; // important to prevent reloading bugs  
            yield return new WaitForSeconds(gunLoadTime);
            curMag += curAmmo;
            curAmmo = 0;
            WUIC.updateAmmo();

            isReloading = false;
            gunSound.PlayOneShot(reload);
            Debug.Log(curAmmo + " ammo remaining");
        }
    }

    #endregion







}
