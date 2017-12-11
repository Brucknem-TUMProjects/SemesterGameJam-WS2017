using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour {

    public void Awake()
    {
        int type = Random.Range(0, 4);
        //float angleRange = 90;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(type == i);
        }

        switch (type)
        {            
            case 0:
                //float angle = Random.Range(transform.rotation.eulerAngles.z - angleRange / 2, transform.rotation.eulerAngles.z + angleRange / 2);
                float angle = Random.Range(-1, 2);
                //print(transform.rotation.eulerAngles.z + " - " + angle);
                transform.GetChild(type).GetComponent<RayController>().Init(transform.rotation.eulerAngles.z + angle * 45);
                    break;
            default:
                transform.position += transform.up * 1.5f;
                transform.rotation = Quaternion.Euler(Random.Range(-30, 30), Random.Range(-30, 30), Random.Range(0, 360)
                    );
                break;
        }
    }
}
