using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombAI : MonoBehaviour
{

    #region mobStats

    public float health = 4.0f, moveSpeed = 1f;
    private float LLH = 2f, RLH = 2f, RAH = 2f, LAH = 2f, armor = 5f;
    public float meleeAttack = 1f, meleeRange = 4f, meleeFrequency = 1f, meleeKnock = 1f;
    public float projectileRange = 1f, projectileSpeed = 1f, projectileFrequency = 1f, projectileKnock = 1f;

    #endregion

    private GameManager gameManager;
    private WaveManager waveManager;
    private EntryManager entryManager;
    private ZombNav zombNav;
    private GameObject boundingBox;
    private GameObject targetEntry;
    public GameObject navTarget;
    private NavMeshAgent nm;

    public enum AIState { idle, approaching, entering, chasing, attacking };

    public AIState aiState = AIState.approaching;

    private float targetRange;


    #region bools

    private bool interruptMove;
    private bool canMove;
    private bool atWindow;
    private bool isBusy;

    #endregion 

    // Start is called before the first frame update
    void Start()
    {



        init();
        StartCoroutine(think());




    }

    // Update is called once per frame
    void Update()
    {

    }

    public void init()
    {
        nm = gameObject.GetComponent<NavMeshAgent>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        waveManager = gameManager.gameObject.GetComponent<WaveManager>();
        entryManager = gameManager.GetComponent<EntryManager>();
        zombNav = this.GetComponent<ZombNav>();
        boundingBox = gameObject.transform.Find("BoundingBox").gameObject;


        // isInside = false;

        canMove = true;
        isBusy = false;
        interruptMove = false;
        atWindow = false;
    }


    IEnumerator think()
    {
        while (true)
        {
            switch (aiState)
            {
                case AIState.idle:
                    break;

                case AIState.approaching:
                    if (atWindow == true && isBusy == false)
                    {
                        isBusy = true;
                        yield return attemptEntry();
                    }
                    else
                    {
                        calculateNavTarget();
                        nm.SetDestination(navTarget.transform.position);
                    }


                    break;
                case AIState.entering:
                    break;
                case AIState.chasing:
                    calculateNavTarget();
                    nm.SetDestination(navTarget.transform.position);
                    break;
                case AIState.attacking:
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(0.1f);
        }

    }



    #region thinkFunctions

    private void calculateNavTarget() //out GameObject navTarget, out float targetRange
    {


        if (aiState == AIState.chasing)
        {
            calculateNearestPlayerNav();
        }
        else if (aiState == AIState.approaching)
        {
            calculateNearestEntry();
        }

    }

    private void calculateNearestEntry()
    {
        // >find nearest window, if targetWindow.isValie==true >>> move to targetWindow.child("entryPoint")
        // >if (isInside)

        if (entryManager.availableEntries.Count > 0)
        {
            // GameObject[] entries;
            // entries = entryManager.availableEntries;
            GameObject closest = null;
            float closeDistance = Mathf.Infinity;
            Vector3 pos = transform.position;
            foreach (GameObject entry in entryManager.availableEntries)
            {
                Vector3 diff = entry.transform.position - pos;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < closeDistance)
                {
                    closest = entry;
                    closeDistance = curDistance;
                }
            }


            navTarget = closest.transform.Find("entryPoint").gameObject; //placeholder
            targetRange = closeDistance;
        }
        else
        {
            navTarget = null;
            targetRange = 0f;
        }






    }


    private void calculateNearestPlayerNav()
    {
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float closeDistance = Mathf.Infinity;
        Vector3 pos = transform.position;
        foreach (GameObject player in players)
        {
            Vector3 diff = player.transform.position - pos;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < closeDistance)
            {
                closest = player;
                closeDistance = curDistance;
            }
        }
        navTarget = closest;
        targetRange = closeDistance;

    }


    IEnumerator attemptEntry()
    {
        BarricadeController TEBC;
        int barHealth;

        TEBC = targetEntry.transform.parent.GetComponent<BarricadeController>();
        barHealth = TEBC.getBarHealth();
        if (barHealth > 0)
        {
            yield return new WaitForSeconds(1f);
            TEBC.updateBoards(false);
        }
        else
        {
            bool check = TEBC.getIsOccupied();
            if (check == false)
                yield return enterPlaceHolder(TEBC);

        }

        // yield return new WaitForSeconds(0.3f);
        isBusy = false;
    }

    IEnumerator enterPlaceHolder(BarricadeController TEBC)
    {
        Vector3 myPos = this.gameObject.transform.position;
        GameObject exitPoint = TEBC.getExitPoint();
        Vector3 targetPos = exitPoint.transform.position;
        int timer = 0;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
        while (timer < 5)
        {
            this.gameObject.transform.position = Vector3.Lerp(myPos, targetPos, 0.2f * timer);
            timer++;
            yield return new WaitForSeconds(0.5f);
        }
        if (timer >= 5)
        {

            setAtWindow(false);
            aiState = AIState.chasing;
        }
        gameObject.GetComponent<NavMeshAgent>().enabled = true;


        //         gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, ADSPosition.transform.localPosition, ADSSpeed * Time.deltaTime);

    }

    #endregion








    public void Damage(PlayerShoot profileIn, GameObject target)
    {
        float damageIn = profileIn.gunDam;
        //    float limbMulti = profileIn.limbMulti;
        switch (target.name)
        {
            case "ZRLeg":
                RLH -= damageIn;
                if (RLH <= 0) { Destroy(target); }
                health -= damageIn * 0.6f;
                Debug.Log("Rleg hit for " + damageIn * 0.6f);
                profileIn.hitScore(10f);
                break;

            case "ZLLeg":
                LLH -= damageIn;
                if (LLH <= 0) { Destroy(target); }
                health -= damageIn * 0.6f;
                Debug.Log("Lleg hit for " + damageIn * 0.6f);
                profileIn.hitScore(10f);
                break;

            case "ZRArm":
                RAH -= damageIn;
                if (RAH <= 0) { Destroy(target); }
                health -= damageIn * 0.6f;
                Debug.Log("Rarm hit for " + damageIn * 0.6f);
                profileIn.hitScore(10f);
                break;
            case "ZLArm":
                LAH -= damageIn;
                if (LAH <= 0) { Destroy(target); }
                health -= damageIn * 0.6f;
                Debug.Log("Larm hit for " + damageIn * 0.6f);
                profileIn.hitScore(10f);
                break;

            case "ZHead":
                health -= damageIn * 2f;
                Debug.Log("Head hit for " + damageIn * 2f);
                profileIn.hitScore(60f);
                break;

            case "ZTorso":
                health -= damageIn;
                Debug.Log("Torso hit for " + damageIn);
                profileIn.hitScore(10f);
                break;

            case "armor":
                armor -= damageIn;
                if (armor <= 0) { Destroy(target); }
                // health -= damageIn*0.6f;
                Debug.Log("armor hit for " + damageIn * 0.6f);
                break;


        }
        if (health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Blooped `em");
            profileIn.hitScore(130f);
            waveManager.hasDied();
        }
    }


    public IEnumerator meleePlayer(GameObject navTarget, float targetRange)
    {

        if (navTarget.tag == "Player" && targetRange < meleeRange)
        {

            Debug.Log("Player is in melee range!");
            RaycastHit hit;
            Vector3 attackDirection;
            attackDirection = gameObject.transform.position - navTarget.transform.position;
            if (Physics.Raycast(gameObject.transform.position, attackDirection, out hit, meleeRange, LayerMask.GetMask("Player"), QueryTriggerInteraction.Ignore)) //  && hit.collider.gameObject == navTarget
            {
                Debug.Log("First raycast hit");
                yield return new WaitForSeconds(0.07f);
                attackDirection = gameObject.transform.position - navTarget.transform.position;
                if (Physics.Raycast(gameObject.transform.position, attackDirection, out hit, meleeRange, LayerMask.GetMask("Player"), QueryTriggerInteraction.Ignore))
                {
                    hit.collider.transform.parent.GetComponent<PlayerManager>().updatePlayerHP(true, meleeAttack);
                }


                yield return true;
            }
            else { yield return false; }

        }
        else { yield return false; }
    }






    #region gets&sets  
    public void setCanMove(bool to)
    {
        canMove = to;
    }

    public bool getCanMove()
    {
        return canMove;
    }

    public void setInterruptMove(bool to)
    {
        interruptMove = to;
    }

    public bool getInterruptMove()
    {
        return interruptMove;
    }

    public void setAtWindow(bool to)
    {
        atWindow = to;
    }
    public bool getAtWindow()
    {
        return atWindow;
    }


    public void setTargetEntry(GameObject to)
    {
        targetEntry = to;
    }
    // public void setIsInside(bool to)
    // {
    //     isInside = to;
    // }
    // public bool getIsInside()
    // {
    //     return isInside;
    // }
    #endregion



    // public void getManagers(out GameManager out1, out WaveManager out2, out EntryManager out3, out GameObject out4)
    // {
    //     out1 = gameManager;
    //     out2 = waveManager;
    //     out3 = entryManager;
    //     out4 = boundingBox;
    // } 

    // rayTrace -> onhit save hit -> run hit.target.parent.onDamage(weaponProfile, hit.target, player.id)
    // onDamage(float profileIn, string target, int playerId)
    // switch (target)
    // case limb: health -= profileIn.damage * damMulti

    //

}
