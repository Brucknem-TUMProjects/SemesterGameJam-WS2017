using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {

    public List<GameObject> obstacles;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < transform.childCount; i++)
        {
            obstacles.Add(transform.GetChild(i).gameObject);
        }
	}
	
	public void SetObstacles(int currentPosition, int laneLength)
    {
        int obstacleCount = GameData.Instance.activeLanes.Count / 4 + ((Random.Range(0, 1f) < 0.85f) ? 0 : 1);
        for (int i = 0; i < obstacleCount; i++)
        {
            //int z = Random.Range(currentPosition * laneLength, (currentPosition + 1) * laneLength) + laneLength / 2;
            int z = currentPosition * laneLength + (int)((i * 1.0f / obstacleCount) * laneLength);
            int orientation;
            int lane;

            do
            {
                lane = Random.Range(-1, 2);
                orientation = Random.Range(0, 4);
            } while (!GameData.Instance.activeLanes.Contains(orientation * 3 + (lane + 1)));

            Vector2 position = GameData.Instance.LaneAttraction(lane, orientation);
            
            GameObject obstacle = obstacles[0];
            obstacle.transform.position = new Vector3(position.x, position.y, z);
            obstacle.transform.rotation = Quaternion.Euler(0,0, 90 * orientation);

            //obstacle.transform.position += obstacle.transform.up * 1f;
            obstacle.GetComponent<ObstacleScript>().Awake();
            obstacles.Remove(obstacle);
            obstacles.Add(obstacle);
        }
        //print(set);
    }
}
