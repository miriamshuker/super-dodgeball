using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthUIManager : MonoBehaviour
{
    [Header("Sequencing")]
    [SerializeField] float delayBetweenHeartIncrease = 0.5f;

    [SerializeField] HorizontalLayoutGroup p1, p2;

    [SerializeField] GameObject p1PrimaryHeartPref, p2PrimaryHeartPref;
    [SerializeField] GameObject p1HeartPref, p2HeartPref;
    [SerializeField] private List<GameObject> p1Hearts = new(), p2Hearts = new();

    private int additionalHearts;

    public IEnumerator RoundStartUISetup()
    {
        if(RoundManagerScript.Instance.p1CurrHealth == 0)
        {
            foreach (GameObject obj in p1Hearts)
            {
                Destroy(obj);
            }
            p1Hearts.Clear();

            GameObject primaryP1 = Instantiate(p1PrimaryHeartPref, p1.transform);
            yield return new WaitForSeconds(delayBetweenHeartIncrease);
            p1Hearts.Add(primaryP1);

            additionalHearts = RoundManagerScript.Instance.playerStartHealth - 1;
            for(int i = 0; i < additionalHearts; i++)
            {
                GameObject p1heart = Instantiate(p1HeartPref, p1.transform);
                p1Hearts.Add(p1heart);
                yield return new WaitForSeconds(delayBetweenHeartIncrease);
            }
        }
        if(RoundManagerScript.Instance.p2CurrHealth == 0)
        {
            foreach(GameObject obj in p2Hearts)
            {
                Destroy(obj);
            }
            p2Hearts.Clear();

            GameObject primaryP2 = Instantiate(p2PrimaryHeartPref, p2.transform);
            yield return new WaitForSeconds(delayBetweenHeartIncrease);
            p2Hearts.Add(primaryP2);

            additionalHearts = RoundManagerScript.Instance.playerStartHealth - 1;
            for(int i = 0; i < additionalHearts; i++)
            {
                GameObject p2heart = Instantiate(p2HeartPref, p2.transform);
                p2Hearts.Add(p2heart);
                yield return new WaitForSeconds(delayBetweenHeartIncrease);
            }
        }
    }

    public void UpdatePlayerHealthUI(string player, int pos)
    {
        if(player == "Player1")
        {
            GameObject temp = p1Hearts[pos];
            p1Hearts.RemoveAt(pos);
            Destroy(temp);
        }
        else if(player == "Player2")
        {
            GameObject temp = p2Hearts[pos];
            p2Hearts.RemoveAt(pos);
            Destroy(temp);
        }
    }
}
