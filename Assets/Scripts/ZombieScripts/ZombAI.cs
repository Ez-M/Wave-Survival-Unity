using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombAI : MonoBehaviour
{
    public float health = 4.0f,  moveSpeed = 1f;
    private float LLH = 2f, RLH = 2f, RAH = 2f, LAH = 2f, armor = 5f;
    public float meleeAttack = 1f, meleeRange = 1f, meleeFrequency=1f, meleeKnock = 1f;
    public float projectileRange = 1f, projectileSpeed = 1f, projectileFrequency = 1f, projectileKnock =1f;

    private GameManager gameManager;
    private WaveManager waveManager;
    private EntryManager entryManager;
    private ZombNav zombNav;


    // Start is called before the first frame update
    void Start()
    {

        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        waveManager = gameManager.gameObject.GetComponent<WaveManager>();
        entryManager = gameManager.GetComponent<EntryManager>();
        zombNav = this.GetComponent<ZombNav>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Damage(PlayerShoot profileIn, GameObject target)
    {
       float damageIn = profileIn.gunDam;   
    //    float limbMulti = profileIn.limbMulti;
            switch (target.name)
            {
            case "ZRLeg":
            RLH -= damageIn;
            if (RLH <= 0){Destroy(target);}
            health -= damageIn*0.6f;
            Debug.Log("Rleg hit for " + damageIn*0.6f);
            profileIn.hitScore(10f);
            break;
            
            case "ZLLeg":
            LLH -= damageIn;
            if (LLH <= 0){Destroy(target);}
            health -= damageIn*0.6f;
            Debug.Log("Lleg hit for " + damageIn*0.6f);
            profileIn.hitScore(10f);
            break;

            case "ZRArm":
            RAH -= damageIn;
            if (RAH <= 0){Destroy(target);}
            health -= damageIn*0.6f;
            Debug.Log("Rarm hit for " + damageIn*0.6f);
            profileIn.hitScore(10f);
            break;            
            case "ZLArm":
            LAH -= damageIn;
            if (LAH <= 0){Destroy(target);}
            health -= damageIn*0.6f;
            Debug.Log("Larm hit for " + damageIn*0.6f);
            profileIn.hitScore(10f);
            break;

            case "ZHead": health-= damageIn*2f;
            Debug.Log("Head hit for " + damageIn*2f);
            profileIn.hitScore(60f);
            break;

            case "ZTorso": health -=damageIn;
            Debug.Log("Torso hit for " + damageIn);
            profileIn.hitScore(10f);
            break;

            case "armor":
            armor -= damageIn;
            if (armor <= 0){Destroy(target);}
            // health -= damageIn*0.6f;
            Debug.Log("armor hit for " + damageIn*0.6f);
            break;


                   }    
        if (health <= 0){
            Destroy(gameObject);
            Debug.Log("Blooped `em");
            profileIn.hitScore(130f);
            waveManager.hasDied();
            }
    }
    

    public bool meleePlayer(GameObject navTarget, float targetRange)
    {
        
        if(navTarget.tag == "Player" && targetRange < meleeRange)
                {
                    RaycastHit hit;
                    Vector3 attackDirection;
                    attackDirection = gameObject.transform.position - navTarget.transform.position;
                    if (Physics.Raycast (gameObject.transform.position, attackDirection , out hit, meleeRange, 1 ,QueryTriggerInteraction.Ignore))
                    {
                        return true;
                    } else {return false;}

                }   else {return false;}
    }


    public void getManagers(out GameManager out1, out WaveManager out2, out EntryManager out3)
    {
        out1 = gameManager;
        out2 = waveManager;
        out3 = entryManager;
    } 

    // rayTrace -> onhit save hit -> run hit.target.parent.onDamage(weaponProfile, hit.target, player.id)
    // onDamage(float profileIn, string target, int playerId)
    // switch (target)
    // case limb: health -= profileIn.damage * damMulti

    //

}
