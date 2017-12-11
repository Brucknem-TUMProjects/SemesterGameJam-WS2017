using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveMouseWithController : MonoBehaviour {

    public Image cursor;
    public float speed = 10;
    bool shown = false;
	
	// Update is called once per frame
	void Update () {
        float x = Input.GetAxisRaw("Horizontal") * speed;
        float y = Input.GetAxisRaw("Vertical") * speed;
        //print(x + " - " + y);
        cursor.transform.position += new Vector3(x, y);

        RaycastHit hit;
        if (Input.GetButtonDown("Submit"))
        {
            print("Submit");
            if (Physics.Raycast(cursor.transform.position, Vector3.forward, out hit, 1111.0F))
            {
                print(hit.transform.gameObject.name);
                try
                {
                    hit.transform.GetComponent<Button>().onClick.Invoke();
                } catch (Exception e) { };

                try
                {
                    if (shown)
                    {
                        hit.transform.GetComponent<Dropdown>().Show();
                    }
                    else
                    {
                        hit.transform.GetComponent<Dropdown>().Hide();
                    }
                    shown = !shown;
                }
                catch (Exception) { };
            }
        }
    }
}
