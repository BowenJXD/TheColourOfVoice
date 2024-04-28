using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLooper : MonoBehaviour
{
    public float speed = 0.1f;
    private Transform child;


    void Start()
    {

    }

    void Update()
    {
        foreach(Transform child in transform)
        {
            Vector3 pos = child.position;


            pos.x -= Time.deltaTime * speed;
            child.position = pos;



            if (child.position.x < -22f)
            {
                pos.x = 23f;
                child.position = pos;
            }

        }


    }
}
