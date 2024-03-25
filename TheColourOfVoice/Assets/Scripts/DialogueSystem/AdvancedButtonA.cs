using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class AdvancedButtonA : AdvancedButtonBase
{
    Widget _fontRing;
    Animator _animator;    
    protected override void Awake()
    {
        base.Awake();
       _fontRing = transform.Find("Font Ring Gold").GetComponent<Widget>();
       _animator = this.GetComponent<Animator>();

    }

    public override void Init(string content, int index, Action<int> onConfirmEvent)
    {
        base.Init(content, index, onConfirmEvent);
        AdvancedText text = GetComponentInChildren<AdvancedText>();
        text.StartCoroutine(text.AdvanceShowText(content, AdvancedText.DisplayType.Default));
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        _fontRing.Fade(1,0.1f,null);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        _fontRing.Fade(0, 0.25f, null);
    }

    private static readonly int Click = Animator.StringToHash("Click");
    protected override void OnClickEvent()
    {
        base.OnClickEvent();
        _animator.SetTrigger(Click);
    }
}
