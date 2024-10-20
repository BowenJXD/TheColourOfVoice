public class ChangeColourExe : ExecutableBehaviour
{
    public PaintColor targetColor;
    
    public Painter painter;

    public override void Init()
    {
        base.Init();
        if (!painter) painter = GetComponent<Painter>();
    }

    protected override void OnStart()
    {
        base.OnStart();
        painter.paintColor = targetColor;
    }
}