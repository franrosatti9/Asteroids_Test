using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private Options options;
    [SerializeField] private GameObject pauseContent;
    
    private void OnEnable()
    {
        options.OnCloseOptions += ShowPauseContent;
    }
    
    private void OnDisable()
    {
        options.OnCloseOptions -= ShowPauseContent;
    }

    public void OnPressContinue()
    {
        GameManager.Instance.ResumeGame();
        HidePauseScreen();
    }

    public void OnPressOptions()
    {
        pauseContent.SetActive(false);
        options.ShowScreen();
    }
    
    public void OnPressMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    public void ShowPauseContent()
    {
        pauseContent.SetActive(true);
    }

    public void ShowPauseScreen()
    {
        gameObject.SetActive(true);
    }

    public void HidePauseScreen()
    {
        if(options.gameObject.activeSelf) options.HideScreen();
        gameObject.SetActive(false);
    }
    

}
