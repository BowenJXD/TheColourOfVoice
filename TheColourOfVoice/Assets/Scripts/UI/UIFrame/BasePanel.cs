/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel
{
    public UIType uiType;
    public GameObject ActiveObj;

    public BasePanel(UIType uitype)
    {
        uiType = uitype;
    }

    public virtual void OnStart() 
    {
        Debug.Log($"{uiType.name} Start to ues");
        UIMethods.GetInstance().GetOrAddCompnent<CanvasGroup>(ActiveObj).interactable = true;

    }
    public virtual void OnEnable() {
        UIMethods.GetInstance().GetOrAddCompnent<CanvasGroup>(ActiveObj).interactable = true;

    }
    public virtual void OnDisable() 
    {
        UIMethods.GetInstance().GetOrAddCompnent<CanvasGroup>(ActiveObj).interactable = false;
    }
    public virtual void OnDestroy() {
        UIMethods.GetInstance().GetOrAddCompnent<CanvasGroup>(ActiveObj).interactable = false;

    }


}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasePanel
{
    public UIType uiType;


    public GameObject ActiveObj;

    public BasePanel(UIType uitype)
    {
        uiType = uitype;
    }

    public virtual void OnStart()
    {
        Debug.Log("Obj is loaded!");
        if (ActiveObj.GetComponent<CanvasGroup>() == null)
        {
            ActiveObj.AddComponent<CanvasGroup>();
        }
    }

    public virtual void OnEnable()
    {
        UIMethods.GetInstance().AddOrGetComponent<CanvasGroup>(ActiveObj).interactable = true;
    }

    public virtual void OnDisable()
    {
        UIMethods.GetInstance().AddOrGetComponent<CanvasGroup>(ActiveObj).interactable = false;
    }

    public virtual void OnDestory()
    {
        UIMethods.GetInstance().AddOrGetComponent<CanvasGroup>(ActiveObj).interactable = false;
    }

}
