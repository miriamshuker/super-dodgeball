using Unity.VisualScripting;
using UnityEngine;

public class PlayerLockingScript : MonoBehaviour
{
    private bool occured;

    private PlayerMovement pm;

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if(RoundManagerScript.Instance.currentState == RoundManagerScript.GameState.roundLoop && pm.enabled != true)
        {
            pm.enabled = true;
        }
        else if (RoundManagerScript.Instance.currentState != RoundManagerScript.GameState.roundLoop && pm.enabled)
        {
            pm.enabled = false;
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }
}
