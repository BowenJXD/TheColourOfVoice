using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;


public class DialogueManager: Singleton<DialogueManager>
{
    [Header("对话系统")]
    [SerializeField]
    private AdvancedDialogueBox advancedDialogueBox;
    private Selectable _currentSelectable;
    private GameObject _pfbButtonA;
    [SerializeField] private ChoicePanel _choicePanel;

    public static void SetCurrentSelectable(Selectable currentSelectable) 
    {
        DialogueManager.Instance._currentSelectable = currentSelectable;
    }
    protected override void Awake()
    {
        base.Awake();
        DialogueManager.Instance._pfbButtonA = Resources.Load<GameObject>("Prefab/TextPrefab/BiggerButton");       
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (null != DialogueManager.Instance._currentSelectable)
            {
                DialogueManager.Instance._currentSelectable.Select();
            }
        }
    }
    public static void OpenDialogueBox(Action<bool> onNextEvent, int boolStyle = 0) 
    {
        DialogueManager.Instance.advancedDialogueBox.Open(onNextEvent, boolStyle);
    }

    public static void CloseDialogueBox() 
    {
        DialogueManager.Instance.advancedDialogueBox.Close(null);
    }

    public static void PrintDialogue(DialogueData data) 
    {
        DialogueManager.Instance.StartCoroutine(DialogueManager.Instance.advancedDialogueBox.PrintDialogue(
            data.content,
            data.speaker,
            data.NeedTyping,
            data.AutoNext,
            data.CanQuickShow

            ));
    }

    public static void CreateDialogueChocies(ChoiceData[] datas, Action<int> onComfirmEvent, int deafultSelectChoiceIndex)
    {
        for(int i = 0; i < datas.Length; i++) 
        {
            AdvancedButtonA button = Instantiate(DialogueManager.Instance._pfbButtonA).GetComponent<AdvancedButtonA>();
            button.gameObject.name = "ButtonA" + i;
            button.Init(datas[i].content, i, onComfirmEvent);
            DialogueManager.Instance._choicePanel.AddButton(button);
        }

        DialogueManager.Instance._choicePanel.Open(deafultSelectChoiceIndex);
    }

}
