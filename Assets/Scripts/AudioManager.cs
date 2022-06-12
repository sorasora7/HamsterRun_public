using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    enum SoundConfig
    {
        VOL_MIN_VAL = 0,
        VOL_MAX_VAL = 100
    }

    [SerializeField] AudioMixer Mixer;

    public static int seVolume{get; private set;}
    public static int bgmVolume{get; private set;}
    public static bool seVolumeMute{get; private set;}
    public static bool bgmVolumeMute{get; private set;}

    void Start(){
        // あとでセーブデータから読み込み
        seVolume = (int)SoundConfig.VOL_MAX_VAL;
        bgmVolume = (int)SoundConfig.VOL_MAX_VAL;
        seVolumeMute = false;
        bgmVolumeMute = false;
        
        Mixer.SetFloat("seVolume", ConvertVolumeToDecibel(seVolume));
        Mixer.SetFloat("bgmVolume", ConvertVolumeToDecibel(bgmVolume));
    }

    public void ChangeSEVolume(float vol){
        seVolume = (int)vol;
        Mixer.SetFloat("seVolume", ConvertVolumeToDecibel(vol));
    }

    public void ChangeBgmVolume(float vol){
        bgmVolume = (int)vol;
        Mixer.SetFloat("bgmVolume", ConvertVolumeToDecibel(vol));
    }

    public void ChangeSEMute(bool mute){
        seVolumeMute = mute;
        float vol = mute ? 0 : seVolume;
        Mixer.SetFloat("seVolume", ConvertVolumeToDecibel(vol));
    }

    public void ChangeBgmMute(bool mute){
        bgmVolumeMute = mute;
        float vol = mute ? 0 : bgmVolume;
        Mixer.SetFloat("bgmVolume", ConvertVolumeToDecibel(vol));
    }

    // 要注意な点はここで設定する値は 0 ~ 1 の値ではなくdB単位( -80 ~ 0 )なので直感的な0 ~ 1での制御をし
    // たい場合は別途 0 ~ 1 をデシベルに変換する関数を作成する必要がある
    // 0 ~ 1の値をdB( デシベル )に変換.
    private float ConvertVolumeToDecibel(float vol){
        var vol_ = Mathf.Clamp(vol, (float)SoundConfig.VOL_MIN_VAL, (float)SoundConfig.VOL_MAX_VAL);    
        // normalize = (zi - min(x)) /  (max(x) - min(x))
        var normalize = (vol_ - (float)SoundConfig.VOL_MIN_VAL) / (SoundConfig.VOL_MAX_VAL - SoundConfig.VOL_MIN_VAL);
        // AudioMix min: -80db, max: 0db
        var masterVolume = normalize * 80 + (-80);
        return masterVolume;
    }
}
