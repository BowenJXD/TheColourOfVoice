using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 脚本挂载在对话框上
/// 这个脚本的作用是接收文字然后显示在对话框上
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
    public Action<bool> OnNext; //bool 参数表示下一句话是否强制直接显示

    private void Awake()
    {
        _content.OnFinished = PrintFinished;
    }

    /// <summary>
    /// 绑定给Onfinished event
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
    /// 接收玩家输入
    /// </summary>
    private void UpdateInput() 
    {
        if (Input.GetButtonDown("Cancel"))
        {
            //1.完成这句话的输入
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
    /// 打开对话框
    /// </summary>
    /// <param name="nextEvent">回调事件，绑定对话框显示之后干什么</param>
    /// <param name="boxStyle">对话框背景的sprite使用哪一个，可以存入多个然后选用</param>
    public void Open(Action<bool> nextEvent, int boxStyle = 0) 
    {
        OnNext = nextEvent; //回调
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
    /// 关闭对话框
    /// </summary>
    /// <param name="onClosed">回调函数</param>
    public void Close(Action onClosed) 
    {
        _widget.Fade(0, 0.2f, () => 
        {
            onClosed?.Invoke();
        });
    }

    /// <summary>
    /// 开始输出对话内容
    /// </summary>
    /// <param name="content">对话的内容</param>
    /// <param name="speaker">对话人的姓名</param>
    /// <param name="needTyping">是否需要逐字打出，如果为否的话就渐变出现</param>
    /// <param name="autoNext">是否需要自动下一句话</param>
    /// <param name="canQuickShow">是否可以在对话逐字打印的时候使用esc快速显示剩余部分</param>
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
