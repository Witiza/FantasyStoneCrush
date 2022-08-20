using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Find a way to not make this a monobehaviour
public class SceneHandler : MonoBehaviour
{
    Scene _currentScene;
    [SerializeField] Camera _camera;
    [SerializeField] CanvasGroup _canvas;
    [SerializeField] SlicedFilledImage _loadingBar;
    [SerializeField] StringEventBus _eventBus;
    Coroutine _currentCoroutine;

    private void Awake()
    {
        _eventBus.Event += LoadScene;
        LoadScene("Test 1");
    }
    public void ReloadScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(string scene)
    {
        if(_currentCoroutine == null)
            _currentCoroutine = StartCoroutine(LoadSceneCoroutine(scene));
    }

    IEnumerator LoadSceneCoroutine(string scene)
    {
        _canvas.gameObject.SetActive(true);
        Camera.SetupCurrent(_camera);
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
        yield return new WaitForSeconds(3);
        _currentScene = SceneManager.GetSceneAt(1);
        _currentCoroutine = null;
        _canvas.gameObject.SetActive(false);
    }
}
