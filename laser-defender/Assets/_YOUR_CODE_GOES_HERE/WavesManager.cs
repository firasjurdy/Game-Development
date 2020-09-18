using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavesManager : MonoBehaviour
{
    public delegate void WaveSpawned(int spawnedWaveNo);
    public event WaveSpawned E_WaveSpawned;

    public delegate void WaveIsOver();
    public event WaveIsOver E_WaveIsOver;

    [SerializeField] private List<Wave> enemyWaves = new List<Wave>();

    public int CurrentWave { get; private set; }

    private bool gameVictory = false;

    private void Start()
    {
        SetupWavesAndSpawn();
        E_WaveIsOver += SpawnNextWave;
    }

    private void Update()
    {
        if (!gameVictory)
        {
            if (enemyWaves != null && CurrentWave <= enemyWaves.Count)
                CheckIfWaveIsOver(enemyWaves[CurrentWave]);
        }      
    }

    public void SetupWavesAndSpawn()
    {      
        //TODO you code goes here
        CurrentWave = 0;
        
        SpawnNextWave();
    }

    public void SpawnNextWave()
    {
        if (CurrentWave < enemyWaves.Count)
            enemyWaves[CurrentWave].SpawnEnemies();
        
        //TODO you code goes here

        E_WaveSpawned?.Invoke(CurrentWave + 1);
    }

    private void CheckIfWaveIsOver(Wave wave)
    {
        if (wave.IsOver())
        {
            GameScore.WavesCleared++;
            
            if (GameScore.WavesCleared == enemyWaves.Count)
            {
                GameScore.SummmaryResult = "YOU ARE VICTORIOUS!!!";               
                FindObjectOfType<GameScene>().EndGame();
                gameVictory = true;
                return;
            }
            else
            {
                CurrentWave++;
            }
                
            E_WaveIsOver?.Invoke();
        }
    }

    //Represents a single enemy wave
    [System.Serializable]
    private class Wave
    {
        [SerializeField] private GameObject[] enemies;

        public void SpawnEnemies()
        {
            foreach (GameObject enemy in enemies)
            {
                ActivateEnemy(enemy);
            }
        }

        public void ActivateEnemy(GameObject enemy)
        {
            enemy.SetActive(true);
        }

        public bool IsOver()
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
