using UnityEngine;

public class PlayerJoinScript : MonoBehaviour
{
    
    public Transform SpawnPoint1, SpawnPoint2;
    public GameObject Player1,  Player2;

    private void Awake()
    {
        GameObject player1 =  Instantiate(Player1, SpawnPoint1.position,SpawnPoint1.rotation);
        player1.name = "Player1";
        GameObject player2 =  Instantiate(Player2, SpawnPoint2.position,SpawnPoint2.rotation);
        player2.name = "Player2";
    }
}
