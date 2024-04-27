using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LebugWindow : EditorWindow
{
    // Styles
    GUIStyle defaultStyle;
    GUIStyle settingsButtonStyle;
    GUIStyle categoryStyle;
    GUIStyle keyStyle;
    GUIStyle valueStyle;

    // Colors
    Color defaultColor = Color.black;
    Color categoryColor = new Color(0.2f, 0.8f, 0.4f, 1); // Greenish
    //Color categoryColor = new Color(0.6f, 0.4f, 0.0f, 1); // Brownish :)
    Color buttonColor = new Color(0.0f, 0.2f, 0.8f, 1); // Blueish
    Color[] contentColor;

    int[] fontSizes;
    int currentFontSizeIndex;

    // Scroll positions
    Vector2 scrollViewPosition = Vector2.zero;

    private void OnEnable()
    {
        contentColor = new Color[] { Color.black, new Color(0.2f, 0.2f, 0.2f, 1) };

        // Min window size
        minSize = new Vector2(200, 100);

        defaultStyle = new GUIStyle();
        defaultStyle.wordWrap = true;
        defaultStyle.normal.background = Texture2D.whiteTexture;
        defaultStyle.normal.textColor = Color.white;
        defaultStyle.fontStyle = FontStyle.Bold;

        fontSizes = new int[] { defaultStyle.fontSize, 15, 30, 45, 60 };

        settingsButtonStyle = new GUIStyle(defaultStyle);
        settingsButtonStyle.wordWrap = true;
        settingsButtonStyle.normal.background = Texture2D.whiteTexture;
        settingsButtonStyle.normal.textColor = new Color(0.8f, 0.8f, 0.8f, 1);
        settingsButtonStyle.hover.background = Texture2D.whiteTexture;
        settingsButtonStyle.hover.textColor = Color.white;
        settingsButtonStyle.fontStyle = FontStyle.Bold;

        settingsButtonStyle.alignment = TextAnchor.MiddleLeft;
        settingsButtonStyle.padding = new RectOffset(10, 10, 2, 2);
        settingsButtonStyle.stretchWidth = false;

        categoryStyle = new GUIStyle(defaultStyle);
        categoryStyle.alignment = TextAnchor.MiddleLeft;
        categoryStyle.padding = new RectOffset(1, 1, 1, 1);

        keyStyle = new GUIStyle(defaultStyle);

        valueStyle = new GUIStyle(defaultStyle);
        valueStyle.alignment = TextAnchor.MiddleRight;
    }

    void OnInspectorUpdate()
    {
        Repaint();
    }

    void OnGUI()
    {
        DrawHeader();
        DrawCategories();
    }

    void DrawHeader()
    {
        GUI.backgroundColor = Color.black;
        EditorGUILayout.BeginHorizontal(defaultStyle, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true));

        GUI.backgroundColor = buttonColor;
        if (GUILayout.Button("Font size", settingsButtonStyle))
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Default"), (currentFontSizeIndex == 0), () => { currentFontSizeIndex = 0; });
            for (int i = 1; i < 5; i++)
            {
                int localI = i;
                menu.AddItem(new GUIContent($"{i + 1}x"), (currentFontSizeIndex == i), () => { currentFontSizeIndex = localI;});
            }

            menu.ShowAsContext();
        }
        GUILayout.Space(3);

        keyStyle.fontSize = fontSizes[currentFontSizeIndex];
        valueStyle.fontSize = fontSizes[currentFontSizeIndex];
        categoryStyle.fontSize = fontSizes[currentFontSizeIndex];

        EditorGUILayout.EndHorizontal();
    }

    void DrawCategories()
    {
        GUI.backgroundColor = defaultColor;
        scrollViewPosition = EditorGUILayout.BeginScrollView(scrollViewPosition, EditorStyles.textField);

        EditorGUILayout.BeginVertical(EditorStyles.textArea, GUILayout.ExpandHeight(true));

        foreach (KeyValuePair<string, Dictionary<string, object>> categoryDict in Lebug.lebugDict)
        {
            GUI.backgroundColor = categoryColor;
            if (GUILayout.Button(
                (Lebug.categoriesExpanded[categoryDict.Key] ? ((char)8212).ToString() : "+") + " " + categoryDict.Key, categoryStyle))
            {
                Lebug.categoriesExpanded[categoryDict.Key] = !Lebug.categoriesExpanded[categoryDict.Key];
            }

            if (!Lebug.categoriesExpanded[categoryDict.Key])
            {
                // This category is collapsed. Don't show the contents...
                continue;
            }

            DrawCategoryContents(categoryDict.Value);
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();
    }

    void DrawCategoryContents(Dictionary<string, object> categoryContents)
    {
        EditorGUILayout.BeginVertical();
        int i = 0;
        foreach (KeyValuePair<string, object> row in categoryContents)
        {
            // Write content line
            GUI.backgroundColor = contentColor[i++ % 2];
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            GUILayout.Label(row.Key, keyStyle);
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            GUILayout.Label(row.Value.ToString(), valueStyle);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(3);
        }
        GUILayout.Space(8);
        EditorGUILayout.EndVertical();
    }

    [MenuItem("Window/Lebug Window")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(LebugWindow));
    }
}