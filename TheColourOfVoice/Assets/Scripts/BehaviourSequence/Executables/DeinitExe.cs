using Sirenix.OdinInspector;

public class DeinitExe : ExecutableBehaviour
{
    [HideIf("waitForSequenceToFinish")]
    public float waitTime = 0f;
    public bool waitForSequenceToFinish = true;
    Entity entity;

    public override void SetUp()
    {
        base.SetUp();
        entity = GetComponent<Entity>();
    }

    protected override void OnStart()
    {
        base.OnStart();
        if (waitForSequenceToFinish)
        {
            ConditionBehaviour conditionBehaviour = blackboard.Get<ConditionBehaviour>(BBKey.CON);
            conditionBehaviour.OnFinish += () => entity?.Deinit();
        }
        else if (waitTime > 0)
        {
            StartExe();
            new LoopTask
            {
                interval = waitTime, 
                finishAction = () =>
                {
                    entity?.Deinit();
                    FinishExe();
                }
            }.Start();
        }
        else
        {
            entity.Deinit();
        }
    }
}