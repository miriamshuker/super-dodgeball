using UnityEngine;

public class DodgeballScript : MonoBehaviour
{
    
    public bool _isLive = false;
    public string originPlayer = "";

 
    //Reset to "not live" and remove any "origin player" when im thrown
    void OnCollisionEnter2D(Collision2D col)
    {
       if(col.gameObject.CompareTag("Ground"))
        {
            _isLive = false;
            originPlayer = "";
        }
    }
}
