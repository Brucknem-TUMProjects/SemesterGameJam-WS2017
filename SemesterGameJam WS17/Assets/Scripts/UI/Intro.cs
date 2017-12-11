using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    [SerializeField]
    LoadingScreen load;
    [SerializeField]
    AudioSource[] audioSources;
    [SerializeField]
    Camera cam;
    [SerializeField]
    float zoomSpeed;

    public void PlayAudio(int i)
    {
        audioSources[i].Play();
    }

    public void EndIntro()
    {
        StartCoroutine(Zoom());
    }

    IEnumerator Zoom()
    {
        cam.orthographicSize = Mathf.Max(0, cam.orthographicSize - zoomSpeed);
        yield return null;
    }
}
