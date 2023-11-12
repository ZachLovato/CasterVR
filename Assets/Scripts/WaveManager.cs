using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private GameObject[] SpawnPoints;
    [SerializeField] private GameObject[] SpawnableHostiles;
    [SerializeField] private List<GameObject> AliveHostiles;

    private float maxAliveCount = 1;
    private float enemyInWaves = 2;
    private float waveCount = 0;

    public delegate void removeHostile(GameObject hostile);
    public static removeHostile onRemoveHostile;

    [SerializeField] private float waveDelay = 5;
    [SerializeField] private float timer = 0;

	private void Start()
	{
        onRemoveHostile += removeDeadHostile;
        spawnHostile(0);
        timer = waveDelay;
	}

	// Update is called once per frame
	void Update()
    {
        // if the amount of enemies that have been killed has reached 0 
        // change to the next wave


        round();

    }

    private void removeDeadHostile(GameObject hostile)
    {
        AliveHostiles.Remove(hostile);
        if (checkEndRound()) createHostiles();
	}

    // fills to number of max alive hostiles
    private void createHostiles()
    {
		while (AliveHostiles.Count - 1 < maxAliveCount)
		{
			spawnHostile(0);
			enemyInWaves--;
		}
	}

    private void changeRoundCount()
    {
        if (waveCount < 1)
        {
            enemyInWaves = 6;
        }
        else if (waveCount < 2) 
        {
            enemyInWaves = 12;
            maxAliveCount++;
        }
        else if (waveCount < 4) 
        {
			enemyInWaves = 15;
			maxAliveCount++;
		}
        else if (waveCount < 8) 
        {
			enemyInWaves = 21;
			maxAliveCount++;
		}        
        else if (waveCount < 12) 
        {
			enemyInWaves = 26;
			maxAliveCount += 3;
		}

        createHostiles();
    }

    // spawns a new enemy at a random spawn point 
    private void spawnHostile(int prefabNum)
    {
        int spawnpoint = Random.Range(0, SpawnPoints.Length);

        AliveHostiles.Add(Instantiate(SpawnableHostiles[prefabNum], SpawnPoints[spawnpoint].transform.position, transform.rotation));
    }

    private void round()
    {
		if (enemyInWaves <= 0 && AliveHostiles.Count == 0)
		{
			if (timer <= 0)
			{
				waveCount++;
				changeRoundCount();
				timer = waveDelay;
			}
			else timer -= Time.deltaTime;
		}
	}

    private bool checkEndRound()
    {
        //if ()

        return false;
    }

}
