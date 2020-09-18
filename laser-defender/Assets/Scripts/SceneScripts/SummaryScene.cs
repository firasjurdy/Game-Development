using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SummaryScene : SceneControllerBase
{
    [SerializeField] private Button _tryAgainBtn = null;
    [SerializeField] private Button _quitBtn = null;
    
    [SerializeField] private Text _scoreValue = null;
    [SerializeField] private Text _wavesValue = null;
    [SerializeField] private Text _title = null;

    [SerializeField] private string _playAgainSceneName = null;
    [SerializeField] private string _mainMenuSceneName = null;

    private void Start()
    {
        _tryAgainBtn.onClick.AddListener(() => LoadScene(_playAgainSceneName));
        _quitBtn.onClick.AddListener(() => LoadScene(_mainMenuSceneName));

        _scoreValue.text = GameScore.CurrentScore.ToString();
        _wavesValue.text = GameScore.WavesCleared.ToString();
        _title.text = GameScore.SummmaryResult;
    }

}
