using UnityEngine;

public class DB_Outline : MonoBehaviour
{
    [SerializeField] DodgeballScript dodgeballScript;

    private SpriteRenderer sprite;

    [SerializeField] Color p1Color;
    [SerializeField] Color p2Color;
    [SerializeField] Color idleColor;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (dodgeballScript.originPlayer == "Player1")
        {
            sprite.color = p1Color;
        }
        else if (dodgeballScript.originPlayer == "Player2")
        {
            sprite.color = p2Color;
        }
        else
        {
            sprite.color = idleColor;
        }
    }
}
