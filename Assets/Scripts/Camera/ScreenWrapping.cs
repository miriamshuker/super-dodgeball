using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class ScreenWrapping : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();


        Vector2 bottomLeft = Camera.main.ScreenToWorldPoint(Vector2.zero);
        Vector2 topRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        float width = topRight.x - bottomLeft.x;
        float height = topRight.y - bottomLeft.y;
    }

    private void Update()
    {
        //get position of object in pixels
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        //get right side of screen in world units
        float rightSideOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
        float leftSideOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).x;        
        
        float topOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y;
        float bottomOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(0f,0f)).y;




        //left
        if(screenPos.x <= 0 && rb.linearVelocity.x < 0)
        {
            transform.position = new Vector2(rightSideOfScreen, transform.position.y);
        }
        //right
        else if(screenPos.x >= Screen.width && rb.linearVelocity.x > 0)
        {
            transform.position = new Vector2(leftSideOfScreen, transform.position.y);
        }
        //up
        else if (screenPos.y >= Screen.height && rb.linearVelocity.y > 0)
        {
            transform.position = new Vector2(transform.position.x, bottomOfScreen);
        }
        //down
        else if(screenPos.y <= 0 && rb.linearVelocity.y < 0)
        {
            transform.position = new Vector2(transform.position.x, topOfScreen);
        }


    }
}
