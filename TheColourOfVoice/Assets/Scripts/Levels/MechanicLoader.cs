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
    
    void Update()
    {
        mechanic.OnUpdate();
    }
    
    private void OnDisable()
    {
        mechanic.Deinit();
    }
}