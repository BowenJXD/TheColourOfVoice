using UnityEngine;

public class PaperButton : MonoBehaviour
{
    //打开选关面板的部分
    [SerializeField] private GameObject chooseLevelPanel;
    [SerializeField] private GameObject paperOutline;
    [SerializeField] private SpriteRenderer paperSpriteImage;
    private void Start()
    {
        //chooseLevelPanel = GameObject.Find("ChooseLevelPanel");
        paperSpriteImage = GetComponent<SpriteRenderer>();
        paperOutline = transform.GetChild(0).gameObject;
        if (chooseLevelPanel == null || paperSpriteImage == null || paperOutline == null)
        {
            return;
        }
        
        chooseLevelPanel.SetActive(false);
    }

    private void OnMouseEnter()
    {
        if (!Level_PsyRoom.Instance.playerIsAwake)
        {
            return;
        }
        Debug.Log("Mouse Enter");
        paperOutline.SetActive(true);
    }
    
    private void OnMouseExit()
    {
        if (!Level_PsyRoom.Instance.playerIsAwake)
        {
            return;
        }
        paperOutline.SetActive(false);
    }
    private void OnMouseUp()
    {
        if (!Level_PsyRoom.Instance.playerIsAwake)
        {
            return;
        }
        chooseLevelPanel.SetActive(true);
       
    }
}
