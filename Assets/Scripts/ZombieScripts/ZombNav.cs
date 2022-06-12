using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombNav : MonoBehaviour
{

    private NavMeshAgent nm;
    public Transform navTarget;
    private ZombAI zombAI; 

    private bool isInside;

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
        isInside = false;
    }



    IEnumerator think()
    {
        while(true)
        {
        navTarget = calculateNavTarget().transform;
        nm.SetDestination(navTarget.position);
        yield return new WaitForSeconds(0.5f);
        }
    }



    private GameObject calculateNavTarget()
    {
        if (isInside == true)
        {
            return calculateNearestPlayerNav();
        } else 
        {
           return calculateNearestEntry();
        }

    }

    private GameObject calculateNearestEntry()
     {
                // >find nearest window, if targetWindow.isValie==true >>> move to targetWindow.child("entryPoint")
                // >if (isInside)
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




}
