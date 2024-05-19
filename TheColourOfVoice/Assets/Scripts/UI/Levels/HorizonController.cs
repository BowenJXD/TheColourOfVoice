using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HorizonController : MonoBehaviour
{
    public HorizontalLayoutGroup layoutGroup;
    public GameObject childPrefab;

    void Start()
    {
        layoutGroup.spacing = 20f;

        for (int i = 0; i < 3; i++)
        {
            GameObject child = Instantiate(childPrefab, layoutGroup.transform);
            child.GetComponent<Text>().text = "Child " + (i + 1);
        }
    }
}
