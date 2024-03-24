/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    private UIManager UIManager;
    public UIManager UIManager_Root { get => UIManager; }

    private static GameRoot instance;

    private SceneControl SceneControl;
    public SceneControl SceneControl_Root{get=>SceneControl;}

    public static GameRoot GetInstance()
    {
        if(instance == null)
        {
            Debug.LogError("no gameroot");
            return instance;
        }
        return instance;
    }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destory(this.gameObject);
        }
        instance = this;
        UIManager = new UIManager();
        SceneControl = new SceneControl();
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        UIManager_Root.CanvasObj = UIMethods.GetInstance().FindCanvas();

        UIManager_Root.Push();
    }
}
*/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    private static GameRoot instance;

    private UIManager UIManager;

    public UIManager UIManager_Root;

    private SceneControl SceneControl;
    public SceneControl SceneControl_Root { get => SceneControl; }

    public static GameRoot GetInstance()
    {
        if (instance == null)
        {
            Debug.LogWarning("GameRoot Ins is false!");
            return instance;
        }

        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        UIManager = new UIManager();
        UIManager_Root = UIManager;
        SceneControl = new SceneControl();
    }

/*    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        UIManager_Root.CanvasObj = UIMethods.GetInstance().FindCanvas();



        SceneControl_Root.dict_scene.Add("Scene2", new Scene2());
        UIManager_Root.Push(new Scene2Panel());



    }*/
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        UIManager_Root.CanvasObj = UIMethods.GetInstance().FindCanvas();

        UIManager_Root.Push(new StartPanel());
    }
    // Update is called once per frame
    void Update()
    {

    }
}
