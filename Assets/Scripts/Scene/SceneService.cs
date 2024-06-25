using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneService
{
    #region Description

    public event Action HideVisualEvent;

    public event Action SceneLoadedEvent;

    private List<string> _nameNextScene = new List<string>();

    #endregion

    public SceneService()
    {
    }

    public async UniTask OpenScene(string nameScene)
    {
        await HideVisual();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(nameScene);
    }

    public void SetupNextScene(string nameScene)
    {
        _nameNextScene.Add(nameScene);
    }

    public void ClearNextScenes()
    {
        _nameNextScene.Clear();
    }


    public async UniTask OpenNextScene()
    {
        if (_nameNextScene.Count > 0)
        {
            await HideVisual();
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(_nameNextScene[0]);
            _nameNextScene.RemoveAt(0);
        }
        else
        {
            Debug.LogError("Next scene not set!");
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneLoadedEvent?.Invoke();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private async UniTask HideVisual()
    {
        HideVisualEvent?.Invoke();
        await UniTask.Delay(2200);
    }
}
