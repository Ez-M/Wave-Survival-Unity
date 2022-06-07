using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombNav : MonoBehaviour
{

    private NavMeshAgent nm;
    public Transform navTarget;

    // Start is called before the first frame update
    void Start()
    {
        nm = gameObject.GetComponent<NavMeshAgent>();
        StartCoroutine(think());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private GameObject calculateNavTarget()
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

    IEnumerator think()
    {
        navTarget = calculateNavTarget().transform;
        nm.SetDestination(navTarget.position);
        yield return new WaitForSeconds(0.5f);
    }




}
