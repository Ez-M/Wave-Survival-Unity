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
        nm.SetDestination(navTarget.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
