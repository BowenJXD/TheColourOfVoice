using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIType
{
    private string path;
    private string name;

    public string Path
    {
        get => path;
    }

    public string Name
    {
        get => name;
    }

    /// <summary>
    /// get UI info
    /// </summary>
    /// <param name="ui_path">Panel path</param>
    /// <param name="ui_name">Panel name</param>
    public UIType(string ui_path,string ui_name)
    {
        name = ui_name;
        path = ui_path;
    }
}
