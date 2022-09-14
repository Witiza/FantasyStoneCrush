using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Find a way to not make this a monobehaviour
public class SceneHandler : MonoBehaviour
{
    Scene _currentScene;
    [SerializeField] CanvasGroup _canvas;
    [SerializeField] SlicedFilledImage _loadingBar;
    [SerializeField] StringEventBus _loadEvent;
    Coroutine _currentCoroutine;

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
                _loadingBar.fillAmount = asyncLoad.progress;
                yield return null;
            }
        }
        asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        while(!asyncLoad.isDone)
        {
            _loadingBar.fillAmount = asyncLoad.progress;
            yield return null;
        }
        _currentScene = SceneManager.GetSceneAt(1);
        _currentCoroutine = null;
        _canvas.gameObject.SetActive(false);
    }

    private void Awake()
    {
        _loadEvent.Event += LoadScene;
        //if (SceneManager.sceneCount==1)
        //{
        //    LoadScene("MainMenu");
        //}
        //else
        //{
        //    _currentScene = SceneManager.GetSceneAt(1);
        //}

    }

    private void OnDestroy()
    {
        _loadEvent.Event -= LoadScene;
    }
}
