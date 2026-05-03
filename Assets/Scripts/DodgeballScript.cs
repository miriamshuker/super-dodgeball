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
    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log("hit something: " + col.gameObject.name + " tag: " + col.gameObject.tag);

        Vector2 rbVelo = col.relativeVelocity;
        //Debug.Log("relative velo= " + rbVelo);
        if (col.gameObject.CompareTag("Terrain"))
        {
            Vector2 hitTerrain = col.contacts[0].normal;
            if (rbVelo.magnitude > 60f)
            {
                //Debug.Log("fast enough to bounce");
                if (hitTerrain.y > .5f)
                {
                    // Floor contact
                    _isLive = false;
                    originPlayer = "";
                    StartCoroutine(vertBounce(rbVelo));
                }
                else if (Mathf.Abs(hitTerrain.x) > .5f)
                {
                    // Side wall contact
                    //Debug.Log("horizontal contact detected");
                    StartCoroutine(horiBounce(rbVelo));
                }
                else
                {
                    // Roof contact & others
                    StartCoroutine(vertBounce(rbVelo));
                }
            }
        }
        if (col.gameObject.CompareTag("Player") && col.gameObject.name != originPlayer)
        {
            StartCoroutine(playerBounce(rbVelo));
        }
    }
    private IEnumerator horiBounce(Vector2 bounceVelo)
    {
        anim.SetBool("landed", true);
        rb.linearVelocity = new Vector2(bounceVelo.x * bounce, rb.linearVelocity.y);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("landed", false);
    }
    private IEnumerator vertBounce(Vector2 bounceVelo)
    {
        anim.SetBool("landed", true);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceVelo.y * bounce);
        yield return new WaitForSeconds(0.1f);
        anim.SetBool("landed", false);
    }
    private IEnumerator playerBounce(Vector2 bounceVelo)
    {
        Debug.Log("player bounce");
        yield return new WaitForSeconds(0.1f);
    }
    private void Animate()
    {
        totalSpeed = rb.linearVelocity.magnitude;
        dirVec = rb.linearVelocity;

        movingAng = Mathf.Atan2(dirVec.y, dirVec.x) * Mathf.Rad2Deg;
        if (totalSpeed > 10)
        {
            transform.eulerAngles = new Vector3(0, 0, movingAng - 90f);
        }
        anim.SetFloat("speed", Mathf.Abs(totalSpeed));
    }
}