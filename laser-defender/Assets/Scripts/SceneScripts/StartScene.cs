using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScene : SceneControllerBase
{
    [SerializeField] private Button _startBtn = null;
    [SerializeField] private Button _quitBtn = null;
    [SerializeField] private string _nextSceneName = null;

    private void Start()
    {
        _startBtn.onClick.AddListener(() => LoadScene(_nextSceneName));
        _quitBtn.onClick.AddListener(Application.Quit);
    }
}
