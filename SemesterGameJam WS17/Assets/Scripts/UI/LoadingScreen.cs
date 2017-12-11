using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField]
    string scene;

    private void Start()
    {
        if(scene.Length != 0)
        {
            StartCoroutine(LoadAsynchronously(scene));
        }
    }

    IEnumerator LoadAsynchronously(string name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (Input.anyKeyDown)
                operation.allowSceneActivation = true;
            yield return null;
        }
    }
}
