using UnityEngine;

public static class RectExtensions
{
    public static Rect GetWorldSapceRect(this RectTransform rt)
    {
        var r = rt.rect;
        r.center = rt.TransformPoint(r.center);
        r.size = rt.TransformVector(r.size);
        return r;
    }
}
