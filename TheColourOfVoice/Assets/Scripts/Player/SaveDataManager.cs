using UnityEngine;

public class SaveDataManager : Singleton<SaveDataManager>
{
    public SaveData saveData;

    protected override void Awake()
    {
        base.Awake();
        saveData = Resources.Load<SaveData>(PathDefines.SaveData);
    }
}