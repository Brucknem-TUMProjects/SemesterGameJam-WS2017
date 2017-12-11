using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            GameData.Instance.difficulty = value;
            speed = value;
        }
    }

    public float jumpSpeed;

    public int lane = 0;
    private float lastLaneSwitch = 0;

    public float gravityStrength = 9.81f;


    private float laneWidth = 2f;


    private Rigidbody rb;
    private Vector3 gravity = Vector3.down;
    public int curOrient = 0;

	public LevelManager lm;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}

    private void FixedUpdate()
    {
        ApplyGravity();
        LerpToCurrentLane();
        IncreaseSpeed();
    }

    // Update is called once per frame
    void Update () {
        GetInput();
	}

    private void OnCollisionEnter(Collision other)
    {
        if (GameData.Instance.Invincible)
            return;

        if(other.gameObject.tag != "Lane")
        {
            GameData.Instance.Score = (int)transform.position.z;
            GameData.Instance.IsAlive = false;
        }
    }

    void IncreaseSpeed()
    {
		Speed += 1f / (Time.fixedDeltaTime * 50000f);
		lm.SetShaderVariables(Speed);
	}

	void GetInput()
    {
        SwitchLane();
        SwitchOrientation();
    }

    void ApplyGravity()
    {
        // apply "current" gravity on rigidbody
        rb.AddForce(gravity * gravityStrength);
        // move player automatically forward
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, speed);
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

            lastLaneSwitch = Time.time;

        }
    }

    void LerpToCurrentLane()
    {
        // lerp player onto  current lane
        if (gravity == Vector3.down)
            transform.position = Vector3.Lerp(transform.position, new Vector3(lane * laneWidth, transform.position.y, transform.position.z), 0.1f);
        else if (gravity == Vector3.right)
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, lane * laneWidth, transform.position.z), 0.1f);
        else if (gravity == Vector3.up)
            transform.position = Vector3.Lerp(transform.position, new Vector3(-lane * laneWidth, transform.position.y, transform.position.z), 0.1f);
        else if (gravity == Vector3.left)
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -lane * laneWidth, transform.position.z), 0.1f);
    }

    // switches the current orientation
    void SwitchOrientation()
    {
        if (Input.GetButtonDown("OrientRight"))
        {
            GameData.Instance.Orientation = (GameData.Instance.Orientation + 1) % 4;
            ChangeOrientation();
        }
        else if (Input.GetButtonDown("OrientLeft"))
        {
            GameData.Instance.Orientation = (GameData.Instance.Orientation - 1);
            if (GameData.Instance.Orientation < 0)
                GameData.Instance.Orientation = 3;
                ChangeOrientation();
        }
        else if (Input.GetButtonDown("OrientUp"))
        {
            GameData.Instance.Orientation = (GameData.Instance.Orientation + 2) % 4;
            ChangeOrientation();
        }
    }

    // changes the orientation of the player
    void ChangeOrientation()
    {
        rb.velocity = new Vector3(0, 0, rb.velocity.z);
        switch (GameData.Instance.Orientation)
        {
            case 0:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                gravity = Vector3.down;
                break;
            case 1:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                gravity = Vector3.right;
                break;
            case 2:
                transform.rotation = Quaternion.Euler(0, 0, 180);
                gravity = Vector3.up;
                break;
            case 3:
                transform.rotation = Quaternion.Euler(0, 0, 270);
                gravity = Vector3.left;
                break;
            default:
                break;
        }
    }
}
