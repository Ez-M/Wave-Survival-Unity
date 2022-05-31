using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonController : MonoBehaviour
{

    public int clicksToPop = 3; 
    
    //public allows the variable to be accessed outside of this script

    void OnMouseDown ()
    {
        clicksToPop --;
        transform.localScale += new Vector3(0.2f, 0.2f, 0.2f);

        if (clicksToPop <= 0) {
            Debug.Log("POP");
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         
    }
}
