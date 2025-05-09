using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Loading : MonoBehaviour
{
    public int NextSceneIndex = 2;

    public Slider ProgressSlider;

    public TextMeshProUGUI ProgressText;

    private void Start()
    {
        StartCoroutine(LoadNextSceneRoutine());
    }

    private IEnumerator LoadNextSceneRoutine()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(NextSceneIndex);
        ao.allowSceneActivation = false;

        while(ao.isDone == false)
        {
            //비동기로 실행할 코드
            Debug.Log(ao.progress);
            ProgressSlider.value = ao.progress;
            ProgressText.text = $"{"Waiting For Map" + ao.progress * 100}";


            if(ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
