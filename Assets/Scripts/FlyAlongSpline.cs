using UnityEngine;
using UnityEngine.Splines;

public class FlyAlongSpline : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private float Speed => speed;
    private Transform self;
    private Transform Self => self ??= transform;

    public Spline Spline { get; set; }
    private float Duration { get; set; }
    private float T { get; set; }

    public void SetTargets(Spline spline)
    {
        T = 0;
        Spline = spline;
        Duration = spline.GetLength() / Speed;
    }

    void Update()
    {
        if (T > 1)
        {
            return;
        }

        Self.position = Spline.EvaluatePosition(T);
        T += Time.deltaTime / Duration;
    }

}