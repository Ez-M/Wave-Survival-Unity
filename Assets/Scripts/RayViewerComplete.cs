using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayViewerComplete : MonoBehaviour
{
// Distance in Unity units over which the Debug.DrawRay will be drawn
    public float weaponRange = 50f;


 // Holds a reference to the first person camera

    public Camera fpsCam; 
    // Start is called before the first frame update
    void Start()
    {
        // 
        fpsCam = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Create a vector at the center of our camera's viewport
        Vector3 lineOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

        //Draw a line in the Scene View  from the point lineOrigin in the direction of fpsCam.transform.forward * weaponRange, using the color green
        Debug.DrawRay(lineOrigin, fpsCam.transform.forward * weaponRange, Color.green);
        
    }
}
