using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public static List<GameObject> wayPointWork = new List<GameObject>();
    private float radiusDragon = 3.5f, distanceDragon, maxDistance = 0.0f, speedDragon = 1.5f, speedDragonRotate = 1.05f;
    public static Vector3 pointEnd, pointSpawn;
    private Vector3 currentPoint;
    public static bool dragonEnd = true;
    private GameObject objectDragon;
    int numberPoint = 0;
    Collider[] colliderDragon;
    private Animator animDragon;
    private Rigidbody RigidDragon;
    void Start()
    {
        animDragon = GetComponent<Animator>();
        RigidDragon = GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (dragonEnd && transform.position != pointSpawn)
        {
          MoveDragon();
        }
    }
    public void MoveDragon()
    {
        animDragon.SetBool("dragonFly", true);
        colliderDragon = Physics.OverlapSphere(transform.position, radiusDragon);
        if (colliderDragon.Length > 0)
        {
            foreach (Collider pointDragon in colliderDragon)
            {
                if (pointDragon.tag == "WayPointEnd" && numberPoint > 4)
                {
                   currentPoint = pointEnd;
                    break;
                }
                if (numberPoint > Random.Range(3f, 6f))
                {
                    currentPoint = pointEnd;
                    break;
                }
                if ((pointDragon.tag == "WayPoint") && (wayPointWork.Contains(pointDragon.gameObject)))
                {

                    distanceDragon = Vector3.Distance(pointDragon.transform.position, pointEnd);
                    
                    if (maxDistance <= distanceDragon)
                    {
                        maxDistance = distanceDragon;
                        currentPoint = pointDragon.transform.position;
                        objectDragon = pointDragon.gameObject;
                    }
                }
            }
        }
        Vector3 targetDragon = currentPoint - transform.position;
        if(targetDragon != new Vector3(0,0,0))
        {
            Quaternion rotateDragon = Quaternion.LookRotation(targetDragon);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotateDragon, speedDragonRotate * Time.deltaTime);
        }
        transform.position = Vector3.MoveTowards(transform.position, currentPoint, speedDragon * Time.deltaTime);

        if (transform.position == currentPoint)
        {
            objectDragon = wayPointWork.Find(obj => obj.transform.position == currentPoint) as GameObject;
            wayPointWork.Remove(objectDragon);
            numberPoint++;
            colliderDragon = Physics.OverlapSphere(transform.position, radiusDragon);
            maxDistance = 0.0f;
            animDragon.SetBool("dragonFly", false);
        }
    }
    public void OnTriggerEnter(Collider stolkn)
    {
        if (stolkn.gameObject.tag == "House")
        {
            numberPoint = 0;
            SpawnDragon.numberDragon--;
            Destroy(gameObject);
        }
        if (stolkn.gameObject.tag == "Animal")
        {
            RigidDragon.AddForce(transform.right * 20f, ForceMode.Impulse);
            transform.position = Vector3.MoveTowards(transform.position, currentPoint, 0.5f * Time.deltaTime);
        }
    }
}
