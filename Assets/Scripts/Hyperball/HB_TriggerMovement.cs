using System.Collections.Generic;
using UnityEngine;

public class HB_TriggerMovement : MonoBehaviour
{
    public enum MovementSetting
    {
        Arc,
        Line,
        Wobble,
        Teleport,
    }
    private CircleCollider2D circ;
    private Rigidbody2D rb;
    public List<GameObject> telePoints;
    public HB_TriggerSpawner hyperSpawner;
    [Header("Timers")]
    public float MovementCountdown = 7f;
    public float DespawnCountdown = 23f;
    public float ColorCountdown = 3.5f;

    private SpriteRenderer spriteRend;
    private bool isRed = true;

    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        circ = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        slideColor();
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
