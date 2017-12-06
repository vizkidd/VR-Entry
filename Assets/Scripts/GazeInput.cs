using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class GazeInput : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler {
    public float gazeTime=2f;
    public UnityEvent PointerEnter,PointerExit,GazeComplete;
    public bool continuous;
    bool gazing;
    public float timer;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Enter");
        gazing = true;
        if(PointerEnter!=null)
        {
            PointerEnter.Invoke();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Exit");
        gazing = false;
        timer = 0;
        if (PointerExit != null)
        {
            PointerExit.Invoke();
        }
    }

    // Use this for initialization
    void Start () {
        gazeTime = Random.Range(2, 8);
	}
	
	// Update is called once per frame
	void Update () {
        if (gazing)
        {
            if (!continuous)
            {
                if (timer <= gazeTime)
                    timer += Time.deltaTime;
                else
                {
                    if (GazeComplete != null)
                    {
                        GazeComplete.Invoke();

                    }
                    //Debug.Log("Gazed");

                    timer = 0;
                    gazing = false;
                }
            }
            else
            {
                timer += Time.deltaTime;
            }

        }

	}
}
