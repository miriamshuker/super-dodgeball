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

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip buttonSound;
    


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
        audioSource.PlayOneShot(buttonSound);
        _StartMenuHolder.SetActive(true);
        _SelectRoundHolder.SetActive(false);
        myEventSystem.SetSelectedGameObject(_StartButton);
    }

    public void GoToRoundSelect()
    {
        audioSource.PlayOneShot(buttonSound);
        _StartMenuHolder.SetActive(false);
        _SelectRoundHolder.SetActive(true);
        myEventSystem.SetSelectedGameObject(_SingleRoundButton);

    }

    public void RestartMenu()
    {
        audioSource.PlayOneShot(buttonSound);
        Debug.Log("Restarted Menu");
        eventSystemGO = GameObject.Find("EventSystem");
        myEventSystem = eventSystemGO.GetComponent<EventSystem>();
        _StartMenuHolder.SetActive(true);
        _SelectRoundHolder.SetActive(false);
        myEventSystem.SetSelectedGameObject(_StartButton);
    }
}
