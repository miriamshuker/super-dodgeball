using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class RoundManagerScript : MonoBehaviour
{

    
    [Header("Round Data")]
    [SerializeField] 
    public DodgeballInputs _input;
    public int pointsToWin = 1;
    public int p1_wins = 0;
    public int p2_wins = 0;

    
    [Header("UI")]
    public GameObject p1WinsUI;
    public GameObject p2WinsUI;
    public GameObject nextRoundUI;


    [Header("List of Scene Names")]
    public List<string> sceneNames;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        _input = new DodgeballInputs();
        _input.DodgeballPlayer.Disable();
        _input.UI.Enable();
    }

    

    public void UpdateP1Points(int point)
    {
        p1_wins += point;
        CheckWin();
    }

    public void UpdateP2Points(int point)
    {
        p2_wins += point;
        CheckWin();
    }

    public void SetPointsToWin(int pointsNeeded)
    {
        pointsToWin = pointsNeeded;
    }

    public void CheckWin()
    {
        if(p1_wins >= pointsToWin)
        {
            p1WinsUI.SetActive(true);
            _input.DodgeballPlayer.Disable();
        }
        else if (p2_wins >= pointsToWin)
        {
            p2WinsUI.SetActive(true);
            _input.DodgeballPlayer.Disable();
        }
        else
        {
            _input.DodgeballPlayer.Disable();
            StartCoroutine(NextRound());
        }
    }

    public void StartRound()
    {
        SceneManager.LoadScene(sceneNames[Random.Range(0, sceneNames.Count)]);
        _input.DodgeballPlayer.Enable();
        _input.UI.Disable();
    }

    public IEnumerator NextRound()
    {
        
        nextRoundUI.SetActive(true);
        yield return new WaitForSeconds(2);
        nextRoundUI.SetActive(false);
        StartRound();
    }

}
