using UnityEngine;
using UnityEngine.UI;

public class ValueDetermine : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    private Image image;
    private int winP;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        winP = RoundManagerScript.Instance.pointsToWin; //code so cooked im ctrl c ctrl v 90 % god shoot me in the brain

        if (winP == 1)
        {
            image.sprite = sprites[0];
            GetComponentInParent<Image>().enabled = true;
        }
        else if (winP == 2)
        {
            image.sprite = sprites[1];
            GetComponentInParent<Image>().enabled = true;
        }
        else if (winP == 3)
        {
            image.sprite = sprites[2];
            GetComponentInParent<Image>().enabled = true;
        }
        else
        {
            image.sprite = null;
            GetComponentInParent<Image>().enabled = false;
        }
    }
}
