using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaMove : MonoBehaviour
{
    const float LANE_CENTER = 0;      // 中央レーンのX座標
    const float LANE_RIGHT = 1.0f;    // 右レーンのX座標
    const float LANE_LEFT = -1.0f;    // 左レーンのX座標

    const float MOVE_SPEED_HORIZON = 10.0f;
    const float MOVE_SPEED_START = 10.0f;
    const float MOVE_SPEED_LV_1 = 20.0f;
    const float MOVE_SPEED_LV_2 = 30.0f;
    const float MOVE_SPEED_LV_3 = 40.0f;
    const float MOVE_SPEED_LV_4 = 50.0f;

    [SerializeField] GameObject UiResult;        // UiResultオブジェクトへの参照
    [SerializeField] float runSpeed = MOVE_SPEED_START;        // 歩行速度
    [SerializeField] float jumpSpeed = 15.0f;    // ジャンプ力
    [SerializeField] float gravity = 50.0f;      // 重力の大きさ
    [SerializeField] float animeSpeed = 1.0f;    // アニメーション速度
    [SerializeField] bool isFreeMove = false;    // キャラクターの自由移動許可

    AudioSource audioSource;
    [SerializeField] AudioClip seGetCoin;
    [SerializeField] AudioClip seGetSeed;
    [SerializeField] AudioClip seJump;
    [SerializeField] AudioClip seStep;
    [SerializeField] AudioClip seGameStart;
    [SerializeField] AudioClip seGameOver;

    CharacterController controller; // キャラクターコントローラーオブジェクトへの参照
    Animator animator;              // アニメーターオブジェクトへの参照
    bool jumpFlg = false;           // ジャンプ中かどうかのフラグ
    float moveLane = LANE_CENTER;   // 移動先のレーン座標
    bool moveLeftFlg = false;       // 左方向レーンへ移動を指示するフラグ
    bool moveRightFlg = false;      // 右方向レーンへ移動を指示するフラグ
    float FingerPosX0 = 0;          // 指が画面に触れた瞬間の指のx座標
    float FingerPosX1 = 0;          // 指が画面から離れた瞬間のx座標
    float PosDiff = 50f;            // x座標の差のしきい値
    bool touchFlg = false;          // マウスタッチ判定
    float moveHorizon = 0f;         // 水平方向の移動値
    float moveVirtical = 1.0f;      // 垂直方向の移動値
    Vector3 moveDirection = Vector3.zero;   // キャラクターの移動方向

    void Reset(){
        UiResult = GameObject.Find("UiResult");
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        GameManager.velocity = runSpeed;

        switch(GameManager.gameStatus){
            case GameManager.GMAE_STATUS_START:
                break;
            case GameManager.GMAE_STATUS_GAME:
                //runSpeed = MOVE_SPEED_LV_2;
                CheckMoveByMouse();
                break;
            case GameManager.GMAE_STATUS_DEAD:
                break;
        }
/*         if (GameManager.gameStatus != GameManager.GMAE_STATUS_DEAD) {
            // マウス操作判定
            CheckMoveByMouse();
        } */
    }

    void FixedUpdate()
    {
        // 加速処理
        if (GameManager.gameStatus != GameManager.GMAE_STATUS_DEAD) {
            if (!isFreeMove) {
                CharacterMove();
            } else {
                CharacterMoveFree();
            }
            animator.speed = animeSpeed;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 障害物との衝突処理
        if(hit.gameObject.tag == "Obstacle"){
            Debug.Log("ゲーム終了！" + hit.gameObject.tag);
            GameManager.gameStatus = GameManager.GMAE_STATUS_DEAD;
            audioSource.PlayOneShot(seGameOver);
            UiResult.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider collision){
        Debug.Log("OnTriggerEnter :" + collision.gameObject.tag);
        // ゲーム開始トリガー
        if (collision.gameObject.tag == "GameStart") {
            GameManager.gameStatus = GameManager.GMAE_STATUS_GAME;
            ChangeMoveSpeed(MOVE_SPEED_LV_4);
            audioSource.PlayOneShot(seGameStart);
        }
        // アイテムとの衝突処理
        if (collision.gameObject.tag == "Item") {
            Debug.Log("コイン取得" + collision.gameObject.tag);
            audioSource.PlayOneShot(seGetCoin);
            GameManager.coin++;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Seed") {
            Debug.Log("種取得" + collision.gameObject.tag);
            AudioSource.PlayClipAtPoint(seGetSeed, transform.position);
            GameManager.coin++;
            Destroy(collision.gameObject);
        }
    }

///////////////////////////////////////////////////////////////////////// myfunc

    // マウス(タッチ)操作判定を行う
    void CheckMoveByMouse(){
        // マウス入力チェック
        if (Input.GetMouseButtonDown(0)) {
            FingerPosX0 = Input.mousePosition.x;
            //Debug.Log("マウス押されたよ！" + FingerPosX0);
        }
        if (Input.GetMouseButtonUp(0)){
            FingerPosX1 = Input.mousePosition.x;
            //Debug.Log("マウス離されたよ！" + FingerPosX1);
            touchFlg = true;
        }
        // ジャンプ、移動判定
        if (touchFlg) {
            if (((Mathf.Abs(FingerPosX1 - FingerPosX0)) < PosDiff) && controller.isGrounded) {
                //Debug.Log("ジャンプ" + (FingerPosX1 - FingerPosX1));
                jumpFlg = true;
            }
            if ((FingerPosX1 - FingerPosX0 >= PosDiff) && !moveRightFlg) {
                //Debug.Log("右に移動するよ！" + (FingerPosX1 - FingerPosX0));
                if (moveLane != LANE_RIGHT) {
                    moveLane += LANE_RIGHT;
                    moveRightFlg = true;
                    audioSource.PlayOneShot(seStep);
                }
            }
            if ((FingerPosX1 - FingerPosX0 <= -PosDiff) && !moveLeftFlg) {
                //Debug.Log("左に移動するよ！" + (FingerPosX1 - FingerPosX0));
                if (moveLane != LANE_LEFT) {
                    moveLane += LANE_LEFT;
                    moveLeftFlg = true;
                    audioSource.PlayOneShot(seStep);
                }
            }
            touchFlg = false;
        }
    }
    void CharacterMove(){
        // レーンの移動設定
        if (moveLeftFlg) {
            if (moveLane < transform.position.x) {
                moveHorizon = -1.0f * MOVE_SPEED_HORIZON;
            } else {
                moveLeftFlg = false;
                moveHorizon = 0f;
            }
        } else if (moveRightFlg) {
            if(moveLane > transform.position.x) {
                moveHorizon = MOVE_SPEED_HORIZON;
            } else {
                moveRightFlg = false;
                moveHorizon = 0f;
            }
        }
        // キャラクター移動処理
        moveDirection = new Vector3 (moveHorizon, moveDirection.y, moveVirtical);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection.z *= runSpeed;
        if (jumpFlg){
            audioSource.PlayOneShot(seJump);
            moveDirection.y = jumpSpeed;
            animator.SetTrigger("JumpTrigger");
            jumpFlg = false;
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
    // キャラクターの自由移動(キーボード入力)
    void CharacterMoveFree(){
        float h,v;
        h = Input.GetAxis ("Horizontal");    //左右矢印キーの値(-1.0~1.0)
        v = Input.GetAxis ("Vertical");      //上下矢印キーの値(-1.0~1.0)

        if (controller.isGrounded) {
            moveDirection = new Vector3 (h, 0, v);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.x *= (runSpeed / 3);
            moveDirection.z *= runSpeed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
    // キャラクターの移動速度変更
    void ChangeMoveSpeed(float speed){
        StartCoroutine("ChangeMoveSpeedCo", speed);
    }
    IEnumerator ChangeMoveSpeedCo(float speed){
        float transTime = 2.0f; //指定の速度に切り替わる秒数
        float diffSpeed = speed - runSpeed;
        for (int i = 0; i < 10; i++) {
            runSpeed += (diffSpeed / 10);
            yield return new WaitForSeconds(transTime / 10);
        }
    }
}