public class DeinitExe : ExecutableBehaviour
{
    public float waitTime = 0f;
    Entity entity;

    public override void SetUp()
    {
        base.SetUp();
        entity = GetComponent<Entity>();
    }

    protected override void OnStart()
    {
        base.OnStart();
        if (waitTime > 0)
        {
            UnNext();
            new LoopTask
            {
                interval = waitTime, 
                finishAction = () =>
                {
                    entity.Deinit();
                    Next();
                }
            }.Start();
        }
        else
        {
            entity.Deinit();
        }
    }
}