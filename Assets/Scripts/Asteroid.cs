using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Asteroid : MonoBehaviour {
    public UnityEvent HealthSet;
    public ParticleSystem explosion;
    //public MeshRenderer asteroidMesh;
    Transform asteroidTransform;
    public cakeslice.Outline asteroidOutline;
    public GameObject asteroidObj;
    public bool moveTowardsPlayer;
    public float movementSpeed = 1f;
    public float health, healthSteps;
    public Image healthBar;
    public LaserController laser;

    bool applyDamage, runOnce;
    GazeInput gazeInput;
    // Use this for initialization
    void Start () {
        asteroidTransform = GetComponent<Transform>();
        gazeInput = GetComponent<GazeInput>();
        //asteroidMesh = asteroidObj.GetComponent<MeshRenderer>();
        health = Random.Range(50, 100);
        movementSpeed = Random.Range(60, 120);
        if (HealthSet != null)
        {
            HealthSet.Invoke();
        }
        healthSteps = health/gazeInput.gazeTime;
        
        gazeInput.PointerEnter.AddListener(DecreaseHealth);
        gazeInput.PointerExit.AddListener(HideHealthBar);

        //set laser
        laser = LaserController.instance;

        //hide health
        healthBar.enabled = false;
    }

    void HideHealthBar()
    {
        //stop firing 
        laser.StopFiring();

        Debug.Log("Hide Health");
        healthBar.enabled = false;
        applyDamage = false;
    }
    private void DecreaseHealth()
    {
        //fire laser at asteroid
        laser.FireAtTarget(asteroidObj.transform);

        Debug.Log("Show Health");
        healthBar.enabled = true;
        applyDamage = true;
        Debug.Log("Decrease Health");
    }
    // Update is called once per frame
    void Update () {
        if (!moveTowardsPlayer)
        {
            
            asteroidTransform.position += Vector3.back * Time.deltaTime * movementSpeed;
        }
        else
        {
            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(asteroidTransform.position, PlayerRef.playerTransform.position, step);
        }
        if(applyDamage)
        {
            health -= Time.deltaTime * gazeInput.gazeTime*PlayerRef.powerMultiplier;
            if (health <= 0)
            {
                PlayerRef.IncreaseHealth(5);
                DestroyAsteroid();
            }
        }

        
	}
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            if (!runOnce)
            {
                PlayerRef.DecreaseHealth(35);
                runOnce = true;
            }
            DestroyAsteroid();
        }
        else if(collision.collider.tag == "DeadZone")
        {
            Destroy(this.gameObject);
        }
    }
    
    public void DestroyAsteroid()
    {
        applyDamage = false;
        StartCoroutine(PlayExplosion());
        
    }
    IEnumerator PlayExplosion()
    {
        //play animation
        asteroidObj.GetComponent<MeshRenderer>().enabled = false;
        asteroidOutline.enabled = false;
        explosion.Play();
        yield return new WaitForSeconds(explosion.main.duration);
        Destroy(this.gameObject);
    }
}
