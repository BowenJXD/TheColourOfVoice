using System.Collections.Generic;
using System.Linq;
using FMODUnity;

public class FMODEventEmitExe : ExecutableBehaviour
{
    public List<StudioEventEmitter> emitters;

    public override void SetUp()
    {
        base.SetUp();
        if (emitters == null || emitters.Count == 0)
        {
            emitters = GetComponents<StudioEventEmitter>().ToList();
        }
    }

    protected override void OnStart()
    {
        base.OnStart();
        foreach (var emitter in emitters)
        {
            emitter.Play();
        }
    }
}