using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HB_TriggerMovement : MonoBehaviour
{
    public enum MovementSetting
    {
        Arc,
        Line,
        Wobble,
        Jitter,
        Teleport,
    }
    [SerializeField] private MovementSetting currentMoveType = MovementSetting.Line;


    private CircleCollider2D circ;
    private Rigidbody2D rb;
    private List<GameObject> telePoints;
    private List<GameObject> players = new List<GameObject>();
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
    private float wobbleTime = 0f;
    [SerializeField] private float wobbleFrequency = 2f;
    [SerializeField] private float wobbleAmplitude = 3f;
    private float jitterInterval = 0f;
    [SerializeField] private float jitterForce = 6f;
    [SerializeField] private float jitterRate = 0.08f;

    public bool claimed = false;

    private SpriteRenderer spriteRend;
    private bool isRed = true;

    void Start()
    {
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
            players.Add(p);
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
            case MovementSetting.Wobble:
                MoveWobble();
                break;
            case MovementSetting.Jitter:
                MoveJitter();
                break;
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
        // Smooth sine-wave weave while travelling forward
        wobbleTime += Time.deltaTime;
        float sine = Mathf.Sin(wobbleTime * wobbleFrequency) * wobbleAmplitude;
        Vector2 perp = new Vector2(-moveDirection.y, moveDirection.x); // perpendicular to travel
        rb.linearVelocity = (moveDirection * speed) + (perp * sine);
    }
    private void MoveJitter()
    {
        // Randomly kicks in a new direction every jitterRate seconds
        jitterInterval -= Time.deltaTime;
        if (jitterInterval <= 0f)
        {
            Vector2 randomKick = Random.insideUnitCircle.normalized;
            rb.AddForce(randomKick * jitterForce, ForceMode2D.Impulse);
            jitterInterval = jitterRate;
        }
    }
    private void MoveTeleport()
    {
        teleportTarget = GetSafeTeleportTarget();
        if (teleportTarget == null) return;

        transform.position = teleportTarget.transform.position;
        currentMoveType = (MovementSetting)Random.Range(0, System.Enum.GetValues(typeof(MovementSetting)).Length - 1);
    }


    private GameObject GetSafeTeleportTarget()
    {
        float minPlayerDistance = 8f;
        int maxAttempts = 10;

        for (int i = 0; i < maxAttempts; i++)
        {
            GameObject candidate = telePoints[Random.Range(0, telePoints.Count)];
            bool tooClose = false;

            foreach (GameObject player in players)
            {
                if (player == null) continue;
                if (Vector2.Distance(candidate.transform.position, player.transform.position) < minPlayerDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose) return candidate;
        }

        Debug.LogWarning("No safe teleport point found, using random fallback");
        return telePoints[Random.Range(0, telePoints.Count)];
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {        
        if (collision.gameObject.CompareTag("Terrain"))
        {
            Vector2 normal = (transform.position - collision.transform.position).normalized;
            moveDirection = Vector2.Reflect(moveDirection, normal);

            arcTorque = Random.Range(-13f, 13f);
            teleportTarget = telePoints[Random.Range(0, telePoints.Count)];
            currentMoveType = (MovementSetting)Random.Range(0, System.Enum.GetValues(typeof(MovementSetting)).Length);
        }
        if (collision.gameObject.CompareTag("Dodgeball"))
        {
            Vector2 bounceDir = (transform.position - collision.transform.position).normalized;
            rb.AddForce(bounceDir * bounceStrength, ForceMode2D.Impulse);
            collision.rigidbody.AddForce(-bounceDir * bounceStrength, ForceMode2D.Impulse);
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
