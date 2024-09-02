public class WaitExe : ExecutableBehaviour
{
    public float waitTime = 1f;
    
    protected override void OnStart()
    {
        base.OnStart();
        StartExe();
        new LoopTask
        {
            interval = waitTime, 
            finishAction = FinishExe
        }.Start();
    }
}