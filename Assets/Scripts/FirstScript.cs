using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int score = 0;
        string playerName = "billy";
        float jump = 5.82f;
        bool gameOver = false;

        float myNumber = 5.85f;

        Debug.Log(myNumber);

        int numA = 10;
        int numB = 15;

        if (numA != numB)
        {
            Debug.Log("Not Equal");
        }

        if (numA > numB)
        {
            Debug.Log("Greater Than");
        }
        else if (numA < numB)
        {
            Debug.Log("Less Than");
        }

        Debug.Log(transform.position);

        Vector3 testVector = new Vector3(10, 2, -5);

        transform.position += testVector;

        Debug.Log(transform.position);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
