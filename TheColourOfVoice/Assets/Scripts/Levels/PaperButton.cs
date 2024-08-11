using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PaperButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    //打开选关面板的部分
    [SerializeField] private GameObject chooseLevelPanel;
    [SerializeField] private GameObject paperOutline;
    [SerializeField] private Image paperSpriteImage;

    private void Start()
    {
        //chooseLevelPanel = GameObject.Find("ChooseLevelPanel");
        paperSpriteImage = GetComponent<Image>();
        paperOutline = transform.GetChild(0).gameObject;
        if (chooseLevelPanel == null || paperSpriteImage == null || paperOutline == null)
        {
            return;
        }
        
        chooseLevelPanel.SetActive(false);
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click Paper");
        if(chooseLevelPanel!=null)
            chooseLevelPanel.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Enter");
        paperOutline.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exit");
        paperOutline.SetActive(false);
    }
}
