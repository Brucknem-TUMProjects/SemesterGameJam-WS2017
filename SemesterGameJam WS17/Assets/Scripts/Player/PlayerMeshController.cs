using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeshController : MonoBehaviour
{
    [SerializeField]
    float rotateSpeed;
    [SerializeField]
    float maxSpeed;
    [SerializeField]
    float changeTime;

    float timer;
    Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timer)
        {
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);
            float z = Random.Range(-1f, 1f);
            Vector3 rotation = new Vector3(x, y, z) * rotateSpeed;

            rigid.angularVelocity = Vector3.ClampMagnitude(rigid.angularVelocity + rotation, maxSpeed);

            timer = Time.time + changeTime;
        }
    }
}
