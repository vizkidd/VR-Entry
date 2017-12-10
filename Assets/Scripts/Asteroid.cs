using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class Asteroid : MonoBehaviour {
    public UnityEvent HealthSet;
    public Canvas asteroidCanvas;
    public ParticleSystem explosion;
    public Renderer asteroidMesh;
    public Transform asteroidTransform;
    public cakeslice.Outline asteroidOutline;
    public GameObject asteroidObj;
    public bool moveTowardsPlayer;
    public float movementSpeed = 1f;
    public float health, healthSteps;
    public Image healthBar;
    public LaserController laser;
    //public GameObject warningBlipObject;


    private bool _isInView,_wasInView;
    public bool IsInView { get { return _isInView; } }

    bool applyDamage, runOnce;
    GazeInput gazeInput;

    #region TODO
    //1.Normalize health and scale. as health decreases, the scale should also decreases
    //2.Create money for asteroid based on scale, money is fixed for small,medium and large asteroids
    #endregion

    void Start () {
        gameObject.tag = "Asteroid";
        asteroidMesh = asteroidObj.GetComponent<MeshRenderer>();
        asteroidTransform = GetComponent<Transform>();
        gazeInput = GetComponent<GazeInput>();
        asteroidCanvas.worldCamera = ScreenManager.instance.uiCamera;
        
        //set random scale
        float randomScaleX = Random.Range(0.5f, 3);
        float randomScaleZ = Random.Range(0.5f, 3);
        float randomScaleY = Random.Range(randomScaleX, randomScaleZ);

        asteroidTransform.localScale = new Vector3(randomScaleX, randomScaleY, randomScaleZ);

        health = Random.Range(50, 100);
        if(movementSpeed!=0)
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

        //Debug.Log("Hide Health");
        healthBar.enabled = false;
        applyDamage = false;
    }
    private void DecreaseHealth()
    {
        //fire laser at asteroid
        laser.FireAtTarget(asteroidObj.transform);

        //Debug.Log("Show Health");
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
            transform.position = Vector3.MoveTowards(asteroidTransform.position, PlayerRef.instance.playerTransform.position, step);
            //if it's moving towards player update IsInView check
            _wasInView = _isInView;
            _isInView = Check_ObjectIsInView();
            //update warning blip list if there is a change in view
            if (_wasInView && !_isInView || _isInView && !_wasInView) //this reads as 'if it was in view & it is not in view now || if it is in view & it wasn't in view'
            {
                WarningBlips.instance.UpdateList(this);
            }
           /* if(_isInView && !_wasInView)
            {
                //asteroid popped into view,show blip
                warningBlipObject.SetActive(true);
                warningBlipObject.GetComponent<EasyTween>().OpenCloseObjectAnimation();
            }
            else if (_wasInView && !_isInView)
            {
                //asteroid popped out of view,hide blip
                warningBlipObject.SetActive(false);
            }*/
        }
        if (applyDamage)
        {
            health -= Time.deltaTime * gazeInput.gazeTime*PlayerRef.instance.powerMultiplier;
            if (health <= 0)
            {
                PlayerRef.IncreaseHealth(5);
                DestroyAsteroid();
            }
        }

        
	}

    public bool Check_ObjectIsInView()
    {
        return asteroidTransform.IsInView(); ;
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
        else if(collision.collider.tag == "DeadZone" || collision.collider.tag == "Asteroid")
        {
            DestroyObject();
        }
    }
    
    public void DestroyObject()
    {
        //for future reference
        StartCoroutine(DestroyObject_CO());
    }

    IEnumerator DestroyObject_CO()
    {
        //remove asteroid from warning blip list
        yield return new WaitUntil(() => { return WarningBlips.instance.RemoveBlip(this); });
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }

    public void DestroyAsteroid()
    {
        applyDamage = false;

        //remove asteroid from warning blip list
        WarningBlips.instance.RemoveBlip(this);

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
