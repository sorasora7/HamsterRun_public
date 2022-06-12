using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsDisplay : MonoBehaviour
{
    int frameCount;             // フレームカウンター
    float prevTime;             // 経過時間
    float fps;                  // 現在のfps
    public GUIStyle fpsStyle;   // GUI表示の表示スタイル

    void Start()
    {
        frameCount = 0;
        prevTime = 0.0f;
    }

    void Update()
    {
        frameCount++;
        float time = Time.realtimeSinceStartup - prevTime;

        if (time >= 0.5f) {
            fps = frameCount / time;
            //Debug.Log(fps);

            frameCount = 0;
            prevTime = Time.realtimeSinceStartup;
        }
    }

    void OnGUI(){
        GUILayout.Label(fps.ToString(), fpsStyle);
    }
}
