using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

	public GameObject player;
	public GameObject[] sides;

	public Material[] materials;

    public ObstacleManager om;

	private int currentPosition = 0;
	private int laneLength = 10;
	private bool front;

	// Use this for initialization
	void Start()
	{
        laneLength = (int) sides[0].transform.GetChild(0).GetChild(0).GetChild(0).localScale.z;
        foreach (GameObject side in sides)
		{
			side.transform.GetChild(1).position = new Vector3(side.transform.GetChild(1).position.x, side.transform.GetChild(1).position.y, laneLength);
		}
        SetShaderVariables();
    }

	// Update is called once per frame
	void FixedUpdate()
	{
		if ((player.transform.position.z - (laneLength/2)) / laneLength > currentPosition)
		{
			currentPosition++;
			//if (currentPosition % 5 == 0)
			//	player.GetComponent<PlayerController>().Speed += 1;
			foreach (GameObject side in sides)
			{
				if (side.transform.GetChild(0).position.z < side.transform.GetChild(1).position.z)
				{
					side.transform.GetChild(0).position = new Vector3(side.transform.GetChild(0).position.x, side.transform.GetChild(0).position.y, (currentPosition + 1) * laneLength);
					front = true;
				}
				else
				{
					side.transform.GetChild(1).position = new Vector3(side.transform.GetChild(1).position.x, side.transform.GetChild(1).position.y, (currentPosition + 1) * laneLength);
					front = false;
				}
				side.GetComponent<GroundManager>().Randomize(front);
			}
            om.SetObstacles(currentPosition, laneLength);

        }
        SetShaderVariables();
    }

	public void SetShaderVariables()
	{
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].SetFloat("_Speed", player.transform.position.z + (laneLength-1) / 2);
		}

		//materials[i].SetFloat("_StartingTime", Time.time);
	}
}
