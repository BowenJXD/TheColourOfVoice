using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
public class FadeOutEffect : MonoBehaviour
{
    public CanvasGroup introCanvasGroup;  // 
    public CanvasGroup intro2CanvasGroup;  // 

    public CanvasGroup maskCanvasGroup; // 
    public float fadeDuration = 1.5f;     // 
    public float displayTime = 2f;        
    private bool inputDisabled = false;   // 用于追踪输入是否被禁用

    void Start()
    {
        // 开始执行淡入淡出的流程
        StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        DisableInput();
        yield return StartCoroutine(FadeIn(introCanvasGroup));

        yield return new WaitForSeconds(displayTime);

        yield return StartCoroutine(FadeOut(introCanvasGroup));

        yield return StartCoroutine(FadeIn(intro2CanvasGroup));

        yield return new WaitForSeconds(displayTime);

        yield return StartCoroutine(FadeOut(intro2CanvasGroup));
        
        yield return StartCoroutine(FadeOut(maskCanvasGroup));
        EnableInput();
    }

    // 淡入效果的协程
    IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = 0f;  
        canvasGroup.gameObject.SetActive(true);
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration); 
            yield return null;
        }
    }

    // 淡出效果的协程
    IEnumerator FadeOut(CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = 1f;  

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(1 - (elapsedTime / fadeDuration));  
            yield return null;
        }
    }

    void DisableInput()
    {
        Main_Menu.isInputDisabled = true;  // 禁用 Main_Menu 的输入

        inputDisabled = true;
    }

    void EnableInput()
    {
        Main_Menu.isInputDisabled = false;  // 恢复 Main_Menu 的输入

        inputDisabled = false;
    }
    void Update()
    {
        if (inputDisabled)
        {
            // 阻止所有按键和鼠标输入
            if (Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
            {
                // 什么都不做，防止响应输入
            }
        }
    }
}