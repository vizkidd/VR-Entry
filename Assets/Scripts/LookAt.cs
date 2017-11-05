using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {
    public Transform target;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (target != null)
        {
            Vector3 tempPos = 2 * transform.position - target.position;
            transform.LookAt(new Vector3(tempPos.x, -transform.position.y, tempPos.z), transform.up);
        }
    }
}
