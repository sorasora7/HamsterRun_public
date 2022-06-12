using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] GameObject score;      // スコア表示オブジェクトへの参照
    [SerializeField] GameObject coin;       // coin表示オブジェクトへの参照
    [SerializeField] GameObject velocity;   // velocity表示オブジェクトへの参照
    [SerializeField] GameObject player;     // playerオブジェクトへの参照
    [SerializeField] GameObject uiResult;   // UiResultオブジェクトへの参照
    GameManager gameManager;                // ゲームマネージャオブジェクト
    Text score_text;                        // scoreテキストコンポーネント
    Text coin_text;                         // coinテキストコンポーネント
    Text velocity_text;                     // velocityテキストコンポーネント
    Text ressult_text;                      // ressultテキストコンポーネント
    Text comment_text;                      // commentテキストコンポーネント

    void Start()
    {
        GameObject obj;
        score_text = score.GetComponent<Text>();
        coin_text = coin.GetComponent<Text>();
        velocity_text = velocity.GetComponent<Text>();

        obj = uiResult.transform.Find("panel/Score/TextScore").gameObject;
        ressult_text = obj.GetComponent<Text>();
        obj = uiResult.transform.Find("panel/Comment/TextComment").gameObject;
        comment_text = obj.GetComponent<Text>();
    }

    void Update()
    {
        // ゲームオーバー時、ゲーム結果を表示する
        if (GameManager.gameStatus == GameManager.GMAE_STATUS_DEAD) {
            ressult_text.text = GameManager.socre + "m";
            comment_text.text = "すごい！";
        }
    }

    void FixedUpdate()
    {
        // UI更新
        score_text.text = "逃走距離 : " + GameManager.socre + "m";
        velocity_text.text = "" + GameManager.velocity + "m/s";
        coin_text.text = "" + GameManager.coin;
    }
}
