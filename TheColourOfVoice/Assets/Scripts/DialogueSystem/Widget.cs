using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ƹ�������ű���UI��������Զ���ĵ��뵭��
/// �����ҪCanvasGroup
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
    /// ���������canvasgroup������Ч��
    /// </summary>
    /// <param name="opacity">Ŀ��͸����</param>
    /// <param name="duration">����Ŀ��͸���ȵ�ʱ��</param>
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
    /// ִ��Fade��Э��
    /// </summary>
    /// <param name="opacity">Ŀ��͸����</param>
    /// <param name="duration">����Ŀ��͸���ȵ�ʱ��</param>
    /// <param name="onFinished">�ص��¼�</param>
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
