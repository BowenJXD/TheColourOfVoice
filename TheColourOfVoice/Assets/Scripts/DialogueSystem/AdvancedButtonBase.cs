using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AdvancedButtonBase : Button
{
    public Action<int> OnConfirm;
    protected int _index;
    
    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(OnClickEvent);
    }

    public virtual void Init(string content, int index, Action<int> onConfirmEvent)
    {
        _index = index;
        OnConfirm += onConfirmEvent;
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        Select();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        DialogueManager.SetCurrentSelectable(this);
    }

    protected virtual void OnClickEvent() 
    {

    }

    public void Confirm() 
    {
        OnConfirm(_index);
    }
}
