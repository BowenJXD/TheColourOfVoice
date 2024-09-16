using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossBanner : Singleton<BossBanner>
{
    public GameObject bannerPanel;
    public Image backgroundImage;
    public Image bossImage; 
    public TextMeshProUGUI bossText;

    public AnimationCurve easeInBack;
    public AnimationCurve easeOutBack;
    private RectTransform bannerRect; 

    private void Start()
    {

        bannerPanel.SetActive(false);
        bannerRect = bannerPanel.GetComponent<RectTransform>();
        
        bannerRect.localScale = new Vector3(0, 1, 1);
    }

    public void Update()
    {

        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!bannerPanel.activeSelf)
            {
                ShowBanner(null,"PigPig Rush" , Color.red);
            }
        }*/
    }


    public void ShowBanner(Sprite bossSprite, string bossSkillName, Color bgColor, bool pauseTime = false)
    {
        if (pauseTime) Time.timeScale = 0;
        
        if(bossSprite!=null)
            bossImage.sprite = bossSprite;  
        if(bossText!=null)
            bossText.text = bossSkillName;
        backgroundImage.color = bgColor;  
        


        bannerPanel.SetActive(true);


        bannerRect.localScale = new Vector3(0, 1, 1);
        bannerRect.DOScale(new Vector3(1, 1, 1), 0.5f)
            .SetEase(Ease.OutBack)
            .OnComplete(OnBannerDisplayed).SetUpdate(true);

    }


    private void OnBannerDisplayed()
    {
        // hide banner after 2 seconds
        DOTween.Sequence()
            .AppendInterval(2f)  
            .AppendCallback(HideBanner).SetUpdate(true);
    }


    public void HideBanner()
    {

        bannerRect.DOScale(new Vector3(1, 0, 1), 0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(OnBannerHidden).SetUpdate(true);
    }


    private void OnBannerHidden()
    {
        bannerPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
