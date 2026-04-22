using UnityEngine;
using System.Collections;


public class DodgeballScript : MonoBehaviour
{
    
    public bool _isLive = false;
    public string originPlayer = "";
    public float bounce = .07f;


    private Rigidbody2D rb;
    private CircleCollider2D circle;
    private float radius;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();

        radius = circle.radius * Mathf.Max(transform.localScale.x, transform.localScale.y);
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
    }
}
