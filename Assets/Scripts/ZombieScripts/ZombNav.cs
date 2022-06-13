using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombNav : MonoBehaviour
{

    private NavMeshAgent nm;
    public Transform navTarget;
    private ZombAI zombAI;
    private GameManager gameManager;
    private EntryManager entryManager;
    private WaveManager waveManager;

    private GameObject targetEntry;

    #region bools
    private bool isInside;
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
        zombAI = this.GetComponent<ZombAI>();
        zombAI.getManagers(out gameManager, out waveManager, out entryManager);
        isInside = false;
    }



    IEnumerator think()
    {
        while (true)
        {
            if (atWindow == true && isBusy == false)
            {
                isBusy = true;
                StartCoroutine(attemptEntry());
                yield return new WaitForSeconds(0.2f);
            }

            if (canMove == true && isBusy == false)
            {
                navTarget = calculateNavTarget().transform;
                nm.SetDestination(navTarget.position);
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(0.2f);
        }

    }

    

    //functions, not coroutines
    #region functions  
    private GameObject calculateNavTarget()
    {


        if (isInside == true)
        {
            return calculateNearestPlayerNav();
        }
        else
        {
            return calculateNearestEntry();
        }

    }

    private GameObject calculateNearestEntry()
    {
        // >find nearest window, if targetWindow.isValie==true >>> move to targetWindow.child("entryPoint")
        // >if (isInside)

        if (entryManager.availableEntries.Count > 0)
        {
            // GameObject[] entries;
            // entries = entryManager.availableEntries;
            GameObject closest = null;
            float distance = Mathf.Infinity;
            Vector3 pos = transform.position;
            foreach (GameObject entry in entryManager.availableEntries)
            {
                Vector3 diff = entry.transform.position - pos;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = entry;
                    distance = curDistance;
                }
            }


            return closest.transform.Find("entryPoint").gameObject; //placeholder
        }
        else { return null; }

    }


    private GameObject calculateNearestPlayerNav()
    {
        GameObject[] players;
        players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 pos = transform.position;
        foreach (GameObject player in players)
        {
            Vector3 diff = player.transform.position - pos;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = player;
                distance = curDistance;
            }
        }
        return closest;
    }

    #endregion


    #region coroutines
    IEnumerator attemptEntry()
    {
        BarricadeController TEBC;
        int barHealth;

        TEBC = targetEntry.GetComponent<BarricadeController>();
        barHealth = TEBC.getBarHealth();
        if(barHealth > 0)
        {
            yield return new WaitForSeconds(1f);
            targetEntry.GetComponent<BarricadeController>().updateBoards(false);
        } else 
        {
            //enter coroutine
        }

        yield return new WaitForSeconds(0.3f);
        isBusy = false;
    }

    #endregion

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
     #endregion 

}
