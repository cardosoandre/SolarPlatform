using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class WaveController : MonoBehaviour
{
    public Text titleTxt; 
    [SerializeField] Text timeBeforeWaveText;
    public List<Wave> waves;

    [SerializeField] Transform enemySpawnLocation;


    private int waveNum;

    private void Update()
    {
        if (GameManager.instance.isGameStarted && !GameManager.instance.isLighthouseDestroyed)
        {
            if (waves[waveNum].startDelay > 0)
            {
                waves[waveNum].startDelay -= Time.deltaTime;
                timeBeforeWaveText.text = "Next wave in " + Mathf.RoundToInt(waves[waveNum].startDelay + 1).ToString() + "!";
                return;
            }
            else if (!timeBeforeWaveText.text.Equals(""))
            {
                timeBeforeWaveText.text = "";
                titleTxt.text = waves[waveNum].waveTitle;
            }

            int remaining = 0;
            foreach (Enemies enemies in waves[waveNum].allEnemies)
            {
                if (enemies.startDelay > 0) enemies.startDelay -= Time.deltaTime;
                else if (enemies.timeBetweenSpawns > enemies.timer) enemies.timer += Time.deltaTime;
                else if (enemies.spawnAmount > 0)
                {
                    enemies.spawnAmount--;
                    enemies.timer = 0;
                    Instantiate(
                        enemies.prefab,
                        new Vector3((Random.Range(enemySpawnLocation.localPosition.x - 5, enemySpawnLocation.localPosition.x + 5) * 3), enemies.prefab.transform.position.y, enemySpawnLocation.localPosition.z),
                        enemies.prefab.transform.rotation);
                }
                remaining += enemies.spawnAmount;
            }

            if (remaining == 0)
            {
                waveNum++;
                if (waveNum == waves.Count) //if true then end game
                {
                    this.enabled = false;
                }
                else titleTxt.text = waves[waveNum].waveTitle;

            }
        }
    }
}

[Serializable]
public class Wave
{
    public string waveTitle;
    public float startDelay;
    public List<Enemies> allEnemies;
}

[Serializable]
public class Enemies
{
    public GameObject prefab;
    public float startDelay;
    public float timeBetweenSpawns;
    public int spawnAmount;
    
    [HideInInspector] public float timer;
    
}
