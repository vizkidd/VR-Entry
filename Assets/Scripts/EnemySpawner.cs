using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class EnemySpawner : MonoBehaviour {
    Transform transform;
    public GameObject asteroidPrefab;
    Asteroid objMovingToPlayer;
    public List<GameObject> asteroids;
    public static int maxCount = 100;
    public static int timeInverval=1;
    float timer;
    
    public int Count
    {
        get { return asteroids.Count; }
    }

    // Use this for initialization
    void Start () {
        transform = GetComponent<Transform>();
        transform.LookAt(PlayerRef.playerTransform);
        asteroids = new List<GameObject>();
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
                    GameObject temp = Instantiate(asteroidPrefab, new Vector3(Random.insideUnitCircle.x * (Screen.width / 2), Random.insideUnitCircle.y * (Screen.height / 2), transform.position.z), Quaternion.identity, transform) as GameObject;
                    //temp.GetComponent<Outline>().enabled = false;
                    if (temp != null)
                        asteroids.Add(temp);

                    //Debug.Log(Count);

                }
                else
                {
                    for (int i = 0; i < Count; i++)
                    {
                        if (asteroids[i] == null)
                        {
                            asteroids.RemoveAt(i);
                        }
                    }


                }
                if (Count > 2 && objMovingToPlayer == null)
                {
                    MoveTowardsPlayer();
                }
                timer = 0;

            }

            timer += Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < Count; i++)
            {
                if (asteroids[i] != null)
                {
                    asteroids[i].GetComponent<Asteroid>().DestroyAsteroid();
                }
            }
        }
	}
    void MoveTowardsPlayer()
    {

            GameObject temp = asteroids[Random.Range(0, Count)];
        if (temp != null)
        {
            objMovingToPlayer = temp.GetComponent<Asteroid>();
            objMovingToPlayer.moveTowardsPlayer = true;
            objMovingToPlayer.asteroidObj.GetComponent<Outline>().enabled = true;

        }
        
    }
}
