using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HB_TriggerMovement : MonoBehaviour
{
    public enum MovementSetting
    {
        Arc,
        Line,
        //Wobble,
        Teleport,
    }
    [SerializeField] private MovementSetting currentMoveType = MovementSetting.Line;


    private CircleCollider2D circ;
    private Rigidbody2D rb;
    private List<GameObject> telePoints;
    private GameObject hyperSpawner;

    [SerializeField] private float MovementCountdown = 7f;
    [SerializeField] private float DespawnCountdown = 23f;
    [SerializeField] private float ColorCountdown = 3.5f;

    [SerializeField] private float speed = 3f;
    [SerializeField] private float bounceStrength = 5f;
    private Vector2 moveDirection;
    [SerializeField] private float arcSpeed = 2f;
    private float arcTorque;
    GameObject teleportTarget;

    private SpriteRenderer spriteRend;
    private bool isRed = true;

    void Start()
    {
        hyperSpawner = GameObject.Find("==== HyperSpawner =====");
        moveDirection = Random.insideUnitCircle.normalized;
        arcTorque = Random.Range(-2f, 2f);

        telePoints = new List<GameObject>();
        foreach (Transform child in hyperSpawner.transform)
            telePoints.Add(child.gameObject);

        teleportTarget = telePoints[Random.Range(0, telePoints.Count)];

        spriteRend = GetComponent<SpriteRenderer>();
        circ = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (DespawnCountdown > 0)
        {
            DespawnCountdown -= Time.deltaTime;
        }
        else 
        {
            hyperSpawner.GetComponent<HB_TriggerSpawner>().hyperInPlay = false;
            Destroy(this.gameObject);
        }
        UpdateTriggerMovement();
        slideColor();
    }

    private void UpdateTriggerMovement()
    {
        if (MovementCountdown > 0)
        {
            MovementCountdown -= Time.deltaTime;
        }
        else
        {
            currentMoveType = (MovementSetting)Random.Range(0, System.Enum.GetValues(typeof(MovementSetting)).Length);
        }
        switch (currentMoveType)
        {
            case MovementSetting.Arc:
                MoveArc();
                break;
            case MovementSetting.Line:
                MoveLine();
                break;
            //case MovementSetting.Wobble:
            //    MoveWobble();
            //    break;
            case MovementSetting.Teleport:
                MoveTeleport();
                break;
        }
    }

    #region Movement Types
    private void MoveArc()
    {
        //float around with a big sweeping arc
        Vector2 forward = transform.up;
        rb.AddForce(forward * arcSpeed);
        rb.AddTorque(arcTorque);
    }
    private void MoveLine()
    {
        //move in a straight line in a random direction
        rb.linearVelocity = moveDirection * speed;
    }
    private void MoveWobble()
    {
        //move in a shakey wobble
        //Debug.Log("skip wobble");
        //currentMoveType = (MovementSetting)Random.Range(0, System.Enum.GetValues(typeof(MovementSetting)).Length);
    }
    private void MoveTeleport()
    {
        if (teleportTarget == null) return;
        transform.position = teleportTarget.transform.position;
        currentMoveType = (MovementSetting)Random.Range(0, System.Enum.GetValues(typeof(MovementSetting)).Length - 1);
    }
    #endregion
    private void OnCollisionEnter2D(Collision2D collision)
    {        
        if (collision.gameObject.CompareTag("Terrain"))
        {
            Debug.Log("hit terrain");
            Vector2 normal = (transform.position - collision.transform.position).normalized;
            moveDirection = Vector2.Reflect(moveDirection, normal);

            arcTorque = Random.Range(-13f, 13f);
            teleportTarget = telePoints[Random.Range(0, telePoints.Count)];
            currentMoveType = (MovementSetting)Random.Range(0, System.Enum.GetValues(typeof(MovementSetting)).Length);
        }
        if (collision.gameObject.CompareTag("Dodgeball"))
        {
            //bounce off ball like pool cues where they kinda pop off each other
            Vector2 bounceDir = (transform.position - collision.transform.position).normalized;
            rb.AddForce(bounceDir * bounceStrength, ForceMode2D.Impulse);
        }
    }

    private void slideColor()
    {
        if (ColorCountdown > 0f)
        {
            ColorCountdown -= Time.deltaTime;
        }
        else
        {
            isRed = !isRed;
            spriteRend.color = isRed ? Color.red : Color.blue;
            ColorCountdown = 3.5f;
        }
    }
    
}
