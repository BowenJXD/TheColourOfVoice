using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �ű������ڶԻ�����
/// ����ű��������ǽ�������Ȼ����ʾ�ڶԻ�����
/// </summary>
public class AdvancedDialogueBox : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Image _background;
    [SerializeField] private Widget _widget;
    [SerializeField] private TextMeshProUGUI _speaker;
    [SerializeField] private AdvancedText _content;
    [SerializeField] private Widget _nextCursorWidget;
    [SerializeField] private Animator _nextCursorAnimator;
    private static readonly int _click = Animator.StringToHash("Click");

    [Header("Config")]
    [SerializeField] private Sprite[] _backgroundStyles;

    private bool _interactable;
    private bool _printFinished;
    private bool _canQuickShow;
    private bool _autoNext;

    private bool CanQuickShow => _canQuickShow && !_printFinished;
    private bool CanNext => _printFinished;
    public Action<bool> OnNext; //bool ������ʾ��һ�仰�Ƿ�ǿ��ֱ����ʾ

    private void Awake()
    {
        _content.OnFinished = PrintFinished;
    }

    /// <summary>
    /// �󶨸�Onfinished event
    /// </summary>
    private void PrintFinished() 
    {
        if (_autoNext)
        {
            _interactable = false;
            OnNext(false);
        }
        else
        {
            _interactable = true;
            _printFinished = true;
            _nextCursorWidget.Fade(1, 0.2f, null);
        }
    }
  
    void Update()
    {
        if (_interactable)
        {
            UpdateInput();
        }
    }

    /// <summary>
    /// �����������
    /// </summary>
    private void UpdateInput() 
    {
        if (Input.GetButtonDown("Cancel"))
        {
            //1.�����仰������
            if (CanQuickShow)
            {
                _content.QuickShowRemainingText();

            }else if (CanNext) 
            {
                _interactable = false;
                _nextCursorAnimator.SetTrigger(_click);
                _nextCursorWidget.Fade(0, 0.5f, null);
                OnNext(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            if (CanQuickShow)
            {
                _content.QuickShowRemainingText();  
            }
            else if (CanNext)
            {
                _interactable = false;
                _nextCursorAnimator.SetTrigger(_click);
                _nextCursorWidget.Fade(0, 0.5f, null);
                OnNext(false);
            }
        }
       
    }

    /// <summary>
    /// �򿪶Ի���
    /// </summary>
    /// <param name="nextEvent">�ص��¼����󶨶Ի�����ʾ֮���ʲô</param>
    /// <param name="boxStyle">�Ի��򱳾���spriteʹ����һ�������Դ�����Ȼ��ѡ��</param>
    public void Open(Action<bool> nextEvent, int boxStyle = 0) 
    {
        OnNext = nextEvent; //�ص�
        _background.sprite = _backgroundStyles[boxStyle];
        

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            _widget.Fade(1, 0.2f, null);
            _speaker.SetText("");
            _content.Initialize();
        }

        _nextCursorWidget.RenderOpacity = 0;

        OnNext(false);
    }

    /// <summary>
    /// �رնԻ���
    /// </summary>
    /// <param name="onClosed">�ص�����</param>
    public void Close(Action onClosed) 
    {
        gameObject.SetActive(false);
        _widget.Fade(0, 0.2f, () => 
        {
            onClosed?.Invoke();
        });
    }

    /// <summary>
    /// ��ʼ����Ի�����
    /// </summary>
    /// <param name="content">�Ի�������</param>
    /// <param name="speaker">�Ի��˵�����</param>
    /// <param name="needTyping">�Ƿ���Ҫ���ִ�������Ϊ��Ļ��ͽ������</param>
    /// <param name="autoNext">�Ƿ���Ҫ�Զ���һ�仰</param>
    /// <param name="canQuickShow">�Ƿ�����ڶԻ����ִ�ӡ��ʱ��ʹ��esc������ʾʣ�ಿ��</param>
    /// <returns></returns>
    public IEnumerator PrintDialogue(string content, string speaker, bool needTyping = true,
        bool autoNext = false, bool canQuickShow = true) 
    {
        _interactable = false;
        _printFinished = false;

        if (_content.text != "")
        {
            _content.Disappear();
            yield return new WaitForSecondsRealtime(0.3f);

        }

        _canQuickShow = canQuickShow;
        _autoNext = autoNext;
        _speaker.SetText(speaker);

        if (needTyping)
        {
            _interactable = true;
            _content.StartCoroutine(_content.AdvanceShowText(content,AdvancedText.DisplayType.Typing));

        }
        else
        {
            _content.StartCoroutine(_content.AdvanceShowText(content, AdvancedText.DisplayType.Fading));
        }
    }
}
