using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Level_demo : MonoBehaviour
{
    [SerializeField] public SequenceEventExecutor week6SequenceEventExcutor;
    [SerializeField] private GameObject player;
    public bool skip = false;
    public int totalTime = 180;
    public TextMeshProUGUI text;
    [SerializeField] public GameObject UICanvas;
    private GameObject playerUI;
    private GameObject endLevelUI;
    [SerializeField] ParticleSystem myParticleSystem;
    [SerializeField] public GameObject StarController;
    private int index;
    private GameObject[] selectedSpell; // 引用选定的法术对象
    
    public float targetOrthoSize = 10f;
    public float zoomDuration = 1f;
    public Ease zoomEase = Ease.OutCubic;

    private bool chaosTimer = false;
    public Sprite[] numberSprites;  // 包含0-9的Sprite数组
    public Image  thousandImage, hundredImage, tenImage, oneImage;  // 用于显示每个位的SpriteRenderer
    public ScoreBar scoreBar;
    [SerializeField] private Text resultText;
    [SerializeField] private Image ratingImage;  // 评级的Image
    public GameObject UIScore;
    public GameObject UIStar;
    public Sprite rate1, rate2, rate3;

    private void Awake()
    {
        index = SceneManager.GetActiveScene().buildIndex;
    }

    void Start()
    {
        /*player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponentInChildren<Fire>().enabled = false;
        Time.timeScale = 0;*/
        
        // 确保 ratingImage 一开始是隐藏的
        ratingImage.gameObject.SetActive(false);
        ratingImage.color = new Color(ratingImage.color.r, ratingImage.color.g, ratingImage.color.b, 0);  // 设置透明度为0

        
        if (!skip && week6SequenceEventExcutor)
        {
            week6SequenceEventExcutor.Init(OnFinishedEvent);
            Debug.Log("Excute sequence");
            week6SequenceEventExcutor.Excute();
        }
        
        if (UICanvas != null)
        {
            playerUI = UICanvas.transform.Find("PlayerUI").gameObject;
            endLevelUI = UICanvas.transform.Find("EndLevelUI").gameObject;
            /*playerUI.SetActive(true);
            endLevelUI.SetActive(false);*/
        }
        if (!player)
        {
            return;
        }
        selectedSpell = new GameObject[3];
        if(player!=null)
        {
            Transform spellManagerTransform = player.transform.Find("SpellManager/Spells");
            if(selectedSpell!=null)
            {
                /*selectedSpell[0] = spellManagerTransform.Find("The Glowing Confidence").gameObject;
                selectedSpell[1] = spellManagerTransform.Find("The Burning Passion").gameObject;
                selectedSpell[2] = spellManagerTransform.Find("The Calming Laser").gameObject;*/

            }
        }
        
        if (skip)
        {
            Debug.Log("Skipdialogue");
            OnFinishedEvent(false);
            return;
        }
    }

    void OnFinishedEvent(bool success)
    {
        Debug.Log(success);
        StartCoroutine(Timer());
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponentInChildren<Fire>().enabled = true;
        Time.timeScale = 1;
        TextPainter.Instance.PaintText();

    }

    IEnumerator Timer()
    {
        TextMeshProUGUI timerText = text.GetComponent<TextMeshProUGUI>();
        while (totalTime >= 0)
        {
            if (!chaosTimer)
            {
                int minutes = totalTime / 60;
                int seconds = totalTime % 60;
                timerText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
                yield return new WaitForSeconds(1);
                totalTime--;
            }
            else
            {
                char c1 = Util.GetRandomElement(new []{'@', '#', '$', '%', '&', '?'});
                char c2 = Util.GetRandomElement(new []{'@', '#', '$', '%', '&', '?'});
                char c3 = Util.GetRandomElement(new []{'@', '#', '$', '%', '&', '?'});
                timerText.text = string.Format("{0}:{1}{2}", c1, c2, c3);
                yield return null;
            }
        }

        Debug.Log("Time's up!");
        Time.timeScale = 0;
        LogUtil.Instance.LogCSV();
        if (totalTime <= 0)
        {
            EndLevel(() => ShowEndLevelUI());
            //ShowEndLevelUI();
            
        }

    }

    public void EndLevel(Action callback)
    {
        VolumeControlOrthoSize.Instance.enabled = false;
        var cam = FindObjectOfType<CinemachineVirtualCamera>();
        // Get the current orthographic size of the camera
        float currentOrthoSize = cam.m_Lens.OrthographicSize;

        // Use DOTween to smoothly transition to the target orthographic size
        DOTween.To(() => currentOrthoSize, x => cam.m_Lens.OrthographicSize = x, targetOrthoSize, zoomDuration)
            .SetEase(zoomEase)
            .SetUpdate(true)
            .OnComplete(callback.Invoke);
    }

    public void ShowEndLevelUI()
    {
        StarController.GetComponent<StarController>().UpdateStars();
        if (playerUI != null)
        {
            playerUI.SetActive(false); 
        }

        if (endLevelUI != null)
        {
            endLevelUI.SetActive(true); 
            // 获取最终分数并启动动画显示
            float finalScore = scoreBar.score;  // 通过 ScoreBar 获取分数
            StartCoroutine(AnimateScore((int)finalScore));  // 假设分数为整数类型，转换为 int 类型传递
            
            LeanTween.moveLocal(endLevelUI, new Vector3(0f, -20f, 0f), 1f).setIgnoreTimeScale(true).setEase(LeanTweenType.easeInOutBack).setOnComplete(() =>
            {
                GameObject[] stars = StarController.GetComponent<StarController>().stars;
                int completedStars = 0;  // 已完成动画的星星数
                int starCount = stars.Length;  // 星星总数

                for (int i = 0; i < stars.Length; i++)
                {
                    int currentIndex = i;
                    
                    stars[currentIndex].transform.localScale = Vector3.zero;
                    stars[currentIndex].SetActive(true);
                    

                    LeanTween.scale(stars[currentIndex].gameObject, Vector3.one * 0.6f, 1f)
                        .setEase(LeanTweenType.easeOutBounce)
                        .setDelay(0.05f * currentIndex)
                        .setIgnoreTimeScale(true)
                        .setOnComplete(() => {
                            Transform fxStar = stars[currentIndex].transform.Find("Fx_Star");
                            if (fxStar != null)
                            {
                                fxStar.gameObject.SetActive(true);
                            }
                            else
                            {
                                Debug.LogError("Fx_Star not found in " + stars[currentIndex].name);
                            }
                            
                            // 当所有星星的动画完成时，继续后续操作
                            completedStars++;
                            if (completedStars >= starCount)
                            {
                                // 所有星星动画结束后，开始移动 Score 和 StarController
                                MoveScoreAndStars();
                            }
                        });

                }
            });
            // 获取星星数量并更新 Text 组件内容
            int starCount = StarController.GetComponent<StarController>().starcount;
            UpdateResultText(starCount);
            
        }

    }
    
    void MoveScoreAndStars()
    {
        // 假设 Score 是整体 UI 的分数显示部分，保持对 Score 的移动
        LeanTween.moveLocal(UIScore, UIScore.transform.localPosition + new Vector3(-50f, 0f, 0f), 1f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setIgnoreTimeScale(true);
    
        // 控制 Stars1、Stars2、Stars3 向左平移
        LeanTween.moveLocal(UIStar.transform.Find("Stars1").gameObject, UIStar.transform.Find("Stars1").localPosition + new Vector3(-40f, 0f, 0f), 1f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setIgnoreTimeScale(true);

        LeanTween.moveLocal(UIStar.transform.Find("Stars2").gameObject, UIStar.transform.Find("Stars2").localPosition + new Vector3(-50f, 0f, 0f), 1f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setDelay(0.1f)  // 延迟0.1秒，稍微错开
            .setIgnoreTimeScale(true);

        LeanTween.moveLocal(UIStar.transform.Find("Stars3").gameObject, UIStar.transform.Find("Stars3").localPosition + new Vector3(-60f, 0f, 0f), 1f)
            .setEase(LeanTweenType.easeInOutQuad)
            .setDelay(0.2f)  // 延迟0.2秒，依次错开
            .setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                // 当三个星星平移动画结束后，显示敲章动画
                int starCount = StarController.GetComponent<StarController>().starcount;
                ShowRatingImage(starCount);
            });
    }

    

    IEnumerator AnimateScore(int finalScore)
    {
        // 将分数拆分为各个位
        int thousand = finalScore / 1000;
        int hundred = (finalScore / 100) % 10;
        int ten = (finalScore / 10) % 10;
        int one = finalScore % 10;

        // 动画显示每个位数从 0 变化到目标值
        yield return StartCoroutine(ChangeDigit(thousandImage, thousand));
        yield return StartCoroutine(ChangeDigit(hundredImage, hundred));
        yield return StartCoroutine(ChangeDigit(tenImage, ten));
        yield return StartCoroutine(ChangeDigit(oneImage, one));
    }
    IEnumerator ChangeDigit(Image digitImage, int targetNumber)
    {
        int currentNumber = 0;
        Debug.Log($"Start animating digit. Target: {targetNumber}");

        // 确保传递的数字正确
        if (targetNumber < 0 || targetNumber > 9)
        {
            Debug.LogError("Invalid target number: " + targetNumber);
            yield break;
        }

        // 循环从 0 到目标值，多循环几次以增加动画效果
        int maxLoops = 10;  // 控制循环次数
        int loopCount = 0;
    
        while (loopCount < maxLoops || currentNumber != targetNumber)
        {
            currentNumber = (currentNumber + 1) % 10;
            Debug.Log($"Updating digit image to: {currentNumber}");
            digitImage.sprite = numberSprites[currentNumber];  // 动态切换数字图片

            // 控制数字变化速度
            yield return new WaitForSecondsRealtime(0.05f);  // 可以调小以加快动画速度

            if (currentNumber == targetNumber && loopCount >= maxLoops)
            {
                break;  // 达到目标数字并且循环次数足够时退出
            }

            loopCount++;
        }

        // 最终数字动画，可以增加缩放效果
        LeanTween.scale(digitImage.gameObject, Vector3.one * 1.2f, 0.2f)
            .setEase(LeanTweenType.easeInOutBack)
            .setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                LeanTween.scale(digitImage.gameObject, Vector3.one, 0.2f)
                    .setEase(LeanTweenType.easeInOutBack)
                    .setIgnoreTimeScale(true);
            });

        Debug.Log("Digit animation complete for: " + targetNumber);
    }


