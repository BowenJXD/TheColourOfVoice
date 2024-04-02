using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System;

/// <summary>
/// 注音系统的数据类
/// </summary>
public class RubyData 
{
    public RubyData(int startIndex, string content) 
    {
        StartIndex = startIndex; //注音对象char在整个text中的index
        RubyContent = content; //注音内容
        EndIndex = startIndex; //结束位置
    }
    public int StartIndex {  get;}
    public int EndIndex { get; set; }
    public string RubyContent { get; }
}



/// <summary>
/// AdvancedTextPreprocessor是继承自ITextPreprocessor
/// 自定义的预处理类
/// </summary>
public class AdvancedTextPreprocessor : ITextPreprocessor
{
    //用于记录停顿位置下标和对应的停顿时间
    //<2,2.3>表示在这个text下标为2的位置停顿2.3秒
    public Dictionary<int, float> IntervalDictionary;

    public List<RubyData> RubyList;
    public AdvancedTextPreprocessor() 
    {
        IntervalDictionary = new Dictionary<int, float>();
        RubyList = new List<RubyData>();
    }

    //尝试获得
    public bool TryGetRubyStartFrom(int index ,out RubyData data) 
    {
        data = new RubyData(0,"");
        foreach (var tempRubyData in RubyList)
        {
            if (tempRubyData.StartIndex == index)
            {
                data = tempRubyData;
                return true;
            }

        }

        return false;
    }

    //ITextPreprocessor接口实现的方法，传入需要输出的字符串进行预处理
    public string PreprocessText(string text)
    {
        IntervalDictionary.Clear();
        RubyList.Clear();

        string processingText = text;
        string pattern = "<.*?>";
        Match match = Regex.Match(processingText, pattern);
        
        //一直进行匹配，直到匹配不到<>
        while (match.Success)
        {
            //裁剪出尖括号中的内容 <饭饭> -> 饭饭
            //Substring-> 裁剪(起点，终点)
            string label = match.Value.Substring(1, match.Length - 2);

            #region 判断数字格式<1.2> -> 延迟格式
            //TryParse尝试将<>中string类型转换成float，如果可以
            //代表这个<>中是数字
            if (float.TryParse(label, out float result)) 
            {
                IntervalDictionary[match.Index - 1] = result;
            }

            #endregion

            #region 判断数字格式<1.2> -> 延迟格式
            //+匹配前面的子表达式一次或多次。要匹配 + 字符，请使用 \+。
            //匹配输入字符串的开始位置，除非在方括号表达式中使用,
            //当该符号在方括号表达式中使用时，表示不接受该方括号表达式中的字符集合。要匹配 ^ 字符本身，请使用 \^。
            else if (Regex.IsMatch(label,"^r=.+"))
            {
                RubyList.Add(new RubyData(match.Index,label.Substring(2))); //从.开始
            }else if(label == "/r") 
            {
                if (RubyList.Count > 0)
                { 
                    RubyList[RubyList.Count - 1].EndIndex = match.Index - 1;
                }
            }
            #endregion
            processingText = processingText.Remove(match.Index, match.Length);

            #region 判断图片格式<sprite> -> sprite格式
            //如果匹配到读取sprite的话
            if (Regex.IsMatch(label,"^sprite=.+"))
            {
               
                processingText = processingText.Insert(match.Index,"*");
            }

            #endregion

            match = Regex.Match(processingText, pattern);
        }

        processingText = text;
        //匹配前面的子表达式零次或一次，或指明一个非贪婪限定符。要匹配 ? 字符，请使用 \?。
        //正则表达式匹配所有的数字并替换，
        //(\.\d+)?判断有或者没有小数点都需要被判断
        //替换为空
        pattern = @"(<(\d+)(\.\d+)?>)|(</r>)|(<r=.*?>)";
        processingText = Regex.Replace(processingText, pattern, "");
        return processingText;
    }
}



/// <summary>
/// AdvancedText继承自TextMeshProUGUI
/// 是TextMeshProUGUI的增强版
/// </summary>
public class AdvancedText : TextMeshProUGUI
{
    #region 管理淡入淡出效果的Widget
    private Widget _widget;
    public Action OnFinished;
    #endregion

    #region 管理和自定义打字输出间隔
    private Coroutine _typingCoroutine;
    private int _typingIndex;
    private float _defaultInterval = 0.03f;
    #endregion

    //对话显示类型
    public enum DisplayType 
    {
        Default,
        Fading,
        Typing
    }

    protected override void Awake()
    {
        base.Awake();
        _widget = GetComponent<Widget>();
    }

    //textPreprocessor 是一个text预处理器，所有输入文字会先经过这个
    //预处理器之后再显示在屏幕上
    public AdvancedText() 
    {
        textPreprocessor = new AdvancedTextPreprocessor();
      
    }

    //将接口类型转换成AdvancedTextPreprocessor类型
    private AdvancedTextPreprocessor _SelfPreprocessor => (AdvancedTextPreprocessor)textPreprocessor;
    
    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialize() 
    {
        SetText("");
        ClearRubyText();
    }

    public void Disappear(float duration = 0.2f)
    {
        _widget.Fade(0, duration, null);
    }

    /// <summary>
    /// 设置文本内容并且调用逐字显示的协程
    /// </summary>
    /// <param name="text">需要输出的内容</param>
    public void ShowTextByTyping(string text) 
    {
        SetText(text);
        StartCoroutine(Typing());
    }

