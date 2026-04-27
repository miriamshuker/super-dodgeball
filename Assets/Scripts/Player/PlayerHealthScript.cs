using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{

    public int maxLives = 5;
    public int currentLives = 5;
    public GameObject myHealthUIPrefab;
    public GameObject myLOSSPrefab;
    private GameObject myHealthUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myHealthUI = Instantiate(myHealthUIPrefab);
        myHealthUI.transform.SetParent(GameObject.Find("Canvas").transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HealthLoss()
    {
        if(currentLives > 0)
        {
            myHealthUI.transform.GetChild(currentLives-1).gameObject.SetActive(false);  
            currentLives -= 1;
            if(currentLives <= 0)
            {
                GameObject loseUI = Instantiate(myLOSSPrefab);
                loseUI.transform.SetParent(GameObject.Find("Canvas").transform, false);
            }
        }
    }
}
