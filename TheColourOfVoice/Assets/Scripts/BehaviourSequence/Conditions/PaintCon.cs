public class PaintCon : ConditionBehaviour
{
    private Painter painter;

    public override void SetUp()
    {
        base.SetUp();
        painter = GetComponentInChildren<Painter>(true);
    }
    
    protected override void Init()
    {
        painter.OnPaint += _ => StartCoroutine(Execute());
    }
    
    protected override void Deinit()
    {
        painter.OnPaint -= _ => StartCoroutine(Execute());
    }
}