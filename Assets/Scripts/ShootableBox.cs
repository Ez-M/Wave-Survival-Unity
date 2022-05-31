using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableBox : MonoBehaviour
{

    public float maxhealth =  3.0f ;
    public float currentHealth = 3.0f;

    public void Damage(PlayerShoot profileIn)
    {
        float damageIn = profileIn.gunDam;  
        //subtract damage amount when Damage function is called
        currentHealth -= damageIn;

        //Check if health has fallen below zero
        if (currentHealth <= 0)
        {
            //if health has fallen below zero, deactivate it
            gameObject.SetActive (false);
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
