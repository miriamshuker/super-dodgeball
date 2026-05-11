using UnityEngine;
using UnityEngine.EventSystems;

public class MenuFunctionalityScript : MonoBehaviour
{

    [SerializeField]
    private GameObject _StartButton;
    [SerializeField]
    private GameObject _SingleRoundButton;

    
    [SerializeField]
    private GameObject _StartMenuHolder;
    [SerializeField]
    private GameObject _SelectRoundHolder;
    [SerializeField]
    private EventSystem myEventSystem;
    
    [SerializeField]
    private GameObject eventSystemGO;
    


    void Start()
    {
        _StartMenuHolder.SetActive(true);
        _SelectRoundHolder.SetActive(false);
        myEventSystem.SetSelectedGameObject(_StartButton);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToStart()
    {
        _StartMenuHolder.SetActive(true);
        _SelectRoundHolder.SetActive(false);
        myEventSystem.SetSelectedGameObject(_StartButton);
    }

    public void GoToRoundSelect()
    {
        _StartMenuHolder.SetActive(false);
        _SelectRoundHolder.SetActive(true);
        myEventSystem.SetSelectedGameObject(_SingleRoundButton);

    }

    public void RestartMenu()
    {
        Debug.Log("Restarted Menu");
        eventSystemGO = GameObject.Find("EventSystem");
        myEventSystem = eventSystemGO.GetComponent<EventSystem>();
        _StartMenuHolder.SetActive(true);
        _SelectRoundHolder.SetActive(false);
        myEventSystem.SetSelectedGameObject(_StartButton);
    }
}
