using UnityEngine;

public class RoundWinUIManager : MonoBehaviour
{
    [SerializeField] string me;

    [SerializeField] GameObject[] roundWinObjs;

    private int myWins = 0;

    public void SetupRoundUI()
    {
        for (int  i = 0; i < RoundManagerScript.Instance.pointsToWin; i++)
        {
            roundWinObjs[i].gameObject.SetActive(true);
        }
    }

    public void SetWins()
    {
        myWins = 0;
    }

    public void CheckIfIWon(string lastPlayerWin, int p)
    {
        if(lastPlayerWin == me)
        {
            roundWinObjs[myWins].GetComponentInChildren<Animator>().Play("Indicator_RoundWin");
            myWins++;
        }
    }
}
