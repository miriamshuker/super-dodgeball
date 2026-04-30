using UnityEngine;
using System.Collections;

public class PlayerHealthScript : MonoBehaviour
{

    public int maxLives = 5;
    public int currentLives = 5;
    public GameObject myHealthUIPrefab;
    public GameObject myLOSSPrefab;
    private GameObject myHealthUI;
    private GameObject roundManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myHealthUI = Instantiate(myHealthUIPrefab);
        myHealthUI.transform.SetParent(GameObject.Find("Canvas").transform, false);
        roundManager = GameObject.Find("RoundManager");
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
                Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                StartCoroutine(NextRoundCoroutine());
            }
        }
    }

    public IEnumerator NextRoundCoroutine()
    {
        Debug.Log("EEEEEEEEEEEEEEEEEEEEEEEEEE");
        GameObject loseUI = Instantiate(myLOSSPrefab);
        loseUI.transform.SetParent(GameObject.Find("Canvas").transform, false);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(2);


        Destroy(loseUI);

        if (this.name == "Player1")
        {
            roundManager.GetComponent<RoundManagerScript>().UpdateP2Points(1);
        }else if (this.name == "Player2")
        {
            roundManager.GetComponent<RoundManagerScript>().UpdateP1Points(1);
        }
    }
}
