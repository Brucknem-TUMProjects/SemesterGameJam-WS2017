using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowScore : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        GetComponentInChildren<Text>().text = GameData.Instance.Score.ToString();
	}
}
