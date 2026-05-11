using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class HB_PlayerScript : MonoBehaviour
{
    public InputManager myInputManager;
    public PlayerMovement playerMovement;
    private GameObject triggerSpawner;
    public PlayerDodgeballScript playerDbScript;


    public bool inHyperForm;
    [SerializeField] private float chargeTime;
    [SerializeField] private float maxChargeTime = 4f;
    private float chargePower = 0f;
    [SerializeField] private float minCharge = .7f;
    [SerializeField] private float maxCharge = 2.3f;


    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject hyperAnchor;
    [SerializeField] private GameObject hyperAim;
    [SerializeField] private GameObject hyperBeam;
    private Vector3 hyperBeamSize;
    private Vector2 smoothedDirection = Vector2.up;
    private float directionSmoothTime = 0.1f;

    public Vector2 beamDirection = Vector2.up;
    public float angle;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D playerRb;
    [SerializeField] private float knockbackForce = 20f;


    // Hyper state tracking
    private HyperState currentHyperState = HyperState.Idle;
    private float hyperStateTimer = 0f;

    enum HyperState
    {
        Idle,
        Charging,
        Shooting,
        Cooldown
    }

    void Start()
    {
        spriteRenderer = playerBody.GetComponent<SpriteRenderer>();
        triggerSpawner = GameObject.Find("==== HyperSpawner =====");
        inHyperForm = false;
        playerRb = GetComponent<Rigidbody2D>();


        hyperBeamSize = hyperAnchor.transform.localScale;


        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (inHyperForm)
        {
            UpdateHyperState();
        }
    }

    private void UpdateHyperState()
    {
        hyperStateTimer += Time.deltaTime;

        switch (currentHyperState)
        {
            case HyperState.Charging:
                HandleCharging();
                break;
            case HyperState.Shooting:
                HandleShooting();
                break;
            case HyperState.Cooldown:
                HandleCooldown();
                break;
        }
    }

    private void HandleCharging()
    {
        hyperAim.SetActive(true);
        chargeTime -= Time.deltaTime;
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        if (mouseDelta != Vector2.zero)
        {
            Vector2 targetDirection = mouseDelta.normalized;
            smoothedDirection = Vector2.Lerp(smoothedDirection, targetDirection, directionSmoothTime);
            beamDirection = smoothedDirection.normalized;
        }

        float moveSpeed = mouseDelta.magnitude;
        chargePower = Mathf.Clamp(chargePower + (moveSpeed * 0.0007f), minCharge, maxCharge);

        angle = Mathf.Atan2(beamDirection.y, beamDirection.x) * Mathf.Rad2Deg;
        playerBody.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        spriteRenderer.flipY = beamDirection.x < 0f;

        hyperAnchor.transform.localScale = new Vector3(hyperBeamSize.x, hyperBeamSize.y * chargePower, 1f);


        if (chargeTime <= 0f || chargePower == maxCharge)
        {
            TransitionToShooting();
        }
    }

    private void HandleShooting()
    {
        hyperAim.SetActive(false);
        hyperBeam.SetActive(true);

        playerBody.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        spriteRenderer.flipY = beamDirection.x < 0f;

        if (hyperStateTimer >= 1.5f)
        {
            TransitionToCooldown();
        }
    }

    private void HandleCooldown()
    {
        hyperBeam.SetActive(false);
        playerBody.transform.rotation = Quaternion.identity;
        inHyperForm = false;
        playerMovement._isHyper = false;
        triggerSpawner.GetComponent<HB_TriggerSpawner>().hyperInPlay = false;

        playerMovement._isFacingRight = beamDirection.x >= 0f;

        hyperAnchor.transform.localScale = new Vector3(hyperBeamSize.x, hyperBeamSize.y, 1f); //resets hyperbeam prevent scale issues
        spriteRenderer.flipY = false; //and this returns rotation to normal

        currentHyperState = HyperState.Idle;
        hyperStateTimer = 0f;
        chargeTime = maxChargeTime;
        chargePower = 0f;

        //Debug.Log("Hyper move complete");
    }

    private void TransitionToShooting()
    {
        currentHyperState = HyperState.Shooting;
        hyperStateTimer = 0f;

        if (playerRb != null)
        {
            float knockbackForce = .1f * chargePower;
           playerRb.AddForce(-beamDirection * knockbackForce, ForceMode2D.Impulse);
            playerMovement._moveVelocity = playerRb.linearVelocity;
        }

        //Debug.Log($"Shooting beam with direction: {beamDirection} and power: {chargePower}");
    }

    private void TransitionToCooldown()
    {
        currentHyperState = HyperState.Cooldown;
        hyperStateTimer = 0f;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("HyperTrigger"))
        {
            HB_TriggerMovement trigger = collision.GetComponent<HB_TriggerMovement>();
            if (trigger == null || trigger.claimed) return; // already taken
            trigger.claimed = true;

            Destroy(collision.gameObject);
            Destroy(collision.gameObject);
            inHyperForm = true;
            currentHyperState = HyperState.Charging;
            hyperStateTimer = 0f;
            chargePower = 0f;
            chargeTime = maxChargeTime;
            playerDbScript.GetComponent<PlayerDodgeballScript>().heldDodgeballAsset.SetActive(false);
            if (playerDbScript.GetComponent<PlayerDodgeballScript>().holdingDodgeball)
            {
                playerDbScript.GetComponent<PlayerDodgeballScript>().holdingDodgeball = false;
            }
            playerMovement._isHyper = true;
        }
        if (collision.gameObject.CompareTag("HyperBeam"))
        {
            //RoundManagerScript.Instance.PlayerDamaged(this.name);
            //RoundManagerScript.Instance.PlayerDamaged(this.name);

            HB_PlayerScript shooterScript = collision.transform.parent.GetComponent<HB_PlayerScript>();
            if (shooterScript != null)
            {
                Debug.Log("punch back target player");
                Vector2 knockbackDir = shooterScript.beamDirection.normalized;
                playerRb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}