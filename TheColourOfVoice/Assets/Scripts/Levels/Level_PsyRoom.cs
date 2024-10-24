using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
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
    [Title("SkipHint")] public GameObject skipHint;
    
    [SerializeField,ReadOnly] private PlayableDirector playableDirector;
    [Title("SaveData")] public SaveData saveData;
    
    //睡觉相关的参数
    private int shakeCount = 0;
    public  bool playerIsAwake = false; //所有过长动画禁止玩家点击选关按钮的时候都把这个设置为false，懒得改了
    [SerializeField] private Light2D sightLight;
    [SerializeField] private Light2D sunShaftlight;

    public GameObject paperOutline;
    public float interval = 0.5f;
    
    //TimeLine有关的参数
    public enum GameMode
    {
        GamePlay,
       DialogueMoment
    }
    public GameMode gameMode;
    public PlayableDirector currentPlayableDirector;
    
    [Title("Character Portraits")] public List<Sprite> characterPortraits;
    public GameObject LittleWitchPortrait;
    public GameObject CharacterPortrait;
    
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
        
        playableDirector = GameObject.Find("TimeLine").GetComponent<PlayableDirector>();
        
        if (playableDirector == null)
        {
            Debug.LogError("PlayableDirector is not set");
            return;
        }
    }

    private void Start()
    {
        dialogueText.gameObject.SetActive(false);
        dialogueName.gameObject.SetActive(false);
        GameObject.Find("Timeline_ani_skip_hint").SetActive(false);
        dialogueNextCursor.SetActive(false);
        dialoguePanelInitialPosition = mainPanel.GetComponent<RectTransform>().anchoredPosition;
        dialoguePanelInitialSize = mainPanel.GetComponent<RectTransform>().sizeDelta;

        if (saveData.levelsCompleted >= 0)
        {
            PlayAftTimeLine();
        }
        else
        {
            LittleWitchAwake();
        }
        Sequence paperOutlineSequence = DOTween.Sequence();
        paperOutlineSequence.AppendCallback(() => TogglePaperOutline());
        paperOutlineSequence.AppendInterval(interval);
        paperOutlineSequence.SetLoops(-1);
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
        }else

        if (Input.GetKeyDown(KeyCode.Escape) && currentPlayableDirector.state == PlayState.Playing)
        {
            SkipTimeLine();
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
    
    private void TogglePaperOutline( )
    {
        if (paperOutline)
        {
            paperOutline.SetActive(!paperOutline.activeSelf);
        }
       
    }
    
    public void PlayAftTimeLine()
    {
        ResetRoom();
        int currentCaseIndex = PlayerPrefs.GetInt("levelIndex", 0);
        CaseData caseData = Resources.Load<CaseData>("CaseData/CaseData_Case0" + currentCaseIndex);
        Debug.Log("CaseData: "+ currentCaseIndex);
        if (caseData == null)
        {
            //Debug.LogError("CaseData is null");
            ResetRoom();
            return;
        }
        playableDirector.playableAsset = caseData.afterLevelTimelineAsset;
        Level_PsyRoom.Instance.ShowDialoguePanel(PlayTimeline);
    }
    
    /// <summary>
    /// AftTimeLine播放完毕后的回调函数,重置房间的光效
    /// </summary>
    public void OnfinishedAftTimeLine()
    {
        //Debug.Log("Finished CG");
        ResetRoom();
        
    }

    /// <summary>
    /// 根据这关的完成星星数来设置关卡完成的icon
    /// </summary>
    public void SettleLevelCompleteIcon()
    {
        //获得关卡Index
        int currentCaseIndex = PlayerPrefs.GetInt("levelIndex", 0);
        CaseData caseData = Resources.Load<CaseData>("CaseData/CaseData_Case0" + currentCaseIndex);
        Debug.Log("CaseData: "+ currentCaseIndex);
        if (caseData == null)
        {
            //Debug.LogError("CaseData is null");
            ResetRoom();
            return;
        }
        
        //获得关卡完成的星星数
        int currentLevelStatCount = SaveDataManager.Instance.saveData.levelStars[currentCaseIndex - 1];
        Debug.Log("currentLevelStatCount: "+ currentLevelStatCount);
        
        choosingLevelPanel.SetActive(true);
        
    }

    private void ResetRoom()
    {
        sightLight.pointLightOuterRadius = 8.0f;
        sunShaftlight.intensity = 10.0f;
        skipHint.SetActive(false);
        playerIsAwake = true;
    }

    private void PlayTimeline()
    {
        playableDirector.Play();
        GameObject.Find("Timeline_ani_skip_hint").SetActive(true);
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
        //删除按下空格醒来的提示,测试用//TODO:记得删除
        GameObject.Find("Temp").SetActive(false);
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

    Tweener tween;
    
    private void Shake(GameObject target,float powerX, float powerY)
    {
        if (tween != null && tween.IsActive()) return;
        tween = target.transform.DOShakePosition(1f, new Vector3(powerX, powerY, 0f), 10, 180, false)
            .SetLoops(1, LoopType.Incremental).OnComplete(() => tween = null);
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
        LittleWitchPortrait.SetActive(false);
        CharacterPortrait.SetActive(false);
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
        //Debug.Log("Show dialogue");
        dialogueText.text = dialogue;
        dialogueText.gameObject.SetActive(true);
        dialogueName.gameObject.SetActive(true);
        //typingCoroutine = StartCoroutine(TypingText(dialogue));
        dialogueName.text = name;
        dialogueText.fontSize = dialogueSize;

        if (name == "LittleWitch")
        {
            ResetPortrait();
            LittleWitchPortrait.GetComponent<Image>().sprite = characterPortraits[0];
            LittleWitchPortrait.SetActive(true);
        }else if (name == "Ava")
        {
            ResetPortrait();
            CharacterPortrait.GetComponent<Image>().sprite = characterPortraits[1];
            CharacterPortrait.SetActive(true);
        }else if (name == "Benjamin")
        {
            ResetPortrait();
            CharacterPortrait.GetComponent<Image>().sprite = characterPortraits[2];
            CharacterPortrait.SetActive(true);
        }
        else if (name == "Chloe")
        {
            ResetPortrait();
            CharacterPortrait.GetComponent<Image>().sprite = characterPortraits[3];
            CharacterPortrait.SetActive(true);
        }else if (name == "Ron")
        {
            ResetPortrait();
            CharacterPortrait.GetComponent<Image>().sprite = characterPortraits[4];
            CharacterPortrait.SetActive(true);
        }
        else if (name == "Nioneisos")
        {
            ResetPortrait();
            CharacterPortrait.GetComponent<Image>().sprite = characterPortraits[5];
            CharacterPortrait.SetActive(true);
        }
        else
        {
            ResetPortrait();
        }
    }
    
    private void  ResetPortrait()
    {
        LittleWitchPortrait.SetActive(false);
        CharacterPortrait.SetActive(false);
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
    /// 切换关卡
    /// </summary>
    public void ChangeLevel(bool isNextLevelTutorial = false)
    {
        if (saveData.levelsCompleted < 0)
        {
            isNextLevelTutorial = true;
        }

        if (isNextLevelTutorial)
        {
            PlayerPrefs.SetInt("levelIndex", 0);
            Lebug.Log("levelIndex", 0);
            SceneTransit.Instance.LoadTargetScene("Tutorial");
            return;
        }
        SceneTransit.Instance.LoadTargetScene("MainGame");
    }
    
    public void FinishedOpenningCG()
    {
        //Debug.Log("Finished CG");
       
        playerIsAwake = true;
        mainPanel.SetActive(false);
    }
    
    private void SkipTimeLine()
    {
        if (playableDirector != null)
        {
            // 将时间设置为 Timeline 的总时长，即跳到结尾
            playableDirector.time = playableDirector.duration;

            // 立即评估（Evaluate）以跳转到设置的时间
            playableDirector.Evaluate();
        }
    }
}
