using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class Level_PsyRoom : Singleton<Level_PsyRoom>
{
    public GameObject player;
    public GameObject uiCamera;
    public GameObject mainPanel;//对话panel
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI dialogueName;
    public GameObject choosingLevelPanel;
    public GameObject dialogueNextCursor;
    
    //睡觉相关的参数
    private int shakeCount = 0;
    public  bool playerIsAwake = false; //所有过长动画禁止玩家点击选关按钮的时候都把这个设置为false，懒得改了
    [SerializeField] private Light2D sightLight;
    [SerializeField] private Light2D sunShaftlight;
    
    //TimeLine有关的参数
    public enum GameMode
    {
        GamePlay,
       DialogueMoment
    }
    public GameMode gameMode;
    public PlayableDirector currentPlayableDirector;
    
    //Glitch打字效果的一些参数
    private Coroutine typingCoroutine;
    public float typingSpeed = 0.07f;
    public int glitchFrames = 3;//显示乱码的帧数
    private string randomChars = "ＡＢＣＤＥＦＧＨＩＪＫＬＭＮＯＰＱＲＳＴＵＶＷＸＹＺabcdefghijklmnopqrstuvwxyz0123456789！＠＃＄％＾＆＊（）＿＋－＝＜＞？";
    
    //关于对话框移动部分的参数
    private Vector2 dialoguePanelInitialPosition;
    private Vector2 dialoguePanelInitialSize;

    protected override void Awake()
    {
        base.Awake();
        gameMode = GameMode.GamePlay;
    }

    private void Start()
    {
        dialogueText.gameObject.SetActive(false);
        dialogueName.gameObject.SetActive(false);
        dialogueNextCursor.SetActive(false);
        dialoguePanelInitialPosition = mainPanel.GetComponent<RectTransform>().anchoredPosition;
        dialoguePanelInitialSize = mainPanel.GetComponent<RectTransform>().sizeDelta;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && playerIsAwake == false)
        {
            Shake(player, 0.1f, 0.1f);
            shakeCount++;

            if (shakeCount == 5)
            {
                LittleWitchAwake(); //小魔女苏醒
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && choosingLevelPanel.activeSelf)
        {
            choosingLevelPanel.SetActive(false);
        }

        if (gameMode == GameMode.DialogueMoment)
        {
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                //按下F之后才能进行下一句话，进行timeline的播放
                ResumeTimeline();
            }
        }

    }

    public void PauseTimeline(PlayableDirector playableDirector)
    {
        //Debug.Log("Pause Timeline");
        currentPlayableDirector = playableDirector;
        gameMode = GameMode.DialogueMoment;
        currentPlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);
        
        //TODO:显示可以进行下一句话的UI提示
        ToggleNextCorsor(true);
    }
    
    public void ResumeTimeline()
    {
        //Debug.Log("Resume Timeline");
        gameMode = GameMode.GamePlay;
        currentPlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1d);
        
        //TODO:显示对话框和文字
        ToggleNextCorsor(false);
        ToggleDialogueBox(true);
    }

    private void LittleWitchAwake()
    {
        //playerIsAwake = true;
        if (!uiCamera)
        {
            return;
        }
        
        //相机移动
        uiCamera.transform.DOMove(new Vector3(0,-0.71f,-10f), 1f).SetEase(Ease.InOutSine)
            .onComplete = OnCameraMoveFinished;
        
        //开启光源
        float awakeProcessValue = 0f;
        DOTween.To(() => awakeProcessValue, x => awakeProcessValue = x, 1f, 1f)
            .onUpdate = () =>
        {
            sightLight.pointLightOuterRadius = Mathf.Lerp(2.69f, 8f, awakeProcessValue);
            sunShaftlight.intensity = Mathf.Lerp(0f, 10f, awakeProcessValue);
        };
    }

    private void Shake(GameObject target,float powerX, float powerY)
    {
        target.transform.DOShakePosition(1f, new Vector3(powerX, powerY, 0f), 10, 180, false)
            .SetLoops(1, LoopType.Incremental);
    }
    
    /// <summary>
    /// Callback：当相机移动结束时调用，打开对话panel
    /// TODO：对话窗口打开之后应该要调用开场的Timeline
    /// </summary>
    private void OnCameraMoveFinished()
    {
        //Debug.Log("Camera move finished");
        if (mainPanel)
        {
            ShowDialoguePanel(() =>
            {
                //Debug.Log("Panel move finished, show text");
                currentPlayableDirector.Play();
            });
        }
    }
    
    /// <summary>
    /// 打开对话框
    /// </summary>
    /// <param name="onDialoguePanelShowed">对话框完成之后执行的回调函数</param>
    public void ShowDialoguePanel(Action onDialoguePanelShowed = null)
    {
        ResetDialoguePanel();
        mainPanel.SetActive(true);
        RectTransform rectTransform = mainPanel.GetComponent<RectTransform>();
        rectTransform.DOAnchorPos(new Vector2(960f, -884f), 1f).SetEase(Ease.InOutSine);
        rectTransform.DOSizeDelta(new Vector2(1170f,316.37f),1f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            onDialoguePanelShowed?.Invoke();
        });
    }
    
    public void ResetDialoguePanel()
    {
        dialogueName.text = "";
        dialogueText.text = "";
        mainPanel.GetComponent<RectTransform>().anchoredPosition = dialoguePanelInitialPosition;
        mainPanel.GetComponent<RectTransform>().sizeDelta = dialoguePanelInitialSize;
    }
  

    /// <summary>
    /// Callback：当小女巫醒来时调用
    /// </summary>
    private void OnLittleWitchAwake()
    {
        Debug.Log("Little witch is awake");
    }
    
    
    /// <summary>
    /// 显示对话
    /// </summary>
    /// <param name="dialogue">传入的当前对话</param>
    public void showDialogue(string dialogue,string name, int dialogueSize = 60)
    {
        Debug.Log("Show dialogue");
        //dialogueText.text = dialogue;
        dialogueText.gameObject.SetActive(true);
        dialogueName.gameObject.SetActive(true);
        typingCoroutine = StartCoroutine(TypingText(dialogue));
        dialogueName.text = name;
        dialogueText.fontSize = dialogueSize;
    }

    /// <summary>
    /// 显示对话框
    /// </summary>
    /// <param name="isActive">是否激活</param>
    public void ToggleDialogueBox(bool isActive)
    {
        mainPanel.SetActive(isActive);
    }

    
    /// <summary>
    /// 显示下一句话的光标
    /// </summary>
    /// <param name="isActive">是否激活</param>
    public void ToggleNextCorsor(bool isActive)
    {
        //显示/关闭下一句话的光标
        dialogueNextCursor.SetActive(isActive);
    }
    
    /// <summary>
    /// 实现主对话框逐字打出的效果
    /// </summary>
    /// <param name="textToType">下一句需要输出到主对话框的text</param>
    /// <returns></returns>
    private IEnumerator TypingText(string textToType)
    {
        dialogueText.text = "";
        int index = 0;
        string currentText = "";

        while (index < textToType.Length)
        {
            // 检查是否是富文本标签的起始
            if (textToType[index] == '<')
            {
                // 查找标签的结束位置
                int endTagIndex = textToType.IndexOf('>', index);
                if (endTagIndex != -1)
                {
                    // 将整个标签直接添加到当前文本中
                    string tag = textToType.Substring(index, endTagIndex - index + 1);
                    currentText += tag;
                    index = endTagIndex + 1;
                    continue;
                }
            }

            // 对于非标签字符，进行glitch效果
            for (int i = 0; i < glitchFrames; i++)
            {
                dialogueText.text = currentText + randomChars[Random.Range(0, randomChars.Length)];
                //PlayTypingSound();
                yield return new WaitForSeconds(typingSpeed / glitchFrames);
                dialogueText.text = currentText;
            }

            // 最后显示正确的字符
            currentText += textToType[index];
            dialogueText.text = currentText;
            index++;
            yield return new WaitForSeconds(typingSpeed);
        }
        // 最后确保所有文本正确显示
        dialogueText.text = currentText;
        typingCoroutine = null;
        
    }

    /// <summary>
    /// TODO：播放打字音效
    /// </summary>
    void PlayTypingSound()
    {
        
    }
    
    public void FinishedOpenningCG()
    {
        Debug.Log("Finished CG");
        playerIsAwake = true;
        mainPanel.SetActive(false);
    }
}
