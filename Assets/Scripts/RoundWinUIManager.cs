using UnityEngine;

public class RoundWinUIManager : MonoBehaviour
{
    [SerializeField] int player;
    private int myWins;

    private RoundManagerScript _roundManager;
    [SerializeField] GameObject[] roundWinObjs;

    private void Awake()
    {
        _roundManager = FindAnyObjectByType<RoundManagerScript>();
    }

    private void Start()
    {
        if(_roundManager != null)
        {
            CheckWins();

            for (int  i = 0; i < _roundManager.pointsToWin; i++)
            {
                roundWinObjs[i].gameObject.SetActive(true);
            }

            SetUI();
        }
    }

    private void Update()
    {
        if (_roundManager != null)
        {
            CheckWins();
        }
    }

    private void CheckWins()
    {
        if (player == 1)
        {
            myWins = _roundManager.p1_wins;
        }
        else if (player == 2)
        {
            myWins = _roundManager.p2_wins;
        }
    }

    private void LateUpdate()
    {
        if (player == 1 && myWins != _roundManager.p1_wins)
        {
            UpdateUI(myWins, player);
        }
        else if (player == 2 && myWins != _roundManager.p2_wins)
        {
            UpdateUI(myWins, player);
        }
    }

    private void SetUI()
    {
        for(int i = 0; i < myWins; i++)
        {
            roundWinObjs[i].GetComponentInChildren<Animator>().Play("Indicator_Full");
        }
    }

    private void UpdateUI(int wins, int player)
    {
        if (player == this.player)
        {
            roundWinObjs[wins].GetComponentInChildren<Animator>().Play("Indicator_RoundWin");
        }
    }
}
