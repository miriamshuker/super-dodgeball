using UnityEngine;
public class HB_HyperBeam : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 20f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Dodgeball"))
        {
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Player"))
        {
            RoundManagerScript.Instance.PlayerDamaged(other.name);
            RoundManagerScript.Instance.PlayerDamaged(other.name);

            // Push player away from the beam's parent (the shooter)
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Debug.Log("knockback target");
                Vector2 knockbackDir = ((Vector2)other.transform.position - (Vector2)transform.parent.position).normalized;
                playerRb.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}
