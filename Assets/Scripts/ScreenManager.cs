using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameScreen
{
    Menu,Game,GameOver
}

public class ScreenManager : MonoBehaviour {
    public static ScreenManager instance;
    public Canvas blipCanvas;

    public GameScreen currentScreen;
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
        blipCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
