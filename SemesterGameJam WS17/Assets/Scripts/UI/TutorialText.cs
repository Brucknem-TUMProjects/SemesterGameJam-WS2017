using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialText : MonoBehaviour {
    [SerializeField]
    float threshold;

    Text text;
    string tutorial;
    float timer;
    int index;

	// Use this for initialization
	void Awake () {
        text = GetComponent<Text>();
        tutorial = text.text;
        text.text = "";
        index = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (index < tutorial.Length && Time.time > timer)
        {
            timer = Time.time + threshold;

            text.text += tutorial[index];
            ++index;
        }
	}
}
