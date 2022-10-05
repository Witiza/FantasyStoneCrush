using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//Find a way to not make this a monobehaviour
public class SceneHandler : MonoBehaviour
{
    Scene _currentScene;
    [SerializeField] CanvasGroup _canvas;
    [SerializeField] TMP_Text _loadingText;
    [SerializeField] SlicedFilledImage _loadingBar;
    [SerializeField] StringEventBus _loadEvent;
    Coroutine _currentCoroutine = null;

    public void LoadScene(string scene)
    {
        if(_currentCoroutine == null)
            _currentCoroutine = StartCoroutine(LoadSceneCoroutine(scene));
    }

    IEnumerator LoadSceneCoroutine(string scene)
    {
        _canvas.gameObject.SetActive(true);
        AsyncOperation asyncLoad;
        if (_currentScene.isLoaded)
        {
            asyncLoad = SceneManager.UnloadSceneAsync(_currentScene);
            while (!asyncLoad.isDone)
            {
                DisplayLoadingStatus(asyncLoad.progress, "Unloading Previous Scene");
                yield return null;
            }
        }
        asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        while(!asyncLoad.isDone)
        {
            DisplayLoadingStatus(asyncLoad.progress, "Loading Next Scene");
            yield return null;
        }
        _currentScene = SceneManager.GetSceneAt(1);
        _currentCoroutine = null;
        _canvas.gameObject.SetActive(false);
    }

    private void Awake()
    {
        _loadEvent.Event += LoadScene;
    }

    void DisplayLoadingStatus(float amount, string text)
    {
        _loadingBar.fillAmount = amount;
        _loadingText.text = text;
    }
    private void OnDestroy()
    {
        _loadEvent.Event -= LoadScene;
    }
}
