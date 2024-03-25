using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制挂载这个脚本的UI组件进行自定义的淡入淡出
/// 组件需要CanvasGroup
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class Widget : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
   [SerializeField] private AnimationCurve _fadingCurve = AnimationCurve.EaseInOut(0,0,1,1);

    public float RenderOpacity 
    {
        get { return _canvasGroup.alpha; }
        set { _canvasGroup.alpha = value;}
    }
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private Coroutine _fadeCoroutine;
    
    /// <summary>
    /// 完成这个组件canvasgroup的消融效果
    /// </summary>
    /// <param name="opacity">目标透明度</param>
    /// <param name="duration">到达目标透明度的时间</param>
    public void Fade(float opacity, float duration, Action onFinished) 
    {
        if (duration <= 0)
        {
            _canvasGroup.alpha = opacity;
            onFinished?.Invoke();
        }
        else
        {
            //print("StartFade");
            if (_fadeCoroutine != null)
            {
                StopCoroutine( _fadeCoroutine );
            }
        }

        _fadeCoroutine = StartCoroutine(Fading(opacity, duration, onFinished));
    }


    /// <summary>
    /// 执行Fade的协程
    /// </summary>
    /// <param name="opacity">目标透明度</param>
    /// <param name="duration">到达目标透明度的时间</param>
    /// <param name="onFinished">回调事件</param>
    /// <returns></returns>
    private IEnumerator Fading(float opacity, float duration, Action onFinished) 
    {
        //print("StartFading");
        float timer = 0;
        float start = RenderOpacity;
        while (timer < duration) 
        {
            timer = Math.Min(duration, timer+Time.unscaledDeltaTime);
            RenderOpacity = Mathf.Lerp(start, opacity, _fadingCurve.Evaluate(timer/duration));
            yield return null;
        }

        onFinished?.Invoke();
        
    }
}
