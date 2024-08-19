using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public TMP_Text text;

    public static string[] tutorialTexts = new[]
    {
        "Use WASD to move.",
        "The more you paint, the faster the score grows. \nTry to get a score of 100.",
        "Try to say %s to the microphone.",
        "Click to release the energy ball.",
        "Enemies are disturbing, but they are optional. \nTry get a score of 1000."
    };
    
    private int index;
    
    public ScoreBar scoreBar;

    public Enemy enemyPrefab;
    
    private void Start()
    {
        index = 0;
        text.text = tutorialTexts[index];
        scoreBar.maxScore = 0;
    }

    private void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        if (index == 0 && (inputX != 0 || inputY != 0))
        {
            IncrementIndex();
            scoreBar.maxScore = 100;
        }
        if (index == 1 && scoreBar.score >= 100)
        {
            SpellManager.Instance.LearnSpell(1);
            tutorialTexts[index+1] = tutorialTexts[index+1].Replace("%s", SpellManager.Instance.allSpells[0].triggerWords);
            IncrementIndex();
            SpellManager.Instance.onCastStateChange += OnCastStateChange;
        }
        if (index == 4 && scoreBar.score >= 1000)
        {
            EndTutorial();
        }
    }

    // index 2
    void OnCastStateChange(CastState state)
    {
        IncrementIndex();
        SpellManager.Instance.onCastStateChange -= OnCastStateChange;
        SpellManager.Instance.onRelease += OnRelease;
    }
    
    // index 3
    void OnRelease()
    {
        IncrementIndex();
        SpellManager.Instance.onRelease -= OnRelease;
        Enemy enemy = PoolManager.Instance.New(enemyPrefab);
        enemy.transform.position = new Vector3(0, 0, 0);
        scoreBar.maxScore = 1000;
    }
    
    void IncrementIndex()
    {
        index++;
        text.text = tutorialTexts[index];
    }
    
    void EndTutorial()
    {
        Debug.Log("Tutorial ends.");
        SceneManager.LoadSceneAsync("MainGame");
    }
}