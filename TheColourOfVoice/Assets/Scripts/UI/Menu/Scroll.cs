using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    [Tooltip("Move speed"), Range(0.01f, 1f)]
    public float moveSpeed;
    private SpriteRenderer render;
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        BgScroll();
    }

    public void BgScroll()
    {

        render.material.mainTextureOffset += new Vector2(moveSpeed * Time.deltaTime, 0);
    }

}
