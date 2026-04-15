using UnityEngine;

public class PlayerDodgeballScript : MonoBehaviour
{
    
    public InputManager myInputManager;
    public GameObject heldDodgeballAsset;
    private GameObject hitDodgeball;
    [SerializeField]
    public GameObject dodgeballPrefab;
    public PlayerMovement playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        heldDodgeballAsset.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        ThrowCheck();
    }

    void FixedUpdate()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Dodgeball"))
        {
            hitDodgeball = collision.gameObject;
            DodgeballScript dbs_heldDodgeball = hitDodgeball.GetComponent<DodgeballScript>();
            if (dbs_heldDodgeball._isLive)
            {
                Debug.Log("OUCH");
            }
            else
            {
                Destroy(hitDodgeball);
                Debug.Log("Pick up Dodgeball"); 
                heldDodgeballAsset.SetActive(true);
            }
        }
    }


    #region Throwing
    private void ThrowCheck()
    {
        //WHEN THROW BUTTON PRESSED
        //Check to make sure you're holding dodgeball, otherwise dont do anything
        //if you are, WHILE YOU'RE HOLDING THE BUTTON, halt movement and let the player begin to aim
        //using movement controls
        //WHEN RELEASED 
        //Instantiate an instance of the dodgeball prefab, set it to LIVE, and throw it in the direction you're facing 
        if (myInputManager.ThrowWasPressed)
        {
            Debug.Log("Aiming...");
            playerMovement._isAiming = true;
        }
        else if (myInputManager.ThrowWasReleased)
        {
            Debug.Log("firing");
            playerMovement._isAiming = false;
        }
    }
    #endregion
}
