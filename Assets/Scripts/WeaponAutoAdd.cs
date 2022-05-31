using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAutoAdd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
          
    addonstart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void addonstart()
    {
        //Add this object to the allweapons list
        WeaponManager.instance.allWeapons.Add(gameObject); 

        // this.gameObject.SetActive(false);
        this.enabled = false;
        
    }
}
