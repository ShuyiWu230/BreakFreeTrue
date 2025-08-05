using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneChanger : MonoBehaviour
{
    public int videoLength;

    void Start()
    {
        StartCoroutine(waitChangeScene());
    }

    IEnumerator waitChangeScene() 
    {
        yield return new WaitForSeconds(videoLength);

        SceneManager.LoadScene(1);
    }
}
