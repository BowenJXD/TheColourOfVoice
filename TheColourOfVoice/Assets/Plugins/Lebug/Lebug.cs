// Lebug.Log v1.1.0
// An alternative to Debug.Log by Levent Alpsal
using System.Collections.Generic;

public static class Lebug
{
    public static Dictionary<string, Dictionary<string, object>> lebugDict = new Dictionary<string, Dictionary<string, object>>();
    public static Dictionary<string, bool> categoriesExpanded = new Dictionary<string, bool>(); // Category collapsed or expanded
    /// <summary>
    /// Show the value of this category(Default) - key pair on Lebug Windows.
    /// Can use same key names in different categories.
    /// Health key in enemy and player category.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="category">Optional, Category name</param>
    /// <param name="expanded">Optional, Should the category start expanded or collapsed</param>
    public static void Log(string key, object value, string category = "Default", bool expanded = true)
    {
        if (null == value)
        {
            value = "NULL";
        }

        if(null == key)
        {
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
            Lebug.Log("Null Key sent for (" + value + ") from class-method:", stackTrace.GetFrame(1).GetMethod().DeclaringType.Name + " - " + stackTrace.GetFrame(1).GetMethod().Name, "LEBUG ERROR");
            return;
        }


        if (!lebugDict.ContainsKey(category))
        {
            categoriesExpanded.Add(category, expanded);
            lebugDict.Add(category, new Dictionary<string, object>());
        }

        if (lebugDict[category].ContainsKey(key))
        {
            lebugDict[category][key] = value.ToString();
        }
        else
        {
            lebugDict[category].Add(key, value.ToString());
        }
    }

    /// <summary>
    /// Deletes a value. If category has no more values, it can be deleted too.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="category"></param>
    /// <param name="keepEmptyCategory">When false, will delete the category if the value was the last one.</param>
    public static void Del(string key, string category = "Default", bool keepEmptyCategory = false)
    {
        if (!lebugDict.ContainsKey(category)) return;
        if (!lebugDict[category].ContainsKey(key)) return;

        lebugDict[category].Remove(key);

        if (lebugDict[category].Count == 0 && !keepEmptyCategory)
        {
            lebugDict.Remove(category);
            categoriesExpanded.Remove(category);
        }
    }

    /// <summary>
    /// Deletes category and its contents. Can be choosen to only empty the category.
    /// </summary>
    /// <param name="category"></param>
    /// <param name="onlyContents">When true, will only delete the contents of the category but will keep the category.</param>
    public static void DelCategory(string category = "Default", bool onlyContents = false)
    {
        if (!lebugDict.ContainsKey(category)) return;

        if (onlyContents)
        {
            lebugDict[category].Clear();
        }
        else
        {
            lebugDict.Remove(category);
            categoriesExpanded.Remove(category);
        }
    }

    /// <summary>
    /// Expands the category. Categories are expanded by default.
    /// </summary>
    /// <param name="category"></param>
    public static void ExpandCategory(string category = "Default")
    {
        if (!lebugDict.ContainsKey(category)) return;
        categoriesExpanded[category] = true;
    }

    /// <summary>
    /// Collapses the category, data not lost.
    /// </summary>
    /// <param name="category"></param>
    public static void CollapseCategory(string category = "Default")
    {
        if (!lebugDict.ContainsKey(category)) return;
        categoriesExpanded[category] = false;
    }

}