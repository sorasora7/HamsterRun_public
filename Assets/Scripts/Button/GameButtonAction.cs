using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameButtonAction : MonoBehaviour
{
    [SerializeField] GameObject UiPause;    // UiPauseオブジェクトへの参照
    [SerializeField] GameObject bgmSlider;
    [SerializeField] GameObject seSlider;
    [SerializeField] Sprite volumeOn;
    [SerializeField] Sprite volumeMute;


    bool isMute = false;

    void Reset(){
        UiPause = GameObject.Find("UiPause");
    }

    public void onClick_Retry(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void onClick_BackToMenu(){
        SceneManager.LoadScene("Title");
    }

    public void onClick_ClosePause(){
        UiPause.SetActive(false);
        Time.timeScale = 1;
    }

    public void onClick_Pause(){
        UiPause.SetActive(true);
        Time.timeScale = 0;
    }

    public void onClick_BgmMute(){
        Slider slider = bgmSlider.GetComponent<Slider>();
        Image btnImage = GetComponent<Image>();

        if (isMute) {
            // ミュート解除処理
            btnImage.sprite = volumeOn;
            slider.interactable = true;
            isMute = false;
        } else {
            // ミュート処理
            btnImage.sprite = volumeMute;
            slider.interactable = false;
            isMute = true;
        }
        AudioManager.Instance.ChangeBgmMute(isMute);
    }
    public void onClick_SEMute(){
        Slider slider = seSlider.GetComponent<Slider>();
        Image btnImage = GetComponent<Image>();

        if (isMute) {
            // ミュート解除処理
            btnImage.sprite = volumeOn;
            slider.interactable = true;
            isMute = false;
        } else {
            // ミュート処理
            btnImage.sprite = volumeMute;
            slider.interactable = false;
            isMute = true;
        }
        AudioManager.Instance.ChangeBgmMute(isMute);
    }
}
