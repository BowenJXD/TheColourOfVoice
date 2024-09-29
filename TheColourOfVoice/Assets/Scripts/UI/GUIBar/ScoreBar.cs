using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

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
    Canvas canvas;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        
        GameObject starPrefab = Resources.Load<GameObject>("Prefabs/Level/StarPrefabs");

        if (starPrefab == null)
        {
            Debug.LogError("Failed to load star prefab from Resources.");
            return;
        }

        InitializeStars(starPrefab);
        
        UpdateScore(0);
        StartCoroutine(UpdatePercentageEverySecond());

        // Load max score from CSV
        try
        {
            var data = ResourceManager.Instance.LoadCSV(PathDefines.StarScores);
            var levelIndex = PlayerPrefs.GetInt("levelIndex", 1);
            var levelData = data[levelIndex.ToString()];
            maxScore = float.Parse(levelData["3"]);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
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

    IEnumerator UpdatePercentageEverySecond()
    {
        while (true) 
        {
            yield return new WaitForSeconds(1f); 
            Initialize(splashGrid); 
        }
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
        for (int i = 0; i < stars.Count; i++)
        {
            if (stars[i] == null)
            {
                Debug.LogError($"Star at index {i} is null.");
                continue;
            }
            float starPositionX = Mathf.Lerp(leftPosition.x, rightPosition.x, scoreThresholds[i] / maxScore);

            RectTransform starRect = stars[i].GetComponent<RectTransform>();
            starRect.anchoredPosition = new Vector3(starPositionX, leftPosition.y, 0f);

            if (score >= scoreThresholds[i])
            {
                stars[i].overrideSprite = Resources.Load<Sprite>("Arts/UI/MarkStar/MarkStar1");
                stars[i].color = Color.white;  
            }
            else
            {
                stars[i].overrideSprite = Resources.Load<Sprite>("Arts/UI/MarkStar/MarkStar2");

                stars[i].color = new Color(1f, 1f, 1f, 0.3f);  
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
