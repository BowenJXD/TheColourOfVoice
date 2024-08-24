using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.Universal;

public class Level_PsyRoom : Singleton<Level_PsyRoom>
{
    public GameObject player;
    public Camera uiCamera;
    public GameObject mainPanel;//对话panel
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI dialogueName;
    public GameObject choosingLevelPanel;
    private int shakeCount = 0;
    private bool playerIsAwake = false;
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

    protected override void Awake()
    {
        base.Awake();
        gameMode = GameMode.GamePlay;
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
        Debug.Log("Pause Timeline");
        currentPlayableDirector = playableDirector;
        gameMode = GameMode.DialogueMoment;
        currentPlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);
        
        //TODO:显示可以进行下一句话的UI提示
        ToggleNextCorsor(true);
    }
    
    public void ResumeTimeline()
    {
        Debug.Log("Resume Timeline");
        gameMode = GameMode.GamePlay;
        currentPlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1d);
        
        //TODO:显示对话框和文字
        ToggleNextCorsor(false);
        ToggleDialogueBox(true);
    }

    private void LittleWitchAwake()
    {
        playerIsAwake = true;
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
        Debug.Log("Camera move finished");
        if (mainPanel)
        {
           
            mainPanel.SetActive(true);
            RectTransform rectTransform = mainPanel.GetComponent<RectTransform>();
            rectTransform.DOAnchorPos(new Vector2(960f, -884f), 1f).SetEase(Ease.InOutSine);
            rectTransform.DOSizeDelta(new Vector2(1170f,316.37f),1f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                //TODO:显示文字
                Debug.Log("Panel move finished, show text");
                currentPlayableDirector.Play();
            });
        }
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
        dialogueText.text = dialogue;
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
        //TODO:显示下一句话的光标
    }
}
