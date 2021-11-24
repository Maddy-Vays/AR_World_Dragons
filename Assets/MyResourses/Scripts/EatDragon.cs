using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatDragon : MonoBehaviour
{
    public List<GameObject> target = new List<GameObject>();
    public List<GameObject> apple = new List<GameObject>();
    private Animator animDragonEat;
    private float speed = 0.5f, timer = 3f;
    private int index = 0;
    private Quaternion rotation = Quaternion.identity;
    private Rigidbody bodyDragon;
    private bool end = false;

    void Start()
    {
        animDragonEat = GetComponent<Animator>();
        animDragonEat.SetBool("dragonSearch", true);
        bodyDragon = GetComponent<Rigidbody>();
    }
    void Update()
    {
        
        if (index < target.Count)
        {
            Vector3 targetFood = target[index].transform.position - transform.position;
            if (targetFood != new Vector3(0, 0, 0))
            {
                Quaternion rotateDragon = Quaternion.LookRotation(targetFood);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotateDragon, speed * Time.deltaTime);
            }
                transform.position = Vector3.MoveTowards(transform.position, target[index].transform.position, speed * Time.deltaTime);
            if (index == target.Count - 1)
            {
                rotation.eulerAngles = new Vector3(transform.eulerAngles.x - 1.5f, transform.eulerAngles.y, transform.eulerAngles.z);
                transform.rotation = rotation;
                bodyDragon.constraints = RigidbodyConstraints.FreezeRotation;
                end = true;
            }
            if (transform.position == target[index].transform.position)
            {
                if (index != target.Count - 1)
                {
                    animDragonEat.SetBool("dragonSearch", false);
                    animDragonEat.SetBool("dragonEat", true);
                    timer -= 1 * Time.deltaTime;
                    if (timer < 0)
                    {
                        timer = 3f;
                        apple[index].SetActive(false);
                        animDragonEat.SetBool("dragonEat", false);
                        animDragonEat.SetBool("dragonSearch", true);
                        index++;
                    }
                }
                if (end)
                {
                    index++;
                    animDragonEat.SetBool("dragonSearch", false);
                    animDragonEat.SetBool("dragonEat", true);
                }
            }
        }
    }
}
