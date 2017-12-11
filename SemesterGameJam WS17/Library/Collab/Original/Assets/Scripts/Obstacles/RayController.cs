using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayController : MonoBehaviour {
    //---------------------------------------------------------------------------------------------//
    // Variables //--------------------------------------------------------------------------------//
    [SerializeField]
    LayerMask hitObject;
    [SerializeField]
    float length;
    [SerializeField]
    Transform rayEmitter;
    [SerializeField]
    Color color;

    //---------------------------------------------------------------------------------------------//
    // Functions //--------------------------------------------------------------------------------//
    private void Awake()
    {
        LineRenderer ray = rayEmitter.Find("Ray").GetComponent<LineRenderer>();
        ray.SetPosition(1, Vector3.up * length);
        ray.startColor = ray.endColor = color;
    }

    public void Init(float angle)
    {
        rayEmitter.rotation = Quaternion.Euler(Vector3.forward * angle);
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayEmitter.position, rayEmitter.up, out hit, length, hitObject))
        {
            //GameData.Instance.IsAlive = false;
            hit.transform.gameObject.GetComponent<PlayerController>().Die();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayEmitter.position, rayEmitter.position + rayEmitter.up * length);
    }
}