// 根据星星数量更新文字
    void UpdateResultText(int starCount)
    {
        if (starCount == 1)
        {
            resultText.text = "PASS";
        }
        else if (starCount == 2)
        {
            resultText.text = "GOOD";
        }
        else if (starCount == 3)
        {
            resultText.text = "Distinguish";
        }
        else
        {
            resultText.text = "FINE";
        }
    }
    
    void ShowRatingImage(int starCount)
    {
        // 根据星星数量设置不同的 image
        if (starCount == 1)
        {
            ratingImage.sprite =rate1;
        }
        else if (starCount == 2)
        {
            ratingImage.sprite = rate2;
        }
        else if (starCount == 3)
        {
            ratingImage.sprite = rate3;
        }
        else
        {
            ratingImage.sprite = rate1;
        }

        // 显示 ratingImage
        ratingImage.gameObject.SetActive(true);

        // 初始缩放和透明度
        ratingImage.transform.localScale = Vector3.zero;
        ratingImage.color = new Color(ratingImage.color.r, ratingImage.color.g, ratingImage.color.b, 0);
    
        // 添加旋转、缩放和透明度动画
        LeanTween.scale(ratingImage.gameObject, Vector3.one, 1f)
            .setEase(LeanTweenType.easeOutElastic)
            .setIgnoreTimeScale(true);

        LeanTween.rotateZ(ratingImage.gameObject, -45f, 1f)
            .setEase(LeanTweenType.easeOutBack)
            .setIgnoreTimeScale(true);

        LeanTween.value(ratingImage.gameObject, 0f, 1f, 1.5f)
            .setOnUpdate((float val) =>
            {
                Color c = ratingImage.color;
                c.a = val;
                ratingImage.color = c;
            })
            .setEase(LeanTweenType.easeInOutQuad)
            .setIgnoreTimeScale(true);
    }
    public void ResetTimer()
    {
        totalTime = 180; 
        Time.timeScale = 1; 
    }

    public void SetChaosTimer(bool enable)
    {
        chaosTimer = enable;
    }
}