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
		if ((player.transform.position.z - (laneLength/2)-2) / laneLength > currentPosition)
		{
			currentPosition++;
			//if (currentPosition % 5 == 0)
			//	player.GetComponent<PlayerController>().Speed += 1;
			foreach (GameObject side in sides)
			{
                Transform child = side.transform.GetChild(0);

                child.position = new Vector3(child.position.x, child.position.y, (currentPosition + 1) * laneLength);
                Transform tmp = child.parent;
                child.parent = null;
                child.parent = tmp;
            }

            for (int i = 0; i < 12; i++)
            {
                if (GameData.Instance.activeLanes.Contains(i))
                {
                    sides[i / 3].transform.GetChild(1).GetChild(i % 3).GetChild(0).gameObject.SetActive(true);
                    sides[i / 3].transform.GetChild(1).GetChild(i % 3).GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    sides[i / 3].transform.GetChild(1).GetChild(i % 3).GetChild(0).gameObject.SetActive(false);
                    sides[i / 3].transform.GetChild(1).GetChild(i % 3).GetChild(1).gameObject.SetActive(true);
                }
            }

            if (currentPosition > GameData.Instance.startVelocity / 4)
            {
                om.SetObstacles(currentPosition, laneLength);

                ChangeActiveLanes();
            }
        }
        SetShaderVariables();
    }

    void ChangeActiveLanes()
    {
        if (GameData.Instance.activeLanes.Count == 12)
        {
            if (Random.Range(0, 1f) < 0.5f)
                GameData.Instance.activeLanes.Remove(GameData.Instance.activeLanes[player.GetComponent<PlayerController>().index]);
        }
        else
        {
            List<int> inactiveLanes = new List<int>();

            for (int i = 0; i < 12; i++)
            {
                if (!GameData.Instance.activeLanes.Contains(i))
                    inactiveLanes.Add(i);
            }

            int toRemove = 0;
            int delta = 0;

            foreach (int currentLane in inactiveLanes)
            {
                float rand = Random.Range(0, 1f);
                int k = -Mathf.FloorToInt(Mathf.Log(rand, 2));

                switch (k)
                {
                    case 1:
                        rand = Random.Range(0, 1f);
                        if (rand < 0.45f)
                            rand = -1;
                        else if (rand > 0.55f)
                            rand = 1;
                        else
                            rand = 0;
                        toRemove = (currentLane + (int)rand) % 12;
                        if (toRemove < 0)
                            toRemove += 12;
                        break;

                    case 2:
                        delta = currentLane % 3;
                        delta--;

                        toRemove = (currentLane + 6) % 12;
                        if (delta == -1)
                            toRemove += 2;
                        else if (delta == 1)
                            toRemove -= 2;
                        break;

                    case 3:
                        delta = currentLane % 3;
                        delta--;

                        if(delta == 0)
                        {
                            if (Random.Range(0, 1f) < 0.5f)
                                toRemove = currentLane - 3;
                            else
                                toRemove = (currentLane + 3) % 12;
                        }
                        else
                        {
                            toRemove = currentLane - delta * 3;
                        }

                        if (toRemove < 0)
                            toRemove += 12;

                        break;
                    case 4:
                        delta = currentLane % 3;
                        delta--;

                        if(Random.Range(0, 1f) < 0.5f)
                            toRemove = currentLane - delta * 4;
                        else
                            toRemove = currentLane - delta * 7;
                        break;
                    default:
                        toRemove = Random.Range(0, 12);
                        break;
                }

                //Add again
                GameData.Instance.activeLanes.Remove(toRemove);

                if (Random.Range(0, 1f) < 0.6f)
                    GameData.Instance.activeLanes.Add(currentLane);
            }
        }
    }

	public void SetShaderVariables()
	{
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].SetFloat("_Speed", player.transform.position.z);
		}

		//materials[i].SetFloat("_StartingTime", Time.time);
	}
}
