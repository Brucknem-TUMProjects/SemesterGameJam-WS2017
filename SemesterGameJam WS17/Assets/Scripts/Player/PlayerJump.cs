using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody rb;
    public float jumpSpeed;

    private void Awake()
    {
        rb = transform.parent.GetComponent<Rigidbody>();   
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButton("Jump") && other.tag == "Lane")
        {
            switch (GameData.Instance.Orientation)
            {
                case 0:
                    rb.AddForce(Vector3.up * jumpSpeed);
                    break;
                case 1:
                    rb.AddForce(Vector3.left * jumpSpeed);
                    break;
                case 2:
                    rb.AddForce(Vector3.down * jumpSpeed);
                    break;
                case 3:
                    rb.AddForce(Vector3.right * jumpSpeed);
                    break;
                default:
                    break;
            }
        }
    }
}
