using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsBar : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] Image fillImageBack;
    [SerializeField] Image fillImageFront;
    [SerializeField] bool delayFill = true;
    [SerializeField] float fillDelay = 0.5f;
    [SerializeField] float fillspeed = 0.1f;
    float currentFillAmount;
    protected float targetFillAmount;
    float t;
    WaitForSeconds waitForDelayFill;
    float maxValue;
    float currentValue;
    Coroutine bufferedFillingCoroutine;

    Canvas canvas;
    // Start is called before the first frame update
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;

        waitForDelayFill = new WaitForSeconds(fillDelay);
        Initialize(player);
    }

    public void Initialize(GameObject player)
    {
        currentValue=player.GetComponent<Health>().GetCurrentHealth();
        //Debug.Log("InicurrentValue"+currentValue);

        maxValue = player.GetComponent<Health>().GetMaxHealth();
        //Debug.Log("InimaxtValue"+ maxValue);

        currentFillAmount = currentValue/maxValue;
        targetFillAmount = currentFillAmount;
        fillImageBack.fillAmount = currentFillAmount;
        fillImageFront.fillAmount = currentFillAmount;

    }


    void Update()
    {
        UpdateStats(player);
    }
    public void UpdateStats(GameObject player)
    {
        currentValue = player.GetComponent<Health>().GetCurrentHealth();
        //Debug.Log("UpcurrentValue" + currentValue);

        maxValue = player.GetComponent<Health>().GetMaxHealth();
        //Debug.Log("UpmaxtValue" + maxValue);

        targetFillAmount = currentValue/maxValue;

        if(bufferedFillingCoroutine != null)
        {
            StopCoroutine(bufferedFillingCoroutine);
        }

        if (currentFillAmount > targetFillAmount)
        {
            fillImageFront.fillAmount = targetFillAmount;

            bufferedFillingCoroutine = StartCoroutine(BufferedFillingCoroutine(fillImageBack));
        }

        if(currentFillAmount < targetFillAmount)
        {
            fillImageBack.fillAmount = targetFillAmount;
            bufferedFillingCoroutine =  StartCoroutine(BufferedFillingCoroutine(fillImageBack));
        }
    }


    IEnumerator BufferedFillingCoroutine(Image image)
    {
        if (delayFill)
        {
            yield return waitForDelayFill;
        }
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * fillspeed;
            currentFillAmount = Mathf.Lerp(currentFillAmount, targetFillAmount, t);
            image.fillAmount = currentFillAmount;
            yield return null;
        }
    }
}
