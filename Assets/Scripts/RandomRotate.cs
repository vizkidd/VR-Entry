using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : MonoBehaviour {
    Transform asteroidTransform;
    public float maxRotationSpeed;
    private Vector3 rotSpeed;
    // Use this for initialization
    void Start () {
        asteroidTransform = GetComponent<Transform>();
        if (maxRotationSpeed == -1)
        {
            maxRotationSpeed = Random.Range(5, 40);
        }
        rotSpeed = Random.insideUnitSphere * maxRotationSpeed;
    }

  
    // Update is called once per frame
    void Update () {
        asteroidTransform.Rotate(rotSpeed * Time.deltaTime);
    }
}
