using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanController : MonoBehaviour
{

    public GameObject playerCap;
    public GameObject mainCam;
    public GameObject gunGam;

    public Transform leanPivot;
    public float currentLean; //actual value of the lean
    public float targetLean;//changes based on where you want to be leaning
    public float leanAngle;// lean angle will set target lean
    public float leanSmoothing;
    public float leanVelocity;

    
    public float leanTime = 10f;

    public bool isLL = false; //bool used to track if leaning left
    public bool isLR = false; //bool used to track if leaning right
    // Start is called before the first frame update
    void Start()
    {
        playerCap = gameObject;

        if(playerCap){
        mainCam = playerCap.transform.Find("PlayerCamera").gameObject;
        gunGam = mainCam.transform.Find("gunCam").gameObject;


        } else {Debug.Log("NO PLAYER CONNECTED");}
        
    }

    // Update is called once per frame
    void Update()
    { 
        
        
    }

    public void LeanRight()
    {
        targetLean = -leanAngle;
    }

        public void LeanLeft()
    {
        targetLean = leanAngle;
    }

    public void CalculateLeaning()
    {

        if (isLL)
        {
            targetLean = leanAngle;
        }
        else if (isLR)
        {
            targetLean = -leanAngle;
        }
        else
        {
            targetLean = 0;
        }


        currentLean = Mathf.SmoothDamp(currentLean, targetLean, ref leanVelocity, leanSmoothing);

        leanPivot.localRotation= Quaternion.Euler(new Vector3(0, 0, currentLean));
    }

 
}