    /// <summary>
    /// 实例化注音prefab并显示在对应的文本正上方
    /// </summary>
    /// <param name="data">注音数据，保存在自定义的AdvancedTextPreprocessor中的RubyList中</param>
    private void SetRubyText(RubyData data) 
    {
        //实例化注音prefab并将自己设置为父物体
        GameObject ruby =Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/TextPrefab/RubyText"), this.transform);
        if (ruby)
        {
            ruby.GetComponent<TextMeshProUGUI>().SetText(data.RubyContent);
            ruby.GetComponent<TextMeshProUGUI>().color = textInfo.characterInfo[data.StartIndex].color;

            //让注音显示在对应文本的中间正上方（前提是轴心点的位置为middleBottom）
            ruby.transform.localPosition = (textInfo.characterInfo[data.StartIndex].topLeft + 
                                            textInfo.characterInfo[data.EndIndex].topRight) / 2;
        }
    }


    /// <summary>
    /// 直接将RubyList中的所有注音prefab全部实例化并标注
    /// </summary>
    private void SetAllRubyText() 
    {
        foreach (var item in _SelfPreprocessor.RubyList)
        {
            SetRubyText(item);
        }
    }


    /// <summary>
    /// 一句话结束之后需要销毁所有的注音预制体
    /// </summary>
    private void ClearRubyText() 
    {
        foreach (var item in GetComponentsInChildren<TextMeshProUGUI>())
        {
            if (item != this)
            {
                Destroy(item.gameObject);
            }
        }
    }

    public void QuickShowRemainingText() 
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine( _typingCoroutine );
            for ( ; _typingIndex < m_characterCount; _typingIndex++)
            {
                StartCoroutine(FadeInCharacter(_typingIndex));
            }
            OnFinished.Invoke();
        }
    }

    public IEnumerator AdvanceShowText(string content, DisplayType type, float fadingDuration = 0.2f) 
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
        }
        ClearRubyText();
        //this.gameObject.SetActive(false);
        this.SetText(content);
        Color tempcolor = this.color;
        this.color = new Color(color.r, color.g, color.b, 0);
        yield return null;

        switch (type) 
        {
            case DisplayType.Default:
                this.color = tempcolor;
                _widget.RenderOpacity = 1;
                SetAllRubyText();
                OnFinished?.Invoke();
                break;
            case DisplayType.Fading:
                this.color = tempcolor;
                _widget.Fade(1, fadingDuration, OnFinished.Invoke);
                SetAllRubyText();
                break;
            case DisplayType.Typing:
                this.color = tempcolor;
                _widget.Fade(1, fadingDuration, null);
                _typingCoroutine = StartCoroutine(Typing());
                ClearRubyText();
                break;
                default: break;
        }   
    }

    /// <summary>
    /// 逐字打印字符，并且响应自定义富文本效果
    /// </summary>
    /// <returns></returns>
    IEnumerator Typing() 
    {
        ForceMeshUpdate();
        //遍历整个字符串并把alpha调整为0
        for (int i = 0; i < m_characterCount; i++) 
        {
            SetSingleCharacterAlpha(i, 0);
        }

        _typingIndex = 0;
        while (_typingIndex < m_characterCount) 
        {
                     
            StartCoroutine(FadeInCharacter(_typingIndex));
            
            //SetSingleCharacterAlpha(_typingIndex, 255);
           
            if (_SelfPreprocessor.IntervalDictionary.TryGetValue(_typingIndex, out float result)) 
            {
                yield return new WaitForSecondsRealtime(result);
            } else yield return new WaitForSecondsRealtime(_defaultInterval);
            _typingIndex++;
        }

        OnFinished.Invoke();
    }

    /// <summary>
    /// 控制字符fadein
    /// </summary>
    /// <param name="index">现在字符的下标</param>
    /// <param name="duration">字符fadein的时间</param>
    /// <returns></returns>
    IEnumerator FadeInCharacter(int index, float duration = 0.2f) 
    {
        if (_SelfPreprocessor.TryGetRubyStartFrom(index, out RubyData rubyData))
        {
            SetRubyText(rubyData);
        }
       
        if (duration <= 0)
        {
            SetSingleCharacterAlpha(index, 255);
        }
        else
        {
            //timer计时，用timer/duration来表示0-1的变化
            float timer = 0;
            while (timer < duration) 
            {
                timer = Mathf.Min(duration, timer + Time.unscaledDeltaTime);
                SetSingleCharacterAlpha(index, (byte)(255 * timer / duration));
                yield return null;
            }
        }
    }


    /// <summary>
    /// 设置单个字符的alpha值，以此实现逐字打印的效果
    /// </summary>
    /// <param name="index">字符位置</param>
    /// <param name="newAlpha">新的alpha值</param>
    private void SetSingleCharacterAlpha(int index, byte newAlpha) 
    {
        //拿到对应下标的字符
        TMP_CharacterInfo charInfo = this.textInfo.characterInfo[index];
        //如果这个下标的char是可见的就执行渐变效果
        //避免 空格 出现bug
        if (!charInfo.isVisible)
        {
            return;
        }
        int matIndex = charInfo.materialReferenceIndex; //字符的材质信息
        int vertIndex = charInfo.vertexIndex;//字符的顶点信息
        for (int i = 0; i < 4; i++) 
        {
            //textInfo为这个text的信息
            textInfo.meshInfo[matIndex].colors32[vertIndex + i].a = newAlpha;

        }

        UpdateVertexData();
    }
}
