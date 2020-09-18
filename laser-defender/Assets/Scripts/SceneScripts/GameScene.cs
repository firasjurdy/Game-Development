using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScene : SceneControllerBase
{
    [SerializeField] private Text _currentScoreLabel = null;
    [SerializeField] private Text _currentWaveNoLabel = null;
    
    [SerializeField] private Slider _slider = null;
    [SerializeField] private Image _fillImage = null;
	
    [SerializeField] private Color _fillNormalColor = Color.white;
    [SerializeField] private Color _fillFullColor = Color.white;
    [SerializeField] private Color _fillLowColor = Color.white;
    
    private PlayerShip _playerShip;
    private float _energyPercentage;
    
    private void Start()
    {       
        GameScore.WavesCleared = 0;
        GameScore.ResetScore();
        GameScore.SummmaryResult = "GAME OVER!!!";
        GameScore.E_ScoreUpdated += UpdateScoreLabel;
        
        _playerShip = FindObjectOfType<PlayerShip>();
        _playerShip.SetupAndStart();
        _energyPercentage = 1f;
        
        _currentWaveNoLabel.enabled = false;

        var wavesManager = FindObjectOfType<WavesManager>();
        wavesManager.E_WaveSpawned += ShowWaveNo;
    }

    private void Update () 
    {
        _energyPercentage = _playerShip.CurrentEnergy / _playerShip.StartingEnergy;
        _slider.value = _energyPercentage;

        if (_energyPercentage >= 0.95f) _fillImage.color = _fillFullColor;
        else if (_energyPercentage < 0.95f && _energyPercentage > 0.15f) _fillImage.color = _fillNormalColor;
        else if (_energyPercentage <= 0.15f) _fillImage.color = _fillLowColor;
        else _fillImage.color = _fillNormalColor;
    }
    
    private void ShowWaveNo(int waveNo)
    {
        StartCoroutine(CoR_ShowWaveNo(waveNo));
    }

    private IEnumerator CoR_ShowWaveNo(int waveNo)
    {       
        _currentWaveNoLabel.text = $"Wave {waveNo}";
        _currentWaveNoLabel.enabled = true;
        yield return new WaitForSeconds(2f);
        _currentWaveNoLabel.enabled = false;
    }

    private void UpdateScoreLabel()
    {
        _currentScoreLabel.text = GameScore.CurrentScore.ToString();
    }

    public void EndGame()
    {
        StartCoroutine(CoR_WaitAndLoadSummary());
    }
    
    private IEnumerator CoR_WaitAndLoadSummary()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("2_Summary");
    }

    private void OnDestroy()
    {
        GameScore.E_ScoreUpdated -= UpdateScoreLabel;
        var wavesManager = FindObjectOfType<WavesManager>();
        if (wavesManager != null) wavesManager.E_WaveSpawned -= ShowWaveNo;
    }
}
