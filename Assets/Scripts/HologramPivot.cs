using UnityEngine;

public class HologramPivot : MonoBehaviour
{

    [SerializeField]
    private float speed;
    private float Speed => speed;

    private Transform self;
    private Transform Self => self ??= transform;

    // Update is called once per frame
    void Update()
    {
        Vector3 euler = Self.rotation.eulerAngles;
        euler.y += Speed * Time.deltaTime;
        Self.rotation = Quaternion.Euler(euler);
    }
}
