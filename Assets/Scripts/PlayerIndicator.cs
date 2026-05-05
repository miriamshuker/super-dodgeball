using UnityEngine;

public class PlayerIndicator : MonoBehaviour
{
    private void LateUpdate()
    {
        float parentY = transform.parent.eulerAngles.y;
        transform.localEulerAngles = new Vector3(0f, -parentY, 0f);
    }
}
