using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using System;

/// <summary>
/// ע��ϵͳ��������
/// </summary>
public class RubyData 
{
    public RubyData(int startIndex, string content) 
    {
        StartIndex = startIndex; //ע������char������text�е�index
        RubyContent = content; //ע������
        EndIndex = startIndex; //����λ��
    }
    public int StartIndex {  get;}
    public int EndIndex { get; set; }
    public string RubyContent { get; }
}



/// <summary>
/// AdvancedTextPreprocessor�Ǽ̳���ITextPreprocessor
/// �Զ����Ԥ������
/// </summary>
public class AdvancedTextPreprocessor : ITextPreprocessor
{
    //���ڼ�¼ͣ��λ���±�Ͷ�Ӧ��ͣ��ʱ��
    //<2,2.3>��ʾ�����text�±�Ϊ2��λ��ͣ��2.3��
    public Dictionary<int, float> IntervalDictionary;

    public List<RubyData> RubyList;
    public AdvancedTextPreprocessor() 
    {
        IntervalDictionary = new Dictionary<int, float>();
        RubyList = new List<RubyData>();
    }

    //���Ի��
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

    //ITextPreprocessor�ӿ�ʵ�ֵķ�����������Ҫ������ַ�������Ԥ����
    public string PreprocessText(string text)
    {
        IntervalDictionary.Clear();
        RubyList.Clear();

        string processingText = text;
        string pattern = "<.*?>";
        Match match = Regex.Match(processingText, pattern);
        
        //һֱ����ƥ�䣬ֱ��ƥ�䲻��<>
        while (match.Success)
        {
            //�ü����������е����� <����> -> ����
            //Substring-> �ü�(��㣬�յ�)
            string label = match.Value.Substring(1, match.Length - 2);

            #region �ж����ָ�ʽ<1.2> -> �ӳٸ�ʽ
            //TryParse���Խ�<>��string����ת����float���������
            //�������<>��������
            if (float.TryParse(label, out float result)) 
            {
                IntervalDictionary[match.Index - 1] = result;
            }

            #endregion

            #region �ж����ָ�ʽ<1.2> -> �ӳٸ�ʽ
            //+ƥ��ǰ����ӱ��ʽһ�λ��Ρ�Ҫƥ�� + �ַ�����ʹ�� \+��
            //ƥ�������ַ����Ŀ�ʼλ�ã������ڷ����ű��ʽ��ʹ��,
            //���÷����ڷ����ű��ʽ��ʹ��ʱ����ʾ�����ܸ÷����ű��ʽ�е��ַ����ϡ�Ҫƥ�� ^ �ַ�������ʹ�� \^��
            else if (Regex.IsMatch(label,"^r=.+"))
            {
                RubyList.Add(new RubyData(match.Index,label.Substring(2))); //��.��ʼ
            }else if(label == "/r") 
            {
                if (RubyList.Count > 0)
                { 
                    RubyList[RubyList.Count - 1].EndIndex = match.Index - 1;
                }
            }
            #endregion
            processingText = processingText.Remove(match.Index, match.Length);

            #region �ж�ͼƬ��ʽ<sprite> -> sprite��ʽ
            //���ƥ�䵽��ȡsprite�Ļ�
            if (Regex.IsMatch(label,"^sprite=.+"))
            {
               
                processingText = processingText.Insert(match.Index,"*");
            }

            #endregion

            match = Regex.Match(processingText, pattern);
        }

        processingText = text;
        //ƥ��ǰ����ӱ��ʽ��λ�һ�Σ���ָ��һ����̰���޶�����Ҫƥ�� ? �ַ�����ʹ�� \?��
        //������ʽƥ�����е����ֲ��滻��
        //(\.\d+)?�ж��л���û��С���㶼��Ҫ���ж�
        //�滻Ϊ��
        pattern = @"(<(\d+)(\.\d+)?>)|(</r>)|(<r=.*?>)";
        processingText = Regex.Replace(processingText, pattern, "");
        return processingText;
    }
}



/// <summary>
/// AdvancedText�̳���TextMeshProUGUI
/// ��TextMeshProUGUI����ǿ��
/// </summary>
public class AdvancedText : TextMeshProUGUI
{
    #region �����뵭��Ч����Widget
    private Widget _widget;
    public Action OnFinished;
    #endregion

