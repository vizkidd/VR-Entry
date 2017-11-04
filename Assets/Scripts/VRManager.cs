using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class VRManager : MonoBehaviour {
    Camera mainCamera;

	// Use this for initialization
	void Start () {

    }
    private void Awake()
    {
        //XRSettings.enabled = false;
        XRSettings.showDeviceView = true;
        XRDevice.SetTrackingSpaceType(TrackingSpaceType.Stationary);
        mainCamera = GetComponentInChildren<Camera>();
        InputTracking.Recenter();
    }
    // Update is called once per frame
    void Update () {
        mainCamera.transform.localRotation = InputTracking.GetLocalRotation(XRNode.CenterEye);	
	}
}
