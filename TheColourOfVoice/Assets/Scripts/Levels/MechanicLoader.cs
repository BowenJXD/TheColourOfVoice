using UnityEngine;

public class MechanicLoader : Singleton<MechanicLoader>, ISetUp
{
    public LevelMechanic mechanic;


    public bool IsSet { get; set; }
    public void SetUp()
    {
        IsSet = true;
    }
    
    private void OnEnable()
    {
        if (!IsSet) SetUp();
        mechanic.Init();
    }
    
    private void OnDisable()
    {
        mechanic.Deinit();
    }
}