using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;

public class ScoreBar : MonoBehaviour
{
    [SerializeField] Image fillImageScore;
    [SerializeField] SplashGrid splashGrid;
    [SerializeField] public float maxScore = 10000f; 
    protected float percentage;
    public float score;
    [SerializeField] TMP_Text percentText;
    
    private List<Image> stars = new List<Image>();
    private Vector3 leftPosition = new Vector3(-717f, 73f, 0f);  
    private Vector3 rightPosition = new Vector3(717f, 73f, 0f);  
    [SerializeField] StarController starController; 
    [SerializeField] List<Image> starList;

    Canvas canvas;

    [SerializeField] public Image thousandImage;
    [SerializeField] public Image hundredImage;
    [SerializeField] public Image tenImage;
    [SerializeField] public Image oneImage;
    [SerializeField] public Sprite[] numberSprites; // 包含 0-9 数字的图片
    
    [Title("流光材质")] public Material targetMaterial;
    public string targetMaterialColorName = "_Color02";
    private Color targetColor;
    private float minColorTiles = 0f;
    private float maxColorTiles = 4000f;
    private float minFloatValue = -3f;
    private float maxFloatValue = 3f;
    
    [Title("滑动Title")]
    public GameObject targetTilesTitle; 
    private RectTransform targetTitleRect;
    public float fastSpeed = 1f;     
    public float slowSpeed = 1.5f;
    public bool doSlide = true;
    [SerializeField] public Image slidethousandImage;
    [SerializeField] public Image slidehundredImage;
    [SerializeField] public Image slidetenImage;
    [SerializeField] public Image slideoneImage;
    
    private int timer = 1;
    
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        
        GameObject starPrefab = Resources.Load<GameObject>("Prefabs/Level/StarPrefabs");
        // numberSprites = Resources.LoadAll<Sprite>("Fonts/pix_nums");

        if (starPrefab == null)
        {
            Debug.LogError("Failed to load star prefab from Resources.");
            return;
        }

        // InitializeStars(starPrefab);
        
        UpdateScore(0);
        StartCoroutine(UpdatePercentageEverySecond());

        // Load max score from CSV
        try
        {
            var data = ResourceManager.Instance.LoadCSV(PathDefines.StarScores);
            var levelIndex = PlayerPrefs.GetInt("levelIndex", LevelManager.Instance.levelIndex);
            var levelData = data[levelIndex.ToString()];
            maxScore = float.Parse(levelData["3"]);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    private void Start()
    {
        targetColor = targetMaterial.GetColor(targetMaterialColorName);
        targetTitleRect = targetTilesTitle.GetComponent<RectTransform>();
        SlideImage();
    }

    //初始化星星
    void InitializeStars(GameObject starPrefab)
    {
        for (int i = 0; i < 3; i++) 
        {
            GameObject star = Instantiate(starPrefab, canvas.transform);
            star.gameObject.SetActive(true);
            stars.Add(star.GetComponent<Image>());
            
            //调整星星位置
        }

        if (starController) UpdateStars();
    }
    
    void UpdateMaterialParameter()
    {
        // 将分数从 [minScore, maxScore] 映射到 [minFloatValue, maxFloatValue]
        float mappedValue = Mathf.Lerp(minFloatValue, maxFloatValue, Mathf.InverseLerp(minColorTiles, maxScore, splashGrid.PaintedCount));
        
        // 更新材质的 float 参数
        if (targetMaterial != null)
        {
            //targetMaterial.SetFloat(targetMaterialColorName, mappedValue);
            Color currentColor = targetColor* mappedValue;
            targetMaterial.SetColor(targetMaterialColorName, currentColor);
        }
    }

    IEnumerator UpdatePercentageEverySecond()
    {
        while (true) 
        {
            yield return new WaitForSeconds(1f); 
            Initialize(splashGrid);
            UpdateScoreDisplay(splashGrid.PaintedCount);
        }
    }
    void UpdateScoreDisplay(int PaintedCount)
    {
        Lebug.Log("PaintedCount: ", PaintedCount);
        Debug.Log("PaintedCount: " + PaintedCount);
        // 计算各个位数
        int thousand = (PaintedCount / 1000) % 10;
        int hundred = (PaintedCount / 100) % 10;
        int ten = (PaintedCount / 10) % 10;
        int one = PaintedCount % 10;

        // 更新四个数字对应的图片
        if (thousandImage)
        {
            thousandImage.sprite = numberSprites[thousand];
            AnimateImage(thousandImage);
        }

        if (hundredImage)
        {
            hundredImage.sprite = numberSprites[hundred];
            AnimateImage(hundredImage);
        }

        if (tenImage)
        {
            tenImage.sprite = numberSprites[ten];
            AnimateImage(tenImage);
        }

        if (oneImage)
        {
            oneImage.sprite = numberSprites[one];
            AnimateImage(oneImage);
        }

        if (PaintedCount -(500* timer) >= 0 && doSlide)
        {
            slidethousandImage.sprite = thousandImage.sprite;
            slidehundredImage.sprite = hundredImage.sprite;
            slidetenImage.sprite = tenImage.sprite;
            slideoneImage.sprite = oneImage.sprite;
            SlideImage();
            timer++;
        }
        //UpdateMaterialParameter();
    }

    public void AnimateImage(Image targetImage)
    {
        // 初始缩放比例
        float originalScale = 1f;

        // 目标缩放比例，表示变大多少
        float targetScale = 1.5f;

        // 动画持续时间
        float duration = 0.3f;

        // 变大到目标比例，然后缩回原始比例
        targetImage.rectTransform.DOScale(targetScale, duration)
            .SetEase(Ease.OutQuad)     
            .OnComplete(() =>          
            {
                // 缩回到原始比例
                targetImage.rectTransform.DOScale(originalScale, duration)
                    .SetEase(Ease.InQuad); 
            });
    }
    
    void SlideImage()
    {
        // 创建一个序列
        Sequence sequence = DOTween.Sequence();

        // 计算起始位置和目标位置
        Vector3 startPosition = new Vector3(-428.0f,247f,0);  // Start
        Vector3 midPosition = new Vector3(140f,247f,0);         // Mid
        Vector3 endPosition = new Vector3(783f,247f,0);     // End

        // 设置起始位置
        targetTitleRect.anchoredPosition = startPosition;

        // 添加进入屏幕的快速滑动动画
        sequence.Append(targetTitleRect.DOAnchorPos(midPosition, fastSpeed)
            .SetEase(Ease.OutCubic));  // 快速滑动，并缓慢减速到屏幕内

        // 添加在屏幕内缓慢滑动的动画
        /*sequence.Append(targetTitleRect.DOAnchorPos(new Vector2(midPosition.x + 200, midPosition.y), slowSpeed)
            .SetEase(Ease.Linear));    // 匀速滑动一段时间*/

        // 添加离开屏幕的快速滑动动画
        sequence.Append(targetTitleRect.DOAnchorPos(endPosition, fastSpeed)
            .SetEase(Ease.InCubic));   // 快速滑动并加速离开屏幕

        // 播放序列动画
        sequence.Play();
    }

    protected virtual void Initialize(SplashGrid splashGrid)
    {
        percentage = splashGrid.paintedPercentage;
        UpdateScore(percentage);
    }
    
    public Action<float> OnScoreChanged;

    protected virtual void UpdateScore(float newPercentage)
    {
        float addedScore = newPercentage * 100;

        score += addedScore;
        score = Mathf.Min(score, maxScore);
        OnScoreChanged?.Invoke(newPercentage);
        SetScoreText(score);

        UpdateUI();
        StartCoroutine(FlashScoreText());
        if (starController) UpdateStars();
    }

    void UpdateStars()
    {
        //Debug.Log("Updating stars.");
        float[] scoreThresholds = starController.scoreThresholds;
        for (int i = 0; i < starList.Count; i++)
        {
            if (starList[i] == null)
            {
                Debug.LogError($"Star at index {i} is null.");
                continue;
            }
            /*
            float starPositionX = Mathf.Lerp(leftPosition.x, rightPosition.x, scoreThresholds[i] / maxScore);

            RectTransform starRect = stars[i].GetComponent<RectTransform>();
            starRect.anchoredPosition = new Vector3(starPositionX, leftPosition.y, 0f);
            */

            if (score >= scoreThresholds[i])
            {
                /*stars[i].overrideSprite = Resources.Load<Sprite>("Arts/UI/MarkStar/MarkStar1");
                stars[i].color = Color.white;  */
                starList[i].color = Color.white;
            }
            else
            {
                /*
                stars[i].overrideSprite = Resources.Load<Sprite>("Arts/UI/MarkStar/MarkStar2");
                */
                starList[i].color = new Color(1f, 1f, 1f, 0.3f);

                /*
                stars[i].color = new Color(1f, 1f, 1f, 0.3f);  
            */
            }
        }
    }

    void UpdateUI()
    {
        if (fillImageScore != null)
        {
            fillImageScore.fillAmount = score / maxScore;
        }
    }

    void SetScoreText(float score)
    {
        percentText.text = Mathf.RoundToInt(score).ToString();
    }

    IEnumerator FlashScoreText()
    {
        TMP_Text text = percentText;
        Color originalColor = text.color;
        Color flashColor = ColorManager.Instance.GetColor(LevelManager.Instance.levelColor);

        text.color = flashColor;

        text.ForceMeshUpdate();
        Mesh mesh = text.mesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] originalVertices = (Vector3[])vertices.Clone();

        float duration = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 offset = Wobble(Time.time + i);
                vertices[i] = originalVertices[i] + offset * (1 - t); 
            }

            mesh.vertices = vertices;
            text.canvasRenderer.SetMesh(mesh);

            yield return null;
        }
        
        text.color = originalColor;
        text.ForceMeshUpdate();
        mesh.vertices = originalVertices;
        text.canvasRenderer.SetMesh(mesh);
    }

    Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * 330f), Mathf.Cos(time * 250f)) * 25f; 
    }
}
