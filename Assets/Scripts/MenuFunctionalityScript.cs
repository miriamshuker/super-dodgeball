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
    


    void Start()
    {
        _StartMenuHolder.SetActive(true);
        _SelectRoundHolder.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToStart()
    {
        _StartMenuHolder.SetActive(true);
        _SelectRoundHolder.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_StartButton);
    }

    public void GoToRoundSelect()
    {
        _StartMenuHolder.SetActive(false);
        _SelectRoundHolder.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_SingleRoundButton);

    }
}
