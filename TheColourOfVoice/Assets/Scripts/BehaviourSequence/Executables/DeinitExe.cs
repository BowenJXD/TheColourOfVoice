public class DeinitExe : ExecutableBehaviour
{
    Entity entity;
    
    public override void SetUp()
    {
        base.SetUp();
        entity = GetComponent<Entity>();
    }

    protected override void OnStart()
    {
        base.OnStart();
        entity.Deinit();
    }
}