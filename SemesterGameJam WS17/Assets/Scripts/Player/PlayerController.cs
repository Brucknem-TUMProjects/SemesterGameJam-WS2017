using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float speed;
    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            GameData.Instance.currentVelocity = value;
            speed = value;
        }
    }

    public float startSpeed;

    public float jumpSpeed;

    public int lane = 0;
    private float lastLaneSwitch = 0;

    public float gravityStrength = 9.81f;

    #region Massspring System
    //private Vector3 attractionPoint = Vector3.zero;
    #endregion


    private float laneWidth = 2f;

    public AudioClip[] audioClips;
    public AudioSource aS;


    private Rigidbody rb;
    private Vector3 gravity = Vector3.down;
    public int curOrient = 0;

    public int index;

    public GameObject springSystem;

    public GameObject explosion;

    private List<Rigidbody> springPoints = new List<Rigidbody>();
    private SpringJoint springJoint;
    private SpringSystemFollowCamera followCamera;
    public Vector3 ActiveSpringPoint {
        get
        {
            return springPoints[index].transform.position;
        }
    }


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        springJoint = GetComponent<SpringJoint>();
        for (int i = 0; i < springSystem.transform.childCount; i++)
        {
            springPoints.Add(springSystem.transform.GetChild(i).GetComponent<Rigidbody>());
        }
        springJoint.connectedBody = springPoints[index];
        followCamera = springSystem.GetComponent<SpringSystemFollowCamera>();
        GameData.Instance.Orientation = 0;
        GameData.Instance.activeLanes = new List<int>();
        for (int i = 0; i < 12; i += 1)
            GameData.Instance.activeLanes.Add(i);
        //speed = startSpeed;
        //GameData.Instance.startVelocity = startSpeed;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, speed);
        IncreaseSpeed();
        followCamera.UpdatePosition(transform.position.z);
        GameData.Instance.Score = (int)transform.position.z;
    }

    void IncreaseSpeed()
    {
        //Speed += 1f / (Time.fixedDeltaTime * 50000f);
        if (dead)
            speed = 0;
        else
            speed = transform.position.z / 200 + GameData.Instance.startVelocity;
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
            Die();
            //GameData.Instance.IsAlive = false;
            //UnityEditor.EditorApplication.isPlaying = false;
        }
    }


	void GetInput()
    {
        SwitchPosition();
    }

    

    //void LaneAttraction()
    //{

    //    switch (GameData.Instance.Orientation)
    //    {
    //        case 0:
    //            switch (lane)
    //            {
    //                case -1:
    //                    attractionPoint = new Vector3(-laneWidth, -4, transform.position.z);
    //                    break;

    //                case 0:
    //                    attractionPoint = new Vector3(0, -4, transform.position.z);
    //                    break;

    //                case 1:
    //                    attractionPoint = new Vector3(laneWidth, -4, transform.position.z);
    //                    break;

    //                default:
    //                    break;
    //            }
    //            break;

    //        case 1:
    //            switch (lane)
    //            {
    //                case -1:
    //                    attractionPoint = new Vector3(4, -laneWidth, transform.position.z);
    //                    break;

    //                case 0:
    //                    attractionPoint = new Vector3(4, 0, transform.position.z);
    //                    break;

    //                case 1:
    //                    attractionPoint = new Vector3(4, laneWidth, transform.position.z);
    //                    break;

    //                default:
    //                    break;
    //            }
    //            break;

    //        case 2:
    //            switch (lane)
    //            {
    //                case -1:
    //                    attractionPoint = new Vector3(-laneWidth, 4, transform.position.z);
    //                    break;

    //                case 0:
    //                    attractionPoint = new Vector3(0, 4, transform.position.z);
    //                    break;

    //                case 1:
    //                    attractionPoint = new Vector3(laneWidth, 4, transform.position.z);
    //                    break;

    //                default:
    //                    break;
    //            }
    //            break;

    //        case 3:
    //            switch (lane)
    //            {
    //                case -1:
    //                    attractionPoint = new Vector3(-4, -laneWidth, transform.position.z);
    //                    break;

    //                case 0:
    //                    attractionPoint = new Vector3(-4, 0, transform.position.z);
    //                    break;

    //                case 1:
    //                    attractionPoint = new Vector3(-4, laneWidth, transform.position.z);
    //                    break;

    //                default:
    //                    break;
    //            }
    //            break;

    //        default:
    //            break;
    //    }
    //}

    void SwitchPosition()
    {
        if (!GameData.Instance.Is_Paused)
        {
            //Akbar
            switch (GameData.Instance.inputType)
            {
                case 0:
                    lane = Input.GetButton("Right") ? 1 : 0;
                    lane = Input.GetButton("Left") ? -1 : lane;


                    if (Input.GetButtonDown("OrientRight"))
                    {
                        GameData.Instance.Orientation++;
                    }
                    else if (Input.GetButtonDown("OrientLeft"))
                    {
                        GameData.Instance.Orientation--;
                    }
                    else if (Input.GetButtonDown("OrientUp"))
                    {
                        GameData.Instance.Orientation += 2;
                    }
                    break;

                //MARI
                case 1:
                    lane += Input.GetButtonDown("Right") ? 1 : 0;
                    lane += Input.GetButtonDown("Left") ? -1 : 0;
                    //lane = Input.GetButtonDown("Down") ? 0 : lane;
                    //print(lane);

                    if (lane > 1) lane = 1;
                    if (lane < -1) lane = -1;


                    if (Input.GetButtonDown("OrientRight"))
                    {
                        GameData.Instance.Orientation++;
                        if (lane == 1)
                            lane = -lane;

                    }
                    else if (Input.GetButtonDown("OrientLeft"))
                    {
                        GameData.Instance.Orientation--;
                        if (lane == -1)
                            lane = -lane;

                    }
                    else if (Input.GetButtonDown("OrientUp"))
                    {
                        GameData.Instance.Orientation += 2;
                        lane = -lane;
                    }

                    break;
                    //Spongebob
            }

            if(Input.GetButtonDown("OrientRight")
                || Input.GetButtonDown("OrientLeft")
                || Input.GetButtonDown("OrientUp")
                || Input.GetButtonDown("Left")
                || Input.GetButtonDown("Right"))
            {
                int x = Random.Range(0, audioClips.Length);
                aS.PlayOneShot(audioClips[x]);
            }

            GameData.Instance.Orientation %= 4;
            if (GameData.Instance.Orientation < 0)
                GameData.Instance.Orientation += 4;

            if (Time.time - lastLaneSwitch > 0.1f)
            {
                index = GameData.Instance.Orientation * 3 + 1;
                index += lane;
                if (index < 0)
                    index += 12;

                lastLaneSwitch = Time.time;
            }

            springJoint.connectedBody = springPoints[index];
            //print(index);
            ChangeOrientation();
        }
    }

    //void LerpToCurrentLane()
    //{
    //    // lerp player onto  current lane
    //    if (gravity == Vector3.down)
    //        transform.position = Vector3.Lerp(transform.position, new Vector3(lane * laneWidth, transform.position.y, transform.position.z), 0.1f);
    //    else if (gravity == Vector3.right)
    //        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, lane * laneWidth, transform.position.z), 0.1f);
    //    else if (gravity == Vector3.up)
    //        transform.position = Vector3.Lerp(transform.position, new Vector3(-lane * laneWidth, transform.position.y, transform.position.z), 0.1f);
    //    else if (gravity == Vector3.left)
    //        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, -lane * laneWidth, transform.position.z), 0.1f);
    //}

    //// switches the current orientation
    //void SwitchOrientation()
    //{
    //    if (Input.GetButtonDown("OrientRight"))
    //    {
    //        GameData.Instance.Orientation = (GameData.Instance.Orientation + 1) % 4;
    //        ChangeOrientation();
    //    }
    //    else if (Input.GetButtonDown("OrientLeft"))
    //    {
    //        GameData.Instance.Orientation = (GameData.Instance.Orientation - 1);
    //        if (GameData.Instance.Orientation < 0)
    //            GameData.Instance.Orientation = 3;
    //            ChangeOrientation();
    //    }
    //    else if (Input.GetButtonDown("OrientUp"))
    //    {
    //        GameData.Instance.Orientation = (GameData.Instance.Orientation + 2) % 4;
    //        ChangeOrientation();
    //    }
    //}

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

        //springJoint.connectedAnchor = springPoints[index / 3].transform.position;
    }

    private bool dead = false;

    public void Die()
    {
        if (!dead)
        {
            GameObject expl = Instantiate(explosion, transform);
            GameData.Instance.IsAlive = false;
            transform.GetChild(1).gameObject.SetActive(false);
            dead = true;
        }
    }
}
