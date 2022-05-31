using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{

    public GameObject Player;
    private PlayerController PC;

    public RectTransform reticle;

    // public Rigidbody rb;

    public float restingSize, maxSize, speed, currentSize;

    // [Range(50f, 250f)]
    // public float size;
    // used for testing reticle ranges \\

    private void Awake() {
        
        reticle = GetComponent<RectTransform>();
        currentSize = 50f; maxSize = 400f; restingSize = 50f; speed = 12f;
        Player = GameObject.Find("PlayerCap");
        PC = Player.GetComponent<PlayerController>();
        

    }


    private void Start()
    {
        
    }

    private void Update() {
        // reticle.sizeDelta = new Vector2(size, size);

        if (PC.isMoving) {
            currentSize = Mathf.Lerp(currentSize, maxSize, Time.deltaTime * speed);
        } else {
            currentSize = Mathf.Lerp(currentSize, restingSize, Time.deltaTime * speed);
        }

        reticle.sizeDelta = new Vector2(currentSize, currentSize);
    }

    // bool isMoving {
    //     get {
    //         if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
    //             return true;
    //         else
    //             return false;
            
    //     }
    // }

}
