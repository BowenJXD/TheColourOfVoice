using UnityEngine;

public enum ELayer
{
    Null,
    Default,
    UI,
    Instance,
    Player,
    Bullet,
    Enemy,
    Tile,
    Bound,
}

public static class LayerUtil
{
    public static void IncludeLayer(this Rigidbody2D rb, ELayer layer)
    {
        int layerMask = 1 << LayerMask.NameToLayer(layer.ToString());
        rb.includeLayers |= layerMask;
        rb.excludeLayers &= ~(layerMask);
    }
    
    public static void IncludeAllLayers(this Rigidbody2D rb, ELayer exception = default)
    {
        rb.includeLayers = ~0;
        rb.excludeLayers = 0;
        if (exception != default) rb.ExcludeLayer(exception);
    }
        
    public static void ExcludeLayer(this Rigidbody2D rb, ELayer layer)
    {
        int layerMask = 1 << LayerMask.NameToLayer(layer.ToString());
        rb.excludeLayers |= layerMask;
        rb.includeLayers &= ~(layerMask);
    }
    
    public static void ExcludeAllLayers(this Rigidbody2D rb, ELayer exception = default)
    {
        rb.includeLayers = 0;
        rb.excludeLayers = ~0;
        if (exception != default) rb.IncludeLayer(exception);
    }
        
    public static void IncludeLayer(this Collider2D col, ELayer layer)
    {
        int layerMask = 1 << LayerMask.NameToLayer(layer.ToString());
        col.includeLayers |= layerMask;
        col.excludeLayers &= ~(layerMask);
    }
    
    public static void IncludeAllLayers(this Collider2D col, ELayer exception = default)
    {
        col.includeLayers = ~0;
        col.excludeLayers = 0;
        if (exception != default) col.ExcludeLayer(exception);
    }

    public static void ExcludeLayer(this Collider2D col, ELayer layer)
    {
        int layerMask = 1 << LayerMask.NameToLayer(layer.ToString());
        col.excludeLayers |= layerMask;
        col.includeLayers &= ~(layerMask);
    }
    
    public static void ExcludeAllLayers(this Collider2D col, ELayer exception = default)
    {
        col.includeLayers = 0;
        col.excludeLayers = ~0;
        if (exception != default) col.IncludeLayer(exception);
    }
}