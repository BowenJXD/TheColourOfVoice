using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StarController : MonoBehaviour
{
    public GameObject playerUI; 
    public GameObject[] stars;  // 
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
        
        foreach (GameObject star in stars)
        {
            SetStarColor(star.GetComponent<Image>(), true);  
        }
        playerUI.SetActive(false);
        UpdateStars();
    }


    void SetStarColor(Image star, bool isBright)
    {
        if (isBright)
        {
            star.overrideSprite = Resources.Load<Sprite>("Arts/UI/MarkStar/MarkStar1");
            star.color = new Color(1f, 1f, 1f); 
        }
        else
        {
            star.overrideSprite = Resources.Load<Sprite>("Arts/UI/MarkStar/MarkStar2");
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
                SetStarColor(stars[i].GetComponent<Image>(), true);
                starCount++;
            }
            else
            {
                SetStarColor(stars[i].GetComponent<Image>(), false);
            }
        }

        Resources.Load<SaveData>(PathDefines.SaveData).levelStars[PlayerPrefs.GetInt("levelIndex", 1) - 1] = starCount;
    }
}