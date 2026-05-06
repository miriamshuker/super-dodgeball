using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;

public class RoundManagerScript : MonoBehaviour
{
#region Setting Up Instance
    public static RoundManagerScript Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
#endregion

    public enum GameState
    {
        menu,
        roundStart,
        roundLoop,
        roundWin,
        gameWin
    }

    public GameState currentState;

    [Header("Refs")]
    [SerializeField] HealthUIManager healthUIManager;
    [SerializeField] RoundWinUIManager p1RoundWinUIManager, p2RoundWinUIManager;

    [Header("Round Data")]
    public DodgeballInputs _input;

    [Header("Game Info")] 
    //things that can be changed from menu selection
    public int pointsToWin = 1;
    public int playerStartHealth = 5;

    //information about the current game
    public int p1CurrHealth;
    public int p2CurrHealth;
    public int p1RoundWins = 0;
    public int p2RoundWins = 0;

    private string lastPlayerWin;
    
    [Header("UI")]
    [SerializeField] private GameObject menuUI, startUI, loopUI, rWinUI, gWinUI;
    private bool occuring;

    [Header("List of Scene Names")]
    public List<string> sceneNames;

    void Start()
    {
        _input = new DodgeballInputs();
        _input.DodgeballPlayer.Disable();
        _input.UI.Enable();

        StateManager(GameState.menu);
    }

    private void StateManager(GameState stateChange)
    {
        occuring = false;
        currentState = stateChange;
    }

    public void SetPointsToWin(int pointsNeeded)
    {
        pointsToWin = pointsNeeded;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(sceneNames[Random.Range(0, sceneNames.Count)]);
        _input.DodgeballPlayer.Enable();
        _input.UI.Disable();

        StateManager(GameState.roundStart);
    }

    private void Update()
    {
        switch (currentState) //holy state machine
        {
            case GameState.menu:
                if(!occuring && SceneManager.GetActiveScene().name != "Menu") {occuring = true; Menu();}
                break;
            case GameState.roundStart:
                if(!occuring) {occuring = true; StartCoroutine(RoundStart());}
                break;
            case GameState.roundLoop:
                if(!occuring) {occuring = true; StartCoroutine(RoundLoop());}
                break;
            case GameState.roundWin:
                if(!occuring) {occuring = true; StartCoroutine(RoundWin());}
                break;
            case GameState.gameWin:
                if(!occuring) {occuring = true; StartCoroutine(GameWin());}
                break;
        }
    }

#region  Game States

    private void Menu()
    {
        ClearUI();
        ResetGame();
        loopUI.SetActive(false);
        SceneManager.LoadScene("Menu");
        menuUI.SetActive(true);
        p1RoundWinUIManager.SetWins();
        p2RoundWinUIManager.SetWins();
    }

    private IEnumerator RoundStart()
    {
        ClearUI();
        p1RoundWinUIManager.SetupRoundUI();
        p2RoundWinUIManager.SetupRoundUI();
        startUI.SetActive(true);
        loopUI.SetActive(true);
        yield return StartCoroutine(SetPlayerHealth());
        StateManager(GameState.roundLoop);
    }

    private IEnumerator RoundLoop()
    {
        ClearUI();
        yield return new WaitForSeconds(1);
    }

    private IEnumerator RoundWin()
    {
        ClearUI();
        p1RoundWinUIManager.CheckIfIWon(lastPlayerWin, 1);
        p2RoundWinUIManager.CheckIfIWon(lastPlayerWin, 2);
        rWinUI.SetActive(true);
        yield return new WaitForSeconds(1);
        StartGame();
    }   

    private IEnumerator GameWin()
    {
        ClearUI();
        p1RoundWinUIManager.CheckIfIWon(lastPlayerWin, 1);
        p2RoundWinUIManager.CheckIfIWon(lastPlayerWin, 2);
        gWinUI.SetActive(true);
        yield return new WaitForSeconds(3);
        StateManager(GameState.menu);
    }

    private IEnumerator SetPlayerHealth()
    {
        yield return StartCoroutine(healthUIManager.RoundStartUISetup());
        p1CurrHealth = playerStartHealth;
        p2CurrHealth = playerStartHealth;
    }

    private void ClearUI()
    {
        menuUI.SetActive(false); startUI.SetActive(false); /*loopUI.SetActive(false);*/ rWinUI.SetActive(false); gWinUI.SetActive(false);
    }

    private void ResetGame()
    {
        p1RoundWins = 0;
        p2RoundWins = 0;
        p1CurrHealth = 0;
        p2CurrHealth = 0;
        lastPlayerWin = "";
    }

#endregion

#region Player Damage Section

    public void PlayerDamaged(string player)
    {
        if(player == "Player1")
        {
            p1CurrHealth--;
            if(p1CurrHealth == 0) UpdatePlayerPoints(player);
            healthUIManager.UpdatePlayerHealthUI(player, p1CurrHealth);
        }
        if(player == "Player2")
        {
            p2CurrHealth--;
            if(p2CurrHealth == 0) UpdatePlayerPoints(player);
            healthUIManager.UpdatePlayerHealthUI(player, p2CurrHealth);
        }
    }

#endregion

#region Round Win Detection

    public void UpdatePlayerPoints(string player) //I hate that im using strings. very bad practice but it works ig
    {
        if(player == "Player1")
        {
            p2RoundWins++;
            lastPlayerWin = "Player2";
        }
        else if(player == "Player2")
        {
            p1RoundWins++;
            lastPlayerWin = "Player1";
        }

        if(p1RoundWins == pointsToWin || p2RoundWins == pointsToWin)
        {
            StateManager(GameState.gameWin);
        }
        else
        {
            StateManager(GameState.roundWin);
        }
    }

#endregion

    //Commenting out old architecture
    /*public void UpdateP1Points(int point)
    {
        p1_wins += point;
        CheckWin();
    }

    public void UpdateP2Points(int point)
    {
        p2_wins += point;
        CheckWin();
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

    public IEnumerator NextRound()
    {
        nextRoundUI.SetActive(true);
        yield return new WaitForSeconds(2);
        nextRoundUI.SetActive(false);
        StartRound();
    } */

}
