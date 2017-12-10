using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;

public class WarningBlips : MonoBehaviour {

    public static WarningBlips instance;
    public GameObject blipPrefab;
    public Sprite inViewSprite, notInViewSprite;
    //Vector2 screenPos;
    RectTransform CanvasRect;

    bool updatingLists,renderingBlips;

    //have it in a dictionary with asteroid as key and image as value to get blip image easily
    Dictionary<Asteroid,GameObject> blipList;

    #region TODO
    //1.Seperate asteroids based on IsInView and add the asteroids in view to a list
    //2.1.Render warning blips on all the asteroids in that list
    //2.2.Render outOfScreen blip for the asteroid which is the closest and not in view
    //3.Generate new blip images for all the asteroids in list
    #endregion

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
    
    void Start () {
        CanvasRect = ScreenManager.instance.blipCanvas.GetComponent<RectTransform>();
        blipList = new Dictionary<Asteroid, GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
        /*screenPos = Camera.main.WorldToViewportPoint(target.position);
        Vector2 WorldObject_ScreenPosition = new Vector2(((screenPos.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)), ((screenPos.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
        
        img.rectTransform.anchoredPosition = WorldObject_ScreenPosition;
        //set size of blip wrt the size of the mesh
        Rect rectangle = Get2DBoundsOf3DMesh(target.gameObject);
        img.rectTransform.sizeDelta = new Vector2(rectangle.width, rectangle.height);*/
        RenderBlips();
    }

    void RenderBlips()
    {   
        //only render blips if it is not already rendering
        if (!renderingBlips)
            StartCoroutine(RenderBlip_CO());
    }

    IEnumerator RenderBlip_CO()
    {
        renderingBlips = true;
        //wait until list is updated
        yield return new WaitUntil(() => { return !updatingLists; });
        blipList = blipList.Where(item => item.Key != null).ToDictionary(t => t.Key, t => t.Value);
        Debug.Log("==================BLIP LIST======================");

        foreach (var item in blipList)
        {            
            Vector2 screenPos = Camera.main.WorldToViewportPoint(item.Key.asteroidTransform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(((screenPos.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)), ((screenPos.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));
            Image tempBlipImg = item.Value.GetComponent<Image>();
            tempBlipImg.rectTransform.anchoredPosition = WorldObject_ScreenPosition;
            //set size of blip wrt the size of the mesh
            Rect rectangle = Get2DBoundsOf3DMesh(item.Key.asteroidMesh);
            tempBlipImg.rectTransform.sizeDelta = new Vector2(rectangle.width / 2, rectangle.height / 2);
        }

        renderingBlips = false;
        yield return null;
    }

    public bool RemoveBlip(Asteroid asteroid)
    {
        StartCoroutine(RemoveBlip_CO(asteroid));
        return true;
    }
    IEnumerator RemoveBlip_CO(Asteroid asteroid)
    {
        yield return new WaitUntil(() => { return !updatingLists; });
        if (blipList.ContainsKey(asteroid))
        {
            Destroy(blipList[asteroid].gameObject);
            blipList.Remove(asteroid);
        }
    }

    public void UpdateList(Asteroid asteroid)
    {
        StartCoroutine(UpdateList_CO(asteroid));
    }

    IEnumerator UpdateList_CO(Asteroid asteroid)
    {
        yield return new WaitUntil(() => { return !renderingBlips; });
        updatingLists = true;

        //remove nulls from dict here
        //UPDATE: no need of null check because we remove item after asteroid is destroyed
        blipList = blipList.Where(item => item.Key != null).ToDictionary(t => t.Key, t => t.Value);

        if (blipList.ContainsKey(asteroid))
        {
            //change image of blip based on asteroid's IsInView
            Image tempBlipImg = blipList[asteroid].GetComponent<Image>();

            switch (asteroid.IsInView)
            {
                case true:
                    tempBlipImg.sprite = inViewSprite;
                    break;
                case false:
                    tempBlipImg.sprite = notInViewSprite;
                    break;
                default:
                    break;
            }
        }
        {
            //create new blip image and assign based on asteroid's view
            GameObject tempBlipGO = Instantiate(blipPrefab);
            tempBlipGO.transform.SetParent(ScreenManager.instance.blipCanvas.transform, false);
            Image tempBlipImg = tempBlipGO.GetComponent<Image>();

            switch (asteroid.IsInView)
            {
                case true:
                    tempBlipImg.sprite = inViewSprite;
                    break;
                case false:
                    tempBlipImg.sprite = notInViewSprite;
                    break;
                default:
                    break;
            }
            tempBlipImg.preserveAspect = true;
            tempBlipImg.enabled = true;
            if (blipList.ContainsKey(asteroid))
            { blipList.Add(asteroid, tempBlipGO); }
            else
            {
                blipList[asteroid] = tempBlipGO;
            }

        }
        updatingLists = false;
        yield return null;
    }

    public static Rect Get2DBoundsOf3DMesh(Renderer go)
    {
        /* // Get mesh origin and farthest extent (this works best with simple convex meshes)
         Vector3 origin = Camera.main.WorldToScreenPoint(new Vector3(bounds.min.x, bounds.max.y, 0f));
         Vector3 extent = Camera.main.WorldToScreenPoint(new Vector3(bounds.max.x, bounds.min.y, 0f));

         // Create rect in screen space and return - does not account for camera perspective
         return new Rect(origin.x, Screen.height - origin.y, extent.x - origin.x, origin.y - extent.y);*/

        //Vector3 cen = go.GetComponent<Renderer>().bounds.center;
        //Vector3 ext = go.GetComponent<Renderer>().bounds.extents;

        Vector3 cen = go.bounds.center;
        Vector3 ext = go.bounds.extents;

        Vector2[] extentPoints = new Vector2[8]
        {
         WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z-ext.z)),
         WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z-ext.z)),
         WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y-ext.y, cen.z+ext.z)),
         WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y-ext.y, cen.z+ext.z)),
         WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z-ext.z)),
         WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z-ext.z)),
         WorldToGUIPoint(new Vector3(cen.x-ext.x, cen.y+ext.y, cen.z+ext.z)),
         WorldToGUIPoint(new Vector3(cen.x+ext.x, cen.y+ext.y, cen.z+ext.z))
        };
        Vector2 min = extentPoints[0];
        Vector2 max = extentPoints[0];
        foreach (Vector2 v in extentPoints)
        {
            min = Vector2.Min(min, v);
            max = Vector2.Max(max, v);
        }
        return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
    }

    public static Vector2 WorldToGUIPoint(Vector3 world)
    {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(world);
        screenPoint.y = (float)Screen.height - screenPoint.y;
        return screenPoint;
    }
}
