/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMethods 
{
    private static UIMethods instance;
    public static UIMethods GetInstance()
    {
        if(instance == null)
        {
            instance = new UIMethods();
            return instance;
        }
        else
        {
            return instance;
        }
    }

    public GameObject FindCanvas()
    {
        GameObject gameObject = GameObject.FindObjectOfType<Canvas>().gameObject;

        if(gameObject == null)
        {
            Debug.LogError("no canvas can find");
            return gameObject;
        }
        return gameObject;
    }

    public GameObject FindObjectInChild(GameObject panel,string child_name)
    {
        Transform[] transforms = panel.GetComponentsInChildren<Transform>;
        foreach (var tra in transforms){
            if (tra.gameObject.name == child_name)
            {
                return tra.gameObject; 
            }
        }
        Debug.LogWarning($"no find ");
        return null;
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIMethods
{
    private static UIMethods instance;
    public static UIMethods GetInstance()
    {
        if (instance == null)
        {
            instance = new UIMethods();
        }
        return instance;
    }



    public GameObject FindObjectInChild(GameObject Find_Panel, string Find_Name)
    {
        Transform[] transforms_find = Find_Panel.GetComponentsInChildren<Transform>();

        foreach (Transform tra in transforms_find)
        {
            if (tra.gameObject.name == Find_Name)
            {
                return tra.gameObject;
                break;
            }
        }

        return null;
    }


    public GameObject FindCanvas()
    {
        GameObject gameObject_canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
        if (gameObject_canvas == null)
        {
            Debug.LogError("Canvas");
        }

        return gameObject_canvas;
    }


    public T AddOrGetComponent<T>(GameObject Get_Obj) where T : Component
    {
        if (Get_Obj.GetComponent<T>() != null)
        {
            return Get_Obj.GetComponent<T>();
        }

        return null;
    }



    public T GetOrAddSingleComponentInChild<T>(GameObject panel, string ComponentName) where T : Component
    {
        Transform[] transforms = panel.GetComponentsInChildren<Transform>();

        foreach (Transform tra in transforms)
        {
            if (tra.gameObject.name == ComponentName)
            {
                return tra.gameObject.GetComponent<T>();
                break;
            }
        }

        return null;
    }

}
