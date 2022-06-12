using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    const int MAX_SCORE_NUM = 10;   // スコア保持件数
    const int SCALE_RATIO = 3;      // Unity上での距離とスコアとして表示する距離の割合

    public const int GMAE_STATUS_START = 0;
    public const int GMAE_STATUS_GAME  = 1;
    public const int GMAE_STATUS_DEAD  = 2;

    public GameObject saveManager;              // saveManagerオブジェクトへの参照
    public static int gameStatus{get;set;}    // ゲームオーバーになったかの判定フラグ
    public static float socre {get;set;}        // 現在のスコア
    public static int coin{get;set;}            // 現在の所持コイン数
    public static float velocity{get;set;}      // 現在の移動速度
    [SerializeField] GameObject player;         // playerオブジェクトへの参照
    SaveManager save_manager;                   // セーブマネージャーコンポーネント
    bool saveFlg = false;                       // セーブを行ったかどうかのフラグ

    void Awake(){
        Application.targetFrameRate = 60; 
    }

    void Start()
    {
        save_manager = saveManager.GetComponent<SaveManager>();
        socre = 0f;
        coin = 0;
        velocity = 0f;
        gameStatus = GMAE_STATUS_START;
    }

    void Update()
    {
        // スコア計算(小数点第一位まで表示)
        if (player.transform.position.z > 0) {
            socre = (Mathf.Floor(player.transform.position.z / SCALE_RATIO * 10)) / 10;
        }

        // ゲームオーバー時にスコアの保存を一回行う
        if (gameStatus == GMAE_STATUS_DEAD && !saveFlg) {
            // スコアデータ作成
            List<SaveData> savelist = save_manager.LoadScore();
            SaveData save = new SaveData();
            DateTime todayNow = DateTime.Now;
            save.date = todayNow.Year.ToString() + "年 " + todayNow.Month.ToString() + "月" + todayNow.Day.ToString() + "日" + DateTime.Now.ToLongTimeString();
            save.score = socre.ToString();
            save.comment = "まーべらす";
            // ハイスコアチェック＆登録
            if (checkHighScore(savelist, save)){
                Debug.Log("ハイスコア更新!!");
            }
            checkRankIn(savelist, save);
            save_manager.SaveScore(savelist);

            saveFlg = true;
        }
    }

    // ハイスコアチェック
    bool checkHighScore(IReadOnlyList<SaveData> saveList, SaveData save){
        if (saveList.Count != 0) {
            SaveData highScore = saveList[0];
            return float.Parse(save.score) > float.Parse(highScore.score);
        } else {
            return true;
        }
    }

    // ランクインチェック(ランクインしていたらリスト入れ替え)
    void checkRankIn(List<SaveData> saveList, SaveData save){
        if (saveList.Count != 0) {
            int i = 0;
            for (i = 0; i < saveList.Count; i++) {
                SaveData sd = saveList[i];
                if (float.Parse(sd.score) < float.Parse(save.score)) {break;}
            }
            // ランクインしていれば登録
            if (i < MAX_SCORE_NUM) {
                saveList.Insert(i, save);
                if (saveList.Count > MAX_SCORE_NUM) {
                    saveList.RemoveAt(saveList.Count - 1);
                }
            }
        } else {
            saveList.Add(save);
        }
    }
}
