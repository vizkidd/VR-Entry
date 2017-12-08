using System.Collections;
using System.Collections.Generic;
using VolumetricLines;
using UnityEngine;
using UnityEngine.Events;

public class LaserController : MonoBehaviour {
    /* public Transform targetTransform;

     public void SetTargetTransform(GameObject target)
     {
         targetTransform = target.transform;
     }

     public void ResetTargetTransform()
     {
         targetTransform = null;
     }
     */
    public Transform targetTransform;
    public Vector3 startPoint;
    public float thresholdDistance;
    public VolumetricLineBehavior laserBeam;
    public particleAttractorLinear particleAttractor;
    public Transform particleAttractorTransform;
    public bool isRunning;
    ParticleSystem flareParticles;
    public float maxParticles,speedMultiplier=1f;
    ParticleSystem.EmissionModule main;
    public bool updateTransform;
    public static LaserController instance;
    float distance;

    void Awake()
    {
        MakeSingleton();
    }

    void MakeSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start () {
        /*flareParticles = GetComponent<ParticleSystem>();
        main = flareParticles.emission;
        maxParticles = main.rateOverTime.constantMax;
        main.rateOverTime = 0;
        StartFlare();*/
    }
	
	// Update is called once per frame
	void Update () {
		if(updateTransform)
        {
            //set beam end point here
            //calculate distance. if distance < thresholdDistance switch off laser
            distance=Vector3.Distance(startPoint, transform.position);
            if (targetTransform != null && distance > thresholdDistance)
            {
                laserBeam.SetStartAndEndPoints(startPoint, new Vector3(startPoint.x,3/* focus beam to center of reticle*/,targetTransform.transform.position.z-5f));
            }
            else
                StopFiring();
        }
	}
    
    public void FireAtTarget(Transform transform)
    {
        particleAttractor.SetTransform(particleAttractorTransform);
        targetTransform = transform;
        updateTransform = true;
    }
    public void StopFiring()
    {
        targetTransform = null;
        particleAttractor.UnsetTransform();
        updateTransform = false;
        laserBeam.SetStartAndEndPoints(startPoint, startPoint);
    }
    public void StartFlare()
    {
        StartCoroutine(StartFlare_CO());
    }
    public void StopFlare()
    {
        StartCoroutine(StopFlare_CO());
    }
    IEnumerator StartFlare_CO()
    {
        //check if already running before starting;
        if (!isRunning)
        { isRunning = true; }
        else
        {
            yield return new WaitUntil(() => { return !isRunning; });
            isRunning = true;
        }
        yield return new WaitForEndOfFrame();
        for (int i=0;i<maxParticles;i++)
        {
            main.rateOverTime = i*speedMultiplier;
            yield return null;
            yield return new WaitForEndOfFrame();
        }

    }

    IEnumerator StopFlare_CO()
    {
        //check if already stopped before stopping;
        if (isRunning)
        { isRunning = false; }
        else
        {
            yield return new WaitUntil(() => { return isRunning; });
            isRunning = false;
        }
        yield return new WaitForEndOfFrame();
        for (int i = (int)maxParticles; i >-1; i--)
        {
            main.rateOverTime = i*speedMultiplier;
            yield return null;
            yield return new WaitForEndOfFrame();
        }

    }
}
