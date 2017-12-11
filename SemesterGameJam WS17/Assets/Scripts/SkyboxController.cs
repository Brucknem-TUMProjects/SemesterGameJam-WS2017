using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : MonoBehaviour {
    [SerializeField]
    Transform follow;

    float distance;

    private void Awake()
    {
        distance = transform.position.z - follow.position.z;
    }

    // Update is called once per frame
    void Update () {
        if (follow == null)
            return;

        transform.position = Vector3.forward * (follow.position.z + distance);
	}
}
