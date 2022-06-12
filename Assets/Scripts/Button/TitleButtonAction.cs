using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtonAction : MonoBehaviour
{
    [SerializeField] GameObject UiSettings; // UiSettingsへの参照

    void Reset(){
        UiSettings = GameObject.Find("UiSettings");
    }

    // title
    public void onClick_Start(){
        SceneManager.LoadScene("Game");
    }
    public void onClick_Score(){
        SceneManager.LoadScene("Score");
    }
    public void onClick_Option(){
        UiSettings.SetActive(true);
    }
    public void onClick_Close(){
        UiSettings.SetActive(false);
    }
    // score?
    public void onClick_Title(){
        SceneManager.LoadScene("Title");
    }
}
