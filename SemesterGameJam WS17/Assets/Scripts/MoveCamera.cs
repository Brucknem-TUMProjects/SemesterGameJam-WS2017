using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    [Header("Camera Settings")]
    public PlayerController player;
    public int distance;
    private Quaternion curRot;
    

    // Update is called once per frame
    void Update() {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z - distance);
        transform.rotation = Quaternion.Lerp(transform.rotation, curRot, .1f);
        ChangeOrientation();
    }

    public void ChangeOrientation()
    {
        switch (GameData.Instance.Orientation)
        {
            case 0:
                curRot = Quaternion.Euler(0, 0, 0);
                break;
            case 1:
                curRot = Quaternion.Euler(0, 0, 90);
                break;
            case 2:
                curRot = Quaternion.Euler(0, 0, 180);
                break;
            case 3:
                curRot = Quaternion.Euler(0, 0, 270);
                break;
            default:
                break;
        }
    }
}
