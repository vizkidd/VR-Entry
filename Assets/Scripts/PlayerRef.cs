using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;

public class PlayerRef : MonoBehaviour {
    public static PlayerRef instance;

    public Transform playerTransform;
    public int health = 100;
    public int powerMultiplier=6;
    public GameObject gameOverPanel;
    public DigitalGlitch glitch;
    public static bool gameOver;

    bool runOnce;
    //public static int score, scoreMultiplier = 100;

    private void Awake()
    {
        MakeSingleton();
    }
    void MakeSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Use this for initialization
    void Start () {
        playerTransform = GetComponent<Transform>();	
	}
	public static void DecreaseHealth(int value)
    {
        PlayerRef.instance.health -= value;
    }
    public static void IncreaseHealth(int value)
    {
        PlayerRef.instance.health += value;
    }
    void Update () {
        //recente input tracking after game begins, implement wisely in future
        //InputTracking.Recenter();
        
        if (health<=0)
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
