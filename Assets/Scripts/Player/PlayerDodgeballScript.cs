using UnityEngine;

public class PlayerDodgeballScript : MonoBehaviour
{
    
    public InputManager myInputManager;
    public GameObject heldDodgeballAsset;
    private SpriteRenderer heldDodgeballSpriteRenderer; 
    private Color lerpedColor = Color.white;
    private GameObject hitDodgeball;
    [SerializeField]
    public GameObject dodgeballPrefab;
    public PlayerMovement playerMovement;
    private PlayerHealthScript myHealthScript;
    private bool holdingDodgeball = false;
    private float timeSpentAiming = 0f;
    private float maxAimingTime = 5f;
    public GameObject aim;

    public float firingSpeed = 160f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        myHealthScript = GetComponent<PlayerHealthScript>();
        heldDodgeballSpriteRenderer = heldDodgeballAsset.GetComponent<SpriteRenderer>();
        
        heldDodgeballAsset.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        if (holdingDodgeball)
        {
            ThrowCheck();
        }

        if (playerMovement._isAiming)
        {
            HoldingThrowTimer();
            lerpedColor = Color.Lerp(Color.red, Color.white, Mathf.PingPong(Time.time, (2/timeSpentAiming)));
            heldDodgeballSpriteRenderer.color = lerpedColor;
            
	        //aimAngle = Mathf.Atan2(myInputManager.Movement.y , myInputManager.Movement.x) * Mathf.Rad2Deg;
        }
    }

    void FixedUpdate()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Dodgeball"))
        {
            hitDodgeball = collision.gameObject;
            DodgeballScript _hitDodgeBallScript = hitDodgeball.GetComponent<DodgeballScript>();


            if (_hitDodgeBallScript._isLive && (_hitDodgeBallScript.originPlayer != this.name))
            {
                Debug.Log("OUCH");
                myHealthScript.HealthLoss();

                
                //LOSE HEALTH AND REFLECT DODGEBALL 
                _hitDodgeBallScript._isLive = false;
                _hitDodgeBallScript.originPlayer = "";
            }

            else if (!_hitDodgeBallScript._isLive && !holdingDodgeball)
            {
                Destroy(hitDodgeball);
                Debug.Log("Pick up Dodgeball"); 
                heldDodgeballAsset.SetActive(true);
                holdingDodgeball = true;
            }
        }
    }


    #region Throwing
    
        //WHEN THROW BUTTON PRESSED
        //Check to make sure you're holding dodgeball, otherwise dont do anything
        //if you are, WHILE YOU'RE HOLDING THE BUTTON, halt movement and let the player begin to aim
        //using movement controls
        //WHEN RELEASED 
        //Instantiate an instance of the dodgeball prefab, set it to LIVE, and throw it in the direction you're facing 

    private void ThrowCheck()
    {
        if (myInputManager.ThrowWasPressed)
        {
            //Debug.Log("Aiming...");
            playerMovement._isAiming = true;
        }
        else if (myInputManager.ThrowWasReleased)
        {
            Throw();
        }
    }

    private void Throw()
    {
        Debug.Log("firing");

        playerMovement._isAiming = false;
        heldDodgeballSpriteRenderer.color = Color.white;
        heldDodgeballAsset.SetActive(false);
        lerpedColor = Color.white;
        holdingDodgeball = false;

        //Instantiate dodgeball and throw it at an angle
        GameObject dodgeballInst = Instantiate(dodgeballPrefab, transform.position, transform.rotation);
            
        DodgeballScript thrownDodgeballScript = dodgeballInst.GetComponent<DodgeballScript>();
        thrownDodgeballScript.originPlayer = this.name;
        thrownDodgeballScript._isLive = true;
        //dodgeballInst.transform.eulerAngles = Vector3.forward * aimAngle;
        //dodgeballInst.GetComponent<Rigidbody2D>().linearVelocity = (myInputManager.Movement * (firingSpeed/10) * (1f + timeSpentAiming));
        if(myInputManager.Movement == new Vector2(0f, 0f))
        {
            dodgeballInst.GetComponent<Rigidbody2D>().AddForce(transform.right * firingSpeed * (1f + timeSpentAiming));
        }
        else
        { 
            dodgeballInst.GetComponent<Rigidbody2D>().AddForce(myInputManager.Movement * firingSpeed * (1f + timeSpentAiming));
        }
        //dodgeballInst.GetComponent<Rigidbody2D>().AddForce(myInputManager.Movement * firingSpeed * (1f + timeSpentAiming));
        timeSpentAiming = 0f;
        
    }
    #endregion

    #region Timer

    private void HoldingThrowTimer()
    {
        if (timeSpentAiming < maxAimingTime)
        {
            timeSpentAiming += Time.deltaTime;
        }
    }
    #endregion
}
