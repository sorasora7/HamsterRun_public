using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RdVelocity : MonoBehaviour
{
	public float speed = 100f;
    public float jumpPower = 150f;
    public float gravity = -200f;
    bool jumpFlg = false;
	float moveX = 0f;
	float moveZ = 0f;
	Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody> ();
    }

    // Update is called once per frame
    void Update()
    {
		moveX = Input.GetAxis ("Horizontal") * speed;
		moveZ = Input.GetAxis ("Vertical") * speed;
		Vector3 direction = new Vector3(moveX , rb.velocity.y, moveZ);

        // Judgement Jump
        if (Input.GetKeyDown (KeyCode.Space)) {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            jumpFlg = true;
        }

        rb.AddForce(new Vector3(0, gravity, 0));
    }

	void FixedUpdate(){
		rb.velocity = new Vector3(moveX, rb.velocity.y, moveZ);

        if (jumpFlg) {
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            jumpFlg = false;
        }
	}
}
