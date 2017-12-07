using cakeslice;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    Transform enemyTransform;
    public GameObject asteroidPrefab;
    public int maxAggressiveObjects=1;
    public static List<Asteroid> aggressiveEnemies;
    public static List<GameObject> enemies;
    public static int maxCount = 100;
    public static int timeInverval=1;
    float timer;
    
    public int Count
    {
        get { return enemies.Count; }
    }

    // Use this for initialization
    void Start () {
        aggressiveEnemies = new List<Asteroid>();
        enemyTransform = GetComponent<Transform>();
        enemyTransform.LookAt(PlayerRef.playerTransform);
        enemies = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!PlayerRef.gameOver)
        {
            if (timer >= timeInverval)
            {
                if (Count < maxCount)
                {

                    //spawn
                    GameObject temp = Instantiate(asteroidPrefab, new Vector3(Random.insideUnitCircle.x * (Screen.width / 2), Random.insideUnitCircle.y * (Screen.height / 2), enemyTransform.position.z), Quaternion.identity, enemyTransform) as GameObject;
                    //temp.GetComponent<Outline>().enabled = false;
                    if (temp != null)
                        enemies.Add(temp);
                    //remove nulls after creating an asteroid
                    enemies = enemies.Where(item => item != null).ToList();

                }

                //move number of asteroids towards player
                if (Count > 2 && aggressiveEnemies.Count < maxAggressiveObjects)
                {
                    MoveTowardsPlayer();
                }
                else
                {
                    //remove nulls from Aggressive asteroids
                    aggressiveEnemies = aggressiveEnemies.Where(item => item != null).ToList();
                    aggressiveEnemies.TrimExcess();
                }
                timer = 0;
            }

            timer += Time.deltaTime;
        }
        else
        {
            StartCoroutine(DestroyAsteroidEach_CO());
        }
	}

    public static IEnumerator GetAggressiveEnemies_InView()
    {
        yield return aggressiveEnemies.Where(item => item.IsInView == true).ToArray();
    }

    IEnumerator GetAggressiveEnemies()
    {
        yield return aggressiveEnemies.ToArray();
    }

    IEnumerator DestroyAsteroidEach_CO()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < Count; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].GetComponent<Asteroid>().DestroyAsteroid();
                yield return new WaitForSeconds(0.2f);
                yield return null;
            }
        }
        enemies.Clear();
        aggressiveEnemies.Clear();
    }
    void MoveTowardsPlayer()
    {

            GameObject temp = enemies[Random.Range(0, Count)];

        if (temp != null)
        {
            Asteroid tmpObj= temp.GetComponent<Asteroid>();
            tmpObj.moveTowardsPlayer = true;
            tmpObj.asteroidObj.GetComponent<Outline>().enabled = true;
            aggressiveEnemies.Add(tmpObj);
        }
        
    }
}
