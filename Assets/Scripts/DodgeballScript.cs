using UnityEngine;
using System.Collections;
using UnityEngine.Scripting.APIUpdating;


public class DodgeballScript : MonoBehaviour
{
    
    public bool _isLive = false;
    public string originPlayer = "";
    public float bounce = .07f;

    private Rigidbody2D rb;
    private CircleCollider2D circle;
    private float radius;

    //Animation Polish
    private Animator anim;
    private float movingAng;
    private Vector2 dirVec;
    private float totalSpeed;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();

        radius = circle.radius * Mathf.Max(transform.localScale.x, transform.localScale.y);
    }

    private void Update()
    {
        Animate();
    }

    //Reset to "not live" and remove any "origin player" when im thrown
    void OnCollisionEnter2D(Collision2D col)
    {
        Vector2 rbVelo = col.relativeVelocity;
        Debug.Log("relative velo= " + rbVelo);

        if (col.gameObject.CompareTag("Terrain"))
        {
            Vector2 hitTerrain = col.contacts[0].normal;

            //rb.linearVelocity = reflectDir * bounce;

            if (hitTerrain.y > .5f)
            {
                _isLive = false;
                originPlayer = "";
                StartCoroutine(vertBounce(rbVelo));
            }
            if (hitTerrain.y < .5f)
            {
                StartCoroutine(vertBounce(rbVelo));
            }

            anim.SetBool("landed", true);
        }
    }

    //private IEnumerator horiBounce()
    //{
    //    horiSpeed *= -.5f;
    //    yield return new WaitForSeconds(1);
    //}

    private IEnumerator vertBounce(Vector2 bounceVelo)
    {
        Debug.Log("start vert bounce");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceVelo.y * bounce);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("landed", false);
    }

    private void Animate()
    {
        totalSpeed = rb.linearVelocity.magnitude;
        dirVec = rb.linearVelocity;
                        
        movingAng = Mathf.Atan2(dirVec.y, dirVec.x) * Mathf.Rad2Deg;
        if(totalSpeed > 10)
        {
            transform.eulerAngles = new Vector3(0, 0, movingAng - 90f);
        }

        anim.SetFloat("speed", Mathf.Abs(totalSpeed));
    }
}
