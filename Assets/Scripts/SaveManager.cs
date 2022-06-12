using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveManager : MonoBehaviour
{
    string filePath;    // セーブデータのファイルパス
    
    void Awake()
    {
        //filePath = Directory.GetCurrentDirectory();
        filePath = Application.persistentDataPath + "/" + ".savedata.json";

        Debug.Log("Save filepath : " + filePath);

        //SaveDataTest test = new SaveDataTest();
        //print(JsonUtility.ToJson(test));
    }
    
    // セーブファイルにスコアをセーブする
    public void SaveScore(IReadOnlyList<SaveData> savelist){
        string json = "";
        foreach (SaveData save in savelist) {
            json = json + JsonUtility.ToJson(save) + "\n";
        }
        Save(json);
    }

    // セーブファイルにスコアをセーブする(ファイル操作)
    void Save(string save)
    {
        StreamWriter streamWriter = new StreamWriter(filePath);
        streamWriter.Write(save);
        streamWriter.Flush();
        streamWriter.Close();
    }
    
    // セーブファイルからスコアをロードする
    public List<SaveData> LoadScore(){
        List<SaveData> saveList = new List<SaveData>();
        string data = Load();
        string[] saveData = data.Split(new[]{"\n"},StringSplitOptions.None);
        foreach(string save in saveData){
            if (save != "") {
                saveList.Add(JsonUtility.FromJson<SaveData>(save));
            }
        }
        return saveList;
    }

    // セーブファイルからスコアをロードする(ファイル操作)
    string Load()
    {
        string save = "";
        Debug.Log("ファイルチェック : " + filePath);
        if (File.Exists(filePath))
        {
            StreamReader streamReader;
            streamReader = new StreamReader(filePath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();
            save = data;
        } else {
            Debug.Log("not exist save file");
        }
        return save;
    }
}
