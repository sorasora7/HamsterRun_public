using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CcMove : MonoBehaviour
{
    public float speed = 5.0F;       //歩行速度
    public float jumpSpeed = 8.0F;   //ジャンプ力
    public float gravity = 20.0F;    //重力の大きさ

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private float h,v;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis ("Horizontal");    //左右矢印キーの値(-1.0~1.0)
        v = Input.GetAxis ("Vertical");      //上下矢印キーの値(-1.0~1.0)

        if (controller.isGrounded) {
            moveDirection = new Vector3 (h, 0, v);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.x *= speed;
            moveDirection.z *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
