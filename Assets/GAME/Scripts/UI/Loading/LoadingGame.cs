using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingGame : MonoBehaviour
{
    public Image LoadingBarFill;
    void Start()
    {
        StartCoroutine(LoadSceneTestFirstRun(1));//load level 1
    }
    private IEnumerator LoadSceneTestFirstRun(int load)
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(load.ToString());
        operation.allowSceneActivation = false;
        float time = 0;
        
        while (time <=6)
        {
            time += Time.deltaTime;
            float progressValue = Mathf.Clamp01((time / 6f));
            LoadingBarFill.fillAmount = progressValue;
            yield return null;
        }
        operation.allowSceneActivation = true;
    }
}
