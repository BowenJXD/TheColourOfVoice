public class WaitExe : ExecutableBehaviour
{
    public float waitTime = 1f;
    
    protected override void OnStart()
    {
        base.OnStart();
        UnNext();
        new LoopTask
        {
            interval = waitTime, 
            finishAction = Next
        }.Start();
    }
}