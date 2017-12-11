using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringSystemFollowCamera : MonoBehaviour {
   
    public Transform follow;
    
    // Update is called once per frame
    void LateUpdate () {
	}

    public void UpdatePosition(float z)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
}