    #region ������Զ������������
    private Coroutine _typingCoroutine;
    private int _typingIndex;
    private float _defaultInterval = 0.03f;
    #endregion

    //�Ի���ʾ����
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

    //textPreprocessor ��һ��textԤ�������������������ֻ��Ⱦ������
    //Ԥ������֮������ʾ����Ļ��
    public AdvancedText() 
    {
        textPreprocessor = new AdvancedTextPreprocessor();
      
    }

    //���ӿ�����ת����AdvancedTextPreprocessor����
    private AdvancedTextPreprocessor _SelfPreprocessor => (AdvancedTextPreprocessor)textPreprocessor;
    
    /// <summary>
    /// ��ʼ��
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
    /// �����ı����ݲ��ҵ���������ʾ��Э��
    /// </summary>
    /// <param name="text">��Ҫ���������</param>
    public void ShowTextByTyping(string text) 
    {
        SetText(text);
        StartCoroutine(Typing());
    }

    /// <summary>
    /// ʵ����ע��prefab����ʾ�ڶ�Ӧ���ı����Ϸ�
    /// </summary>
    /// <param name="data">ע�����ݣ��������Զ����AdvancedTextPreprocessor�е�RubyList��</param>
    private void SetRubyText(RubyData data) 
    {
        //ʵ����ע��prefab�����Լ�����Ϊ������
        GameObject ruby =Instantiate<GameObject>(Resources.Load<GameObject>("Prefab/TextPrefab/RubyText"), this.transform);
        if (ruby)
        {
            ruby.GetComponent<TextMeshProUGUI>().SetText(data.RubyContent);
            ruby.GetComponent<TextMeshProUGUI>().color = textInfo.characterInfo[data.StartIndex].color;

            //��ע����ʾ�ڶ�Ӧ�ı����м����Ϸ���ǰ�������ĵ��λ��ΪmiddleBottom��
            ruby.transform.localPosition = (textInfo.characterInfo[data.StartIndex].topLeft + 
                                            textInfo.characterInfo[data.EndIndex].topRight) / 2;
        }
    }


    /// <summary>
    /// ֱ�ӽ�RubyList�е�����ע��prefabȫ��ʵ��������ע
    /// </summary>
    private void SetAllRubyText() 
    {
        foreach (var item in _SelfPreprocessor.RubyList)
        {
            SetRubyText(item);
        }
    }


    /// <summary>
    /// һ�仰����֮����Ҫ�������е�ע��Ԥ����
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
    /// ���ִ�ӡ�ַ���������Ӧ�Զ��帻�ı�Ч��
    /// </summary>
    /// <returns></returns>
    IEnumerator Typing() 
    {
        ForceMeshUpdate();
        //���������ַ�������alpha����Ϊ0
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
    /// �����ַ�fadein
    /// </summary>
    /// <param name="index">�����ַ����±�</param>
    /// <param name="duration">�ַ�fadein��ʱ��</param>
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
            //timer��ʱ����timer/duration����ʾ0-1�ı仯
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
    /// ���õ����ַ���alphaֵ���Դ�ʵ�����ִ�ӡ��Ч��
    /// </summary>
    /// <param name="index">�ַ�λ��</param>
    /// <param name="newAlpha">�µ�alphaֵ</param>
    private void SetSingleCharacterAlpha(int index, byte newAlpha) 
    {
        //�õ���Ӧ�±���ַ�
        TMP_CharacterInfo charInfo = this.textInfo.characterInfo[index];
        //�������±��char�ǿɼ��ľ�ִ�н���Ч��
        //���� �ո� ����bug
        if (!charInfo.isVisible)
        {
            return;
        }
        int matIndex = charInfo.materialReferenceIndex; //�ַ��Ĳ�����Ϣ
        int vertIndex = charInfo.vertexIndex;//�ַ��Ķ�����Ϣ
        for (int i = 0; i < 4; i++) 
        {
            //textInfoΪ���text����Ϣ
            textInfo.meshInfo[matIndex].colors32[vertIndex + i].a = newAlpha;

        }

        UpdateVertexData();
    }
}
