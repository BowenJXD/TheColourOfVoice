using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreBar : MonoBehaviour
{
    [SerializeField] Image fillImageScore;
    [SerializeField] SplashGrid splashGrid;
    [SerializeField] float maxScore = 10000f; 
    protected float percentage;
    public float score;
    [SerializeField] TMP_Text percentText;
    
    private Image starDarkImage;
    private Image starLightImage;

    public Transform starContainer;  
    private List<Image> stars = new List<Image>();
    
    Canvas canvas;
    void SetScoreText(float score)
    {
        percentText.text = Mathf.RoundToInt(score).ToString();
    }

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        
        starContainer.SetParent(this.transform);

        /*RectTransform starContainerRect = starContainer.Setc<RectTransform>();
        starContainerRect.anchorMin = new Vector2(0f, 0.5f);
        starContainerRect.anchorMax = new Vector2(1f, 0.5f);
        starContainerRect.anchoredPosition = Vector2.zero;
        starContainerRect.sizeDelta = new Vector2(0, 0); */
        
        GameObject starPrefab = Resources.Load<GameObject>("Prefabs/Level/StarPrefabs");

        if (starPrefab == null)
        {
            Debug.LogError("Failed to load star prefab from Resources.");
            return;
        }
        
        Transform starDarkTransform  =  starPrefab.transform.Find("StarDark");
        Transform starLightTransform  =  starPrefab.transform.Find("StarLight");

        starDarkImage = starDarkTransform.GetComponent<Image>();
        starLightImage = starLightTransform.GetComponent<Image>();

        float[] starScores = { 1000f, 2000f, 3000f }; 
        float[] starPositions = CalculateStarPositions(starScores, maxScore);
    
        InitializeStars(starDarkImage, starLightImage, starPositions);
        
        UpdateScore(0);
        StartCoroutine(UpdatePercentageEverySecond());

        // Load max score from csv
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
    
    
    void InitializeStars(Image starDarkPrefab, Image starLightPrefab, float[] starPositions)
    {
        foreach (float position in starPositions)
        {
            Image star = Instantiate(starDarkPrefab, starContainer);
            star.gameObject.SetActive(true);

            RectTransform starRect = star.GetComponent<RectTransform>();
            starRect.anchorMin = new Vector2(position, 0.5f);
            starRect.anchorMax = new Vector2(position, 0.5f);
            starRect.anchoredPosition = Vector2.zero;

            stars.Add(star);
        }
    }

    
    float[] CalculateStarPositions(float[] starScores, float maxScore)
    {
        float[] starPositions = new float[starScores.Length];
        for (int i = 0; i < starScores.Length; i++)
        {
            starPositions[i] = starScores[i] / maxScore;
        }
        return starPositions;
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
        SetScoreText(score);
        OnScoreChanged?.Invoke(newPercentage);

        UpdateUI();
        StartCoroutine(FlashScoreText());
        UpdateStars();

    }

    void UpdateStars()
    {
        for (int i = 0; i < stars.Count; i++)
        {
            if (score >= stars[i].GetComponent<RectTransform>().anchorMin.x * maxScore)
            {
                if (starLightImage != null)
                {
                    stars[i].sprite = starLightImage.sprite;
                    /*Destroy(stars[i].gameObject); 
                    Image starLight = Instantiate(starLightImage, starContainer);
                    stars[i] = starLight;
                
                    RectTransform starRect = stars[i].GetComponent<RectTransform>();
                    starRect.anchorMin = new Vector2(starRect.anchorMin.x, 0.5f);
                    starRect.anchorMax = new Vector2(starRect.anchorMin.x, 0.5f);
                    starRect.anchoredPosition = Vector2.zero;*/
                }
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

