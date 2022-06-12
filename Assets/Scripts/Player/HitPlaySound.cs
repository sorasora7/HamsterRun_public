using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlaySound : MonoBehaviour
{
    public AudioClip bgm;
    public AudioClip getCoin;

    void OnCollisionEnter(Collision collision) {
        AudioSource.PlayClipAtPoint(getCoin, transform.position);
    }
}
