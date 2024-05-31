using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        for (int i = 0; i < stars.Length; i++)
        {
            if (score >= scoreThresholds[i])
            {
                SetStarColor(stars[i], true);
            }
            else
            {
                SetStarColor(stars[i], false);
            }
        }
    }
}