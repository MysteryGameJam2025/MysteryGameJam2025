using UnityEngine;

public class CameraSwitchZone : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    private Camera Camera => cam;
    void OnTriggerEnter(Collider other)
    {
        CameraManagerSingleton.Instance.SetActiveCamera(Camera);
    }
}
