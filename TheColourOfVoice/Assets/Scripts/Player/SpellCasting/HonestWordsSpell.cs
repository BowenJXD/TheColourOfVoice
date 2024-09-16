using UnityEngine;

public class HonestWordsSpell : Spell
{
    public PaintColor paintColor;
    public float interval;
    public float damage;
    public Buff buff;

    public override void SetUp()
    {
        base.SetUp();
        if (!buff) buff = GetComponentInChildren<Buff>(true);
        if (buff) buff.Init();
    }

    public override void Execute()
    {
        base.Execute();
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (!SplashGrid.Instance.TryGetCellIndex(mousePos, out var tilePos))
            return;
        
        TextPainter.Instance.PaintText(triggerWords, tilePos, paintColor, interval, ProcessPaintedTile);
    }
    
    void ProcessPaintedTile(SplashTile tile)
    {
        tile.OnPainted += OnPaint;
    }

    private bool OnPaint(Painter painter)
    {
        if (painter.CompareTag("Enemy"))
        {
            if (painter.TryGetComponent(out Health health))
            {
                health.AlterHealth(-damage);
            }

            if (painter.TryGetComponent(out BuffOwner buffOwner))
            {
                buffOwner.ApplyBuff(buff);
            }
        }

        return true;
    }
}