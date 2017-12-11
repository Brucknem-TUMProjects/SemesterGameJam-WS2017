using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Awake()
    {
        //if(indices.Count == 12)
        //{
        //    float rand = Random.Range(0, 1);
        //    if(rand < 0.5f)
        //    {
        //        rand = Mathf.Floor(Random.Range(0, 12));

        //    }
        //}

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            if (Random.Range(0, 1f) < 1.0f / (GameData.Instance.currentVelocity / 50f))
            {
                transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
                transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
                transform.GetChild(i).GetChild(1).gameObject.SetActive(true);
            }
        }
    }
}
