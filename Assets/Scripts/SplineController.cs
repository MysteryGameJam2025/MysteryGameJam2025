using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UIElements;

public class SplineController : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;
    private LineRenderer LineRenderer => lineRenderer;
    [SerializeField]
    private int resolution;
    private int Resolution => resolution;
    [SerializeField]
    private Transform startPos;
    private Transform StartPos => startPos;
    [SerializeField]
    private Transform endPos;
    private Transform EndPos => endPos;
    [SerializeField]
    private float arcStrength;
    private float ArcStrength => arcStrength;
    [SerializeField]
    private GameObject ring;
    private GameObject Ring => ring;
    [SerializeField]
    private GameObject cap;
    private GameObject Cap => cap;

    private Transform[] Rings { get; set; }
    private Transform[] Caps { get; set; }

    private Spline Spline { get; set; }

    void OnEnable()
    {
        SpawnRings();
        SpawnCaps();
        UpdateValues();
    }

    void SpawnRings()
    {
        Rings = new Transform[3];
        for (int i = 0; i < 3; i++)
        {
            Rings[i] = Instantiate(Ring).transform;
        }
    }

    void SpawnCaps()
    {
        Caps = new Transform[2]
        {
            Instantiate(Cap).transform,
            Instantiate(Cap).transform
        };
    }

    // Update is called once per frame
    void Update()
    {
        UpdateValues();
    }

    [EasyButtons.Button]
    void UpdateValues()
    {
        UpdateSpline();
        UpdateLineRenderer();
        UpdateRings();
        UpdateCaps();
    }


    void UpdateSpline()
    {
        Vector3 midPos = Vector3.Lerp(StartPos.position, EndPos.position, 0.5f);
        float yOffset = ArcStrength * Vector3.Distance(StartPos.position, EndPos.position);
        midPos += new Vector3(0, yOffset, 0);
        float3[] bezierKnots = new float3[3]
        {
            new float3(StartPos.position),
            new float3(midPos),
            new float3(EndPos.position)
        };
        Spline = new Spline(bezierKnots);
    }

    void UpdateLineRenderer()
    {
        LineRenderer.positionCount = Resolution;
        float step = 1f / (Resolution - 1f);
        for (int i = 0; i < Resolution; i++)
        {
            float t = step * i;
            float3 position = Spline.EvaluatePosition(t);
            LineRenderer.SetPosition(i, position);
        }
    }

    void UpdateRings()
    {
        for (int i = 1; i <= 3; i++)
        {
            float t = 0.25f * i;
            Spline.Evaluate(t, out float3 position, out float3 tangent, out float3 upVector);
            Transform ring = Rings[i - 1];
            ring.position = position;
            ring.up = upVector;
            ring.forward = tangent;
        }
    }

    void UpdateCaps()
    {
        Caps[0].position = StartPos.position;
        Caps[1].position = EndPos.position;
    }

    void OnDisable()
    {
        Destroy(Caps[0].gameObject);
        Destroy(Caps[1].gameObject);

        for (int i = 0; i < Rings.Length; i++)
        {
            Destroy(Rings[i].gameObject);
        }
    }
}
