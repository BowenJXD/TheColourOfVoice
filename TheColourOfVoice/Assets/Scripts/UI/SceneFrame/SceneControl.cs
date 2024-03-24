/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneControl 
{
    public Dictionary<string, SceneBase> dict_scene;

    private static SceneControl instance;
    public static SceneControl GetInstance()
    {
        if(instance == null)
        {
            Debug.LogError("SceneContro no exit");
            return instance;
        }
        return instance;

    }

    public void LoadScene(string sceneName,SceneBase sceneBase)
    {
        if(!dict_scene.ContainsKey(sceneName))
        {
            dict_scene.Add(sceneName, sceneBase);
        }
        if(dict_scene.ContainKey(SceneManager.GetActiveScene().name)) {
            dict_scene[SceneManager.GetActiveScene().name].ExitScene();
        }
        else
        {
            Debug.LogWarning("not contain in dict");
        }


        SceneManager.LoadScene(sceneName);
        GameRoot.GetInstance().UIManager_Root.Pop(true);
        sceneBase.EnterScene();
    }

}
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl
{
    private static SceneControl instance;
    public static SceneControl GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("SceneControl");
            return instance;
        }

        return instance;
    }
    public int scene_number = 1;
    public string[] string_scene;

    public Dictionary<string, SceneBase> dict_scene;

    public SceneControl()
    {
        instance = this;

        dict_scene = new Dictionary<string, SceneBase>();
        //dict_scene.Add();
    }


    public void SceneLoad(string SceneName, SceneBase sceneBase)
    {
        if (scene_number >= 2)
        {
            foreach (string scenename in string_scene)
            {
                if (scenename == SceneName)
                {
                    Debug.Log($"{SceneName}");
                    break;
                }
                scene_number++;
                string_scene[scene_number] = SceneName;
            }
        }

        if (!dict_scene.ContainsKey(SceneName))
        {
            dict_scene.Add(SceneName, sceneBase);
        }

        if (scene_number >= 2)
        {
            dict_scene[SceneManager.GetActiveScene().name].ExitScene();
        }


        sceneBase.EnterScene();
        GameRoot.GetInstance().UIManager_Root.Pop(true);
        SceneManager.LoadScene(SceneName);

    }

}
