using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject virtualCamera;
    GameObject vCam;

    // Start is called before the first frame update
    void Start()
    {
        vCam = virtualCamera.transform.Find("VCamMain").gameObject;
        //StartCoroutine("changeCamera");
    }

    IEnumerator changeCamera(){
        yield return new WaitForSeconds(1.0f);
        CinemachineVirtualCamera  cinema = vCam.GetComponent<CinemachineVirtualCamera>();
        cinema.Priority = 12;
    }
}
