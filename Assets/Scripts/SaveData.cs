using System;

[Serializable]
public class SaveData
{
    public string date;     // 日付
    public string score;    // スコア
    public string comment;  // コメント
}

///////////////////////////////////////

[Serializable]
public class SaveDataTest
{
    public int[] a = new int[10];
    public SaveDataSetting saveDataSetting;    // 設定用データ
    public SaveDataScore[] SaveDataScore = new SaveDataScore[10];        // スコア用データ
}
[Serializable]
public class SaveDataSetting
{
    // setting
    public string bgmVolume;    // BGMボリューム
    public int bgmMute;         // BGMミュート(1:ミュート 0:非ミュート)
    public string seVolume;     // SEボリューム
    public int seMute;          // SEミュート(1:ミュート 0:非ミュー
}
[Serializable]
public class SaveDataScore
{
    // score
    public string date;     // 日付
    public string score;    // スコア
    public string comment;  // コメント
}