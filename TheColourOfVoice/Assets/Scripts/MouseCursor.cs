using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public Texture2D customCursorTexture; // 自定义鼠标指针的纹理
    public Vector2 hotspot = Vector2.zero; // 鼠标指针的热点

    private void Awake()
    {
        // 确保此对象在场景切换时不被销毁
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetCustomCursor();
    }

    private void SetCustomCursor()
    {
        Cursor.SetCursor(customCursorTexture, hotspot, CursorMode.Auto);
    }
}
