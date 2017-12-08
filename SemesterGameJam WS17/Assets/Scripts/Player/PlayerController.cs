using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;

    public int lane = 0;
    private float lastLaneSwitch = 0;

    public float gravityStrength = 9.81f;

    private Vector3[] laneMiddles = new Vector3[] {
        new Vector3(0,0,0),
    };
    private float laneWidth = 1.2f;

    private Rigidbody rb;
    private Vector3 gravity = Vector3.down;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}

    private void FixedUpdate()
    {
        rb.AddForce(gravity * gravityStrength);
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, speed);


        transform.position = Vector3.Lerp(transform.position, new Vector3(lane * laneWidth, transform.position.y, transform.position.z), 0.1f);
    }

    // Update is called once per frame
    void Update () {
        GetInput();
        //transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.forward * velocity, 0.5f);
	}

    void GetInput()
    {
        SwitchLane();
    }

    void SwitchLane()
    {
        int input = Input.GetButtonDown("Right") ? 1 : 0;
        input = Input.GetButtonDown("Left") ? -1 : input;

        if (Time.time - lastLaneSwitch > 0.1f && input != 0)
        {
            lane += input;
            if (lane < -1 || lane > 1)
            {
                if (lane < -1)
                    lane = -1;
                if (lane > 1)
                    lane = 1;
                input = 0;
            }

            print("Lane: " + lane);
            lastLaneSwitch = Time.time;
            if(input != 0)
                rb.AddForce(Vector3.up * 80f);
        }
    }
}
