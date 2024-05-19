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
/*    [SerializeField] Text percentText;*/
    [SerializeField] TMP_Text percentText;

    Canvas canvas;
    void SetScoreText(float score)
    {
        percentText.text = Mathf.RoundToInt(score).ToString();
    }

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        UpdateScore(0);
        StartCoroutine(UpdatePercentageEverySecond());
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
/*        SetPercentText(0);
*/
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

    }

    void UpdateUI()
    {
        if (fillImageScore != null)
        {
            fillImageScore.fillAmount = score / maxScore;
            //Debug.Log("score");
        }
    }

    IEnumerator FlashScoreText()
    {
        TMP_Text text = percentText;

        Color originalColor = text.color;
        Color flashColor = Color.yellow; 

        text.color = flashColor;

        // 形变效果
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
                vertices[i] = originalVertices[i] + offset * (1 - t); // 渐变回原始位置
            }

            mesh.vertices = vertices;
            text.canvasRenderer.SetMesh(mesh);

            yield return null;
        }




        // 恢复到原始颜色和形变
        text.color = originalColor;
        text.ForceMeshUpdate();
        mesh.vertices = originalVertices;
        text.canvasRenderer.SetMesh(mesh);



    }


    Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * 330f), Mathf.Cos(time * 250f)) * 25f; // 控制形变的幅度
    }
}

