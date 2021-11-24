using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDragon : MonoBehaviour
{
    public List<GameObject> prefabDragon = new List<GameObject>();
    public GameObject pointEnd;
    private GameObject dragonHit;
    public List<GameObject> wayPoint = new List<GameObject>();
    private bool spawn = false;
    public static bool swipe = true;
    private int indexDdragon;
    public static int numberDragon = 3;
    private Collider[] colliderSpawn;
    private float radiusSpawn = 0.2f, timerSpawn = 1f;
    private Vector3 targetEnd, screenPoint, curScreenPoint;
    private bool onSpawn = true, camPointerDown = false, hitDragon = false, dragonMove = false;
    private Touch touch;
    void Start()
    {
        Dragon.wayPointWork.AddRange(wayPoint);
        Dragon.pointEnd = pointEnd.transform.position;
        targetEnd = pointEnd.transform.position - transform.position;
        Dragon.pointSpawn = transform.position;
    }
    void Update()
    {
    /*  if (Input.GetMouseButtonDown(0))
      {
           camPointerDown = true;
      }
      if (Input.GetMouseButtonUp(0))
      { 
           camPointerDown = false;
           hitDragon = false;
           dragonHit = null;
      }
      if (camPointerDown)
      {
            MovingDragon();
      }*/

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Animal")
                    {
                        if (dragonHit == null)
                        {
                            dragonHit = hit.collider.gameObject;
                            hitDragon = true;
                            swipe = false;
                        }
                    }
                }
            }
            if (touch.phase == TouchPhase.Moved)
            {
                if (Input.touchCount == 1)
                {
                    MovingDragon();
                }
            }
            if (touch.phase == TouchPhase.Ended)
            {
                hitDragon = false;
                dragonHit = null;
                swipe = true;
            }
        }

        if (dragonMove && hitDragon == false)
      {
            Dragon.dragonEnd = true;
            dragonMove = false;
            spawn = true;
      }
        if (numberDragon > 4)
        {
            numberDragon = 0;
        }
        if (spawn && numberDragon < 4)
       {
           timerSpawn -= 1 * Time.deltaTime;
           if (timerSpawn < 0)
           {
              DragonSpawn();
              timerSpawn = 1f;
           }
        }
       if (Dragon.wayPointWork.Count < 4)
       {
            Dragon.wayPointWork.AddRange(wayPoint);
       }
    }
    public void DragonSpawn()
    {
        colliderSpawn = null;
        onSpawn = true;
        colliderSpawn = Physics.OverlapSphere(transform.position, radiusSpawn);
        if (colliderSpawn.Length > 0)
        {
            foreach (Collider pointSpawn in colliderSpawn)
            {
                if (pointSpawn.tag == "Animal")
                {
                    onSpawn = false;
                    break;
                }
            }
        }
        if (onSpawn)
        {
            indexDdragon = Random.Range(0, prefabDragon.Count);
            Instantiate(prefabDragon[indexDdragon], transform.position, Quaternion.LookRotation(targetEnd));
            spawn = false;
            numberDragon++;
        }
    }
    public void MovingDragon()
    {
        if (hitDragon)
        {
            swipe = false;
            screenPoint = Camera.main.WorldToScreenPoint(dragonHit.transform.position);
          //  curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            curScreenPoint = new Vector3(touch.position.x, touch.position.y, screenPoint.z);
            dragonHit.transform.position = Camera.main.ScreenToWorldPoint(curScreenPoint);
            dragonMove = true;
        }
    }
}
