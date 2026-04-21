using UnityEngine;


[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class ScreenWrapping : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer mainRenderer;

    private SpriteRenderer[] ghosts;
    private float left, right, top, bottom;
    private float width, height;

    private float buffer = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainRenderer = GetComponent<SpriteRenderer>();

        Camera cam = Camera.main;

        Vector2 bottomLeft = cam.ScreenToWorldPoint(Vector2.zero);
        Vector2 topRight = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        left = bottomLeft.x;
        right = topRight.x;
        bottom = bottomLeft.y;
        top = topRight.y;

        width = right - left;
        height = top - bottom;

        CreateGhosts();
    }

    private void CreateGhosts()
    {
        ghosts = new SpriteRenderer[8];

        for (int i = 0; i < ghosts.Length; i++)
        {
            GameObject g = new GameObject("Ghost_" + i);
            g.transform.SetParent(transform);

            SpriteRenderer sr = g.AddComponent<SpriteRenderer>();
            sr.sprite = mainRenderer.sprite;
            sr.sortingLayerID = mainRenderer.sortingLayerID;
            sr.sortingOrder = mainRenderer.sortingOrder;

            // optional transparency
            sr.color = new Color(1f, 1f, 1f, 0.6f);

            ghosts[i] = sr;
        }
    }

    private void LateUpdate()
    {
        SyncGhostVisuals();

        Vector3 pos = transform.position;

        int index = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;

                Vector3 offset = new Vector3(x * width, y * height, 0);
                Vector3 ghostPos = pos + offset;

                bool shouldShow =
                    (x == -1 && pos.x < left + buffer) ||
                    (x == 1 && pos.x > right - buffer) ||
                    (y == -1 && pos.y < bottom + buffer) ||
                    (y == 1 && pos.y > top - buffer);

                ghosts[index].enabled = shouldShow;
                ghosts[index].transform.position = ghostPos;

                index++;
            }
        }
    }

    private void SyncGhostVisuals()
    {
        foreach (var g in ghosts)
        {
            g.sprite = mainRenderer.sprite;
            g.flipX = mainRenderer.flipX;
            g.flipY = mainRenderer.flipY;
            g.color = new Color(mainRenderer.color.r, mainRenderer.color.g, mainRenderer.color.b, 0.6f);
        }
    }

    private void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        // wrap logic (same as yours, just cleaner)
        if (screenPos.x <= 0 && rb.linearVelocity.x < 0)
            transform.position = new Vector2(right, transform.position.y);
        else if (screenPos.x >= Screen.width && rb.linearVelocity.x > 0)
            transform.position = new Vector2(left, transform.position.y);

        if (screenPos.y >= Screen.height && rb.linearVelocity.y > 0)
            transform.position = new Vector2(transform.position.x, bottom);
        else if (screenPos.y <= 0 && rb.linearVelocity.y < 0)
            transform.position = new Vector2(transform.position.x, top);
    }
}
