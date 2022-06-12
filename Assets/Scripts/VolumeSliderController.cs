using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    public enum VolumeType
    {
        SE,
        BGM
    }
    [SerializeField] VolumeType volumeType;
    [SerializeField] GameObject Text;
    Text text_;
    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        text_ = Text.GetComponent<Text>();
        slider = GetComponent<Slider>();

        if (volumeType == VolumeType.SE) {
            slider.value = AudioManager.seVolume;
        } else {
            slider.value = AudioManager.bgmVolume;
        }
        text_.text = "" + slider.value;
    }

    public void VolumeChange(){
        text_.text = "" + slider.value;
        if (volumeType == VolumeType.SE) {
            AudioManager.Instance.ChangeSEVolume(slider.value);
        } else {
            AudioManager.Instance.ChangeBgmVolume(slider.value);
        }
    }
}
