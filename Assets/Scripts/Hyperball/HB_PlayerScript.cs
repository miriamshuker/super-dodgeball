using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class HB_PlayerScript : MonoBehaviour
{
    public InputManager myInputManager;
    public PlayerMovement playerMovement;

    private GameObject triggerSpawner;
    public PlayerDodgeballScript playerDbScript;
    //public GameObject heldDodgeballAsset;
    //private SpriteRenderer heldDodgeballSpriteRenderer;
    //private bool holdingDodgeball = false;

    //private GameObject hyperTrigger;
    [SerializeField] private bool inHyperForm;
    [SerializeField] private float chargeTime;

    [SerializeField] private GameObject gokuCharge;
    [SerializeField] private GameObject gokuShoot;


    void Start()
    {
        triggerSpawner = GameObject.Find("==== HyperSpawner =====");

        inHyperForm = false;
    }

    void Update()
    {
        if (inHyperForm)
        {
            gokuCharge?.SetActive(true);
            Debug.Log("get hyper");
            ChargingHyper();
        }
    }

    private void ChargingHyper()
    {
        if (chargeTime > 0f)
        {
            chargeTime -= Time.deltaTime;
        }
        else
        {
            ShootHyper();
            Debug.Log("hyper shoot or fizzle");
        }
        //heldDodgeballAsset.SetActive(true);
        //holdingDodgeball = true;
        playerDbScript.GetComponent<PlayerDodgeballScript>().heldDodgeballAsset.SetActive(false);
        if (playerDbScript.GetComponent<PlayerDodgeballScript>().holdingDodgeball == true)
        {
            playerDbScript.GetComponent<PlayerDodgeballScript>().holdingDodgeball = false;
            Debug.Log("dodgeball evaporated/died");
        }
    }

    private IEnumerator ShootHyper()
    {
        gokuCharge?.SetActive(false);
        gokuShoot?.SetActive(true);

        //suspend player in air/slide backwards from beam shooting
        yield return new WaitForSeconds(1.5f);
        gokuShoot?.SetActive(false);

        inHyperForm = false;

        triggerSpawner.GetComponent<HB_TriggerSpawner>().hyperInPlay = false;
        chargeTime = 10f;

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HyperTrigger"))
        {
            //hyperTrigger = collision.gameObject;
            Destroy(collision.gameObject);
            inHyperForm = true;
        }

        if (collision.gameObject.CompareTag("HyperBeam"))
        {

        }
    }
}
