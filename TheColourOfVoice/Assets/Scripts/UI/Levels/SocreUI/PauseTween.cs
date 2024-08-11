using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseTween : MonoBehaviour
{
    
    [SerializeField] GameObject PauseUI;

    private void Awake()
    {
        LeanTween.reset();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
