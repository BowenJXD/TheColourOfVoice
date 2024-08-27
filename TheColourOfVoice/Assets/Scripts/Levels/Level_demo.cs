using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    private void Awake()
    {
        index = SceneManager.GetActiveScene().buildIndex;
    }

    void Start()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponentInChildren<Fire>().enabled = false;
        Time.timeScale = 0;
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

        
       

        if(index== 1)
        {
            if (selectedSpell[1] != null)
            {
                selectedSpell[1].SetActive(false);
            }
            if (selectedSpell[2] != null)
            {
                selectedSpell[2].SetActive(false);
            }
        }
        if(index== 2)
        {
            if (selectedSpell[1] != null)
            {
                selectedSpell[1].SetActive(true);
            }
            if (selectedSpell[2] != null)
            {
                selectedSpell[2].SetActive(false);
            }
        }
        if(index== 3)
        {
            if (selectedSpell[1] != null)
            {
                selectedSpell[1].SetActive(true);
            }
            if (selectedSpell[2] != null)
            {
                selectedSpell[2].SetActive(true);
            }
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
        while (totalTime >= 0)
        {
            int minutes = totalTime / 60;
            int seconds = totalTime % 60;
            text.GetComponent<TextMeshProUGUI>().text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
            yield return new WaitForSeconds(1);
            totalTime--;
        }

        Debug.Log("Time's up!");
        Time.timeScale = 0;
        LogUtil.Instance.LogCSV();
        if (totalTime <= 0)
        {
            EndLevel();
            //ShowEndLevelUI();
            
        }

    }

    void EndLevel()
    {
        VolumeControlOrthoSize.Instance.enabled = false;
        var cam = FindObjectOfType<CinemachineVirtualCamera>();
        // Get the current orthographic size of the camera
        float currentOrthoSize = cam.m_Lens.OrthographicSize;

        // Use DOTween to smoothly transition to the target orthographic size
        DOTween.To(() => currentOrthoSize, x => cam.m_Lens.OrthographicSize = x, targetOrthoSize, zoomDuration)
            .SetEase(zoomEase)
            .SetUpdate(true)
            .OnComplete(() => ShowEndLevelUI());
    }
    
    void ShowEndLevelUI()
    {
        StarController.GetComponent<StarController>().UpdateStars();
        if (playerUI != null)
        {
            playerUI.SetActive(false); 
        }

        if (endLevelUI != null)
        {
            endLevelUI.SetActive(true); 
            LeanTween.moveLocal(endLevelUI, new Vector3(0f, -20f, 0f), 1f).setIgnoreTimeScale(true).setEase(LeanTweenType.easeInOutBack).setOnComplete(() =>
            {
                GameObject[] stars = StarController.GetComponent<StarController>().stars;

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
                        });

                    /*LeanTween.rotate(stars[i].gameObject, new Vector3(0f, 0f, 360f), 1f)
                        .setEase(LeanTweenType.linear)
                        .setDelay(0.05f * i)
                        .setLoopClamp() 
                        .setIgnoreTimeScale(true)
                        ;*/
                }
            });
        }

    }
    
    public void ResetTimer()
    {
        totalTime = 180; 
        Time.timeScale = 1; 
    }
}