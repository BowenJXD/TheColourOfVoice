using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel:BasePanel
{
    private static string name = "StartPanel";

    private static string path = "Panels/StartPanel";
    public static readonly UIType uiType = new UIType(path, name);

    public StartPanel() :base(uiType)
    {

    }
    public override void OnStart()
    {
        base.OnStart();
    }

    private void Back()
    {
        GameRoot.GetInstance().UIManager_Root.Pop(false);
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    public override void OnDestory()
    {
        base.OnDestory();
    }

}
