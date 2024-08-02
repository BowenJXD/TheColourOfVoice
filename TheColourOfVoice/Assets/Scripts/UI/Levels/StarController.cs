using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Utility;

public class StarController : MonoBehaviour
{
    public GameObject playerUI; 
    public Image[] stars; 
    public float score; 
    public float[] scoreThresholds; 
    [SerializeField] ScoreBar scoreBar;
    [SerializeField] TMP_Text percentText;
    
    void Awake()
    {
        var data = ResourceManager.Instance.LoadCSV(PathDefines.StarScores);
        var levelIndex = PlayerPrefs.GetInt("levelIndex", 1);
        var levelData = data[levelIndex.ToString()];
        scoreThresholds = new float[3];
        for (int i = 0; i < 3; i++)
        {
            scoreThresholds[i] = float.Parse(levelData[(i + 1).ToString()]);
        }
        
        foreach (Image star in stars)
        {
            SetStarColor(star, false);
        }
        playerUI.SetActive(false);
        UpdateStars();
    }


    void SetStarColor(Image star, bool isBright)
    {
        if (isBright)
        {
            star.color = new Color(1f, 1f, 1f); 
        }
        else
        {
            star.color = new Color(0.5f, 0.5f, 0.5f); 
        }
    }


    public void UpdateStars()
    {
        score = scoreBar.score;
        percentText.text = Mathf.RoundToInt(score).ToString();
        
        int starCount = 0;
        for (int i = 0; i < stars.Length; i++)
        {
            if (score >= scoreThresholds[i])
            {
                SetStarColor(stars[i], true);
                starCount++;
            }
            else
            {
                SetStarColor(stars[i], false);
            }
        }

        Resources.Load<SaveData>(PathDefines.SaveData).levelStars[PlayerPrefs.GetInt("levelIndex", 1) - 1] = starCount;
    }
}