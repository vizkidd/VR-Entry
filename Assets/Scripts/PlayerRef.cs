using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRef : MonoBehaviour {
    public static Transform playerTransform;
    public static int health = 100;
    public static int powerMultiplier=2;
    public GameObject gameOverPanel;
    public static bool gameOver;
    bool runOnce;
    //public static int score, scoreMultiplier = 100;
	// Use this for initialization
	void Start () {
        playerTransform = GetComponent<Transform>();	
	}
	public static void DecreaseHealth(int value)
    {
        health -= value;
    }
    public static void IncreaseHealth(int value)
    {
        health += value;
    }
    void Update () {
		if(health<=0)
        {
            //gameover
            if(!runOnce)
            {
                gameOverPanel.SetActive(true);
                gameOver = true;
            }
        }
	}
}
