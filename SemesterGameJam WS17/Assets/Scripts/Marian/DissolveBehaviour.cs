using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveBehaviour : MonoBehaviour
{

	public Material dMat;
	public float speed, max;

	private float currentY, startTime;

	private void Update()
	{	
		//if (currentY < max)
		//{
		//	dMat.SetFloat("_DissolveY", currentY);
		//	currentY += speed * Time.deltaTime;
		//}
		if (Input.GetKeyDown(KeyCode.F))
		{
			Debug.Log("Triggered");
			TriggerEffect();
		}
	}

	public void TriggerEffect()
	{
		//startTime = Time.time;
		//currentY = 0;
		dMat.SetFloat("_StartingTime", Time.time);
	}

    public void SetVariables()
    {
        dMat.SetFloat("_Speed", GameData.Instance.currentVelocity * 5);
        //dMat.SetFloat("_StartingTime", Time.time);
    }
}
