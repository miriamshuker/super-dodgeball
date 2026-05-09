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


    [SerializeField] private bool inHyperForm;
    [SerializeField] private float chargeTime;
    [SerializeField] private float maxChargeTime = 4f;


    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject hyperBeam;
    [SerializeField] private GameObject gokuCharge;
    [SerializeField] private GameObject gokuShoot;
    private Vector2 lastMousePos;
    private Vector2 smoothedDirection = Vector2.up;
    private float directionSmoothTime = 0.1f;

    private Vector2 beamDirection = Vector2.up;
    private float chargePower = 0f;
    private float debugTimer = 0f;


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
        triggerSpawner = GameObject.Find("==== HyperSpawner =====");
        inHyperForm = false;

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
        gokuCharge?.SetActive(true);
        chargeTime -= Time.deltaTime;
        debugTimer += Time.deltaTime;

        Vector2 currentMousePos = Mouse.current.position.ReadValue();
        Vector2 mouseDelta = currentMousePos - lastMousePos;
        lastMousePos = currentMousePos;

        if (mouseDelta != Vector2.zero)
        {
            Vector2 targetDirection = mouseDelta.normalized;
            smoothedDirection = Vector2.Lerp(smoothedDirection, targetDirection, directionSmoothTime);
            beamDirection = smoothedDirection.normalized;
        }

        float moveSpeed = mouseDelta.magnitude;
        chargePower = Mathf.Clamp01(chargePower + (moveSpeed * 0.0001f)); // Adjust multiplier as needed
        float angle = Mathf.Atan2(beamDirection.y, beamDirection.x) * Mathf.Rad2Deg; //GOKUUUUUU
        playerBody.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        if (debugTimer >= 1f)
        {
            Debug.Log($"Charging... Time left: {chargeTime:F2}s | Beam direction: {beamDirection} | Charge Power: {chargePower:F2}");
            debugTimer = 0f;
        }
        
        if (chargeTime <= 0f)
        {
            TransitionToShooting();
        }
    }

    private void HandleShooting()
    {
        gokuCharge?.SetActive(false);
        gokuShoot?.SetActive(true);
        hyperBeam.SetActive(true);

        //Debug.Log("Shooting hyper beam toward " + beamDirection + " with " + chargePower);
        if (hyperStateTimer >= 1.5f)
        {
            TransitionToCooldown();
        }
    }

    private void HandleCooldown()
    {
        hyperBeam.SetActive(false);
        gokuShoot?.SetActive(false);
        playerBody.transform.rotation = Quaternion.identity;
        inHyperForm = false;
        playerMovement._isHyper = false;
        triggerSpawner.GetComponent<HB_TriggerSpawner>().hyperInPlay = false;

        currentHyperState = HyperState.Idle;
        hyperStateTimer = 0f;
        chargeTime = maxChargeTime;

        //Debug.Log("Hyper move complete");
    }

    private void TransitionToShooting()
    {
        currentHyperState = HyperState.Shooting;
        hyperStateTimer = 0f;

        Debug.Log($"Shooting beam with direction: {beamDirection} and power: {chargePower}");
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
                Debug.Log("Dodgeball evaporated");
            }
            playerMovement._isHyper = true;
        }
        if (collision.gameObject.CompareTag("HyperBeam"))
        {
            //asd
            Debug.Log("hit by hyperbeam");
        }
    }
}