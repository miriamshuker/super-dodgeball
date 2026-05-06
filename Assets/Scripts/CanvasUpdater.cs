using UnityEngine;

public class CanvasUpdater : MonoBehaviour
{
    void Update()
    {
        if(this.GetComponent<Canvas>().worldCamera == null)
        {
            this.GetComponent<Canvas>().worldCamera = CameraController.Instance.GetComponent<Camera>();
        }
    }
}
