using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    const int STAGE_GEN_NUM = 6;      // ステージの生成数
    const int OBSTACLE_GEN_NUM = 3;   // 障害物の生成数
    const int STAGE_TIP_SIZE = 30;    // ステージチップのサイズ

    [SerializeField] GameObject player;             // playerオブジェクトへの参照
    [SerializeField] GameObject[] stageTips;        // ステージチップの配列
    [SerializeField] GameObject[] stageGutter;      // ステージ側面のチップ配列
    [SerializeField] GameObject[] stageObstacle;    // 障害物の配列
    [SerializeField] bool genObstacle = true;       // 障害物を生成フラグ(falseにすると生成されない)
    List<GameObject> generatedStageList = new List<GameObject>(STAGE_GEN_NUM + 1);      //作成したステージチップの保持リスト
    List<GameObject> generatedRightGutterList = new List<GameObject>(STAGE_GEN_NUM + 1);      //作成したステージチップの保持リスト
    List<GameObject> generatedLeftGutterList = new List<GameObject>(STAGE_GEN_NUM + 1);      //作成したステージチップの保持リスト
    List<GameObject> generatedObstacleList = new List<GameObject>(STAGE_GEN_NUM + 1);   //作成した障害物の保持リスト
    int currentTipIndex = 0;                        // 現在のステージチップ番号

    void Start()
    {
        // 開始時のステージ生成
        for(int i = 0; i < STAGE_GEN_NUM; i++){
            generatedStageList.Add(GenerateStage(i));
            generatedRightGutterList.Add(GenerateRightGutter(i));
            generatedLeftGutterList.Add(GenerateLeftGutter(i));
        }
         for(int i = 0; i < OBSTACLE_GEN_NUM; i++) {
            if(genObstacle){generatedObstacleList.Add(GenerateObstacle());}
        }
    }

    void Update()
    {
        if(player.transform.position.z <= 0){return;}
        
        float playerPosition = player.transform.position.z;
        int tipIndex = (int)Mathf.Floor(playerPosition / STAGE_TIP_SIZE);
        // ステージ生成
        if(currentTipIndex < tipIndex){
            generatedStageList.Add(GenerateStage(currentTipIndex + STAGE_GEN_NUM));
            generatedRightGutterList.Add(GenerateRightGutter(currentTipIndex + STAGE_GEN_NUM));
            generatedLeftGutterList.Add(GenerateLeftGutter(currentTipIndex + STAGE_GEN_NUM));
            DestroyOldestStage();
            currentTipIndex = tipIndex;
        }
        // 障害物生成
        if(genObstacle){
            if (generatedObstacleList.Count == 0 || (playerPosition >= generatedObstacleList[0].transform.position.z)) {
                //Debug.Log("障害物生成");
                generatedObstacleList.Add(GenerateObstacle());
                DestroyOldObstcle();
            }
        }
    }

    // 指定のインデックス位置にstageオブジェクトをランダムに生成
    GameObject GenerateStage (int tipIndex)
    {
        int nextStageTip = Random.Range(0, stageTips.Length);
        GameObject stageObject = (GameObject)Instantiate(
            stageTips[nextStageTip],
            new Vector3(0, 0, tipIndex * STAGE_TIP_SIZE),
            Quaternion.identity);
        return stageObject;
    }

    GameObject GenerateRightGutter (int tipIndex)
    {
        int nextStageTip = Random.Range(0, stageGutter.Length);
        GameObject stageObject = (GameObject)Instantiate(
            stageGutter[nextStageTip],
            new Vector3(3, 0.1f, tipIndex * STAGE_TIP_SIZE),
            Quaternion.identity);
        return stageObject;
    }

    GameObject GenerateLeftGutter (int tipIndex)
    {
        int nextStageTip = Random.Range(0, stageGutter.Length);
        GameObject stageObject = (GameObject)Instantiate(
            stageGutter[nextStageTip],
            new Vector3(-3, 0.1f, tipIndex * STAGE_TIP_SIZE),
            Quaternion.identity);
        return stageObject;
    }

    // 指定のインデックス位置にobstacleオブジェクトをランダムに配置
    GameObject GenerateObstacle(){
        float genPos = 0;
        if (generatedObstacleList.Count == 0) {
            genPos = player.transform.position.z + 100;
        } else {
            genPos = generatedObstacleList[generatedObstacleList.Count - 1].transform.position.z + 50;
        }
        
        int nextObstacle = Random.Range(0, stageObstacle.Length);
        GameObject obstacle = (GameObject)Instantiate(
            stageObstacle[nextObstacle],
            new Vector3(0, 1, genPos),
            Quaternion.identity);
        return obstacle;
    }

    // 一番古いステージを削除
   void DestroyOldestStage ()
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);

        oldStage = generatedRightGutterList[0];
        generatedRightGutterList.RemoveAt(0);
        Destroy(oldStage);

        oldStage = generatedLeftGutterList[0];
        generatedLeftGutterList.RemoveAt(0);
        Destroy(oldStage);
    }

    void DestroyOldObstcle(){
        if (genObstacle) {
            if (generatedObstacleList.Count > OBSTACLE_GEN_NUM) {
                GameObject oldObstacle = generatedObstacleList[0];
                generatedObstacleList.RemoveAt(0);
                Destroy(oldObstacle);
            }
        }
    }
}
