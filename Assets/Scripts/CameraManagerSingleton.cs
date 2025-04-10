using UnityEngine;

public class CameraManagerSingleton : AbstractMonoBehaviourSingleton<CameraManagerSingleton>
{
    public Camera ActiveCamera { get; private set; }

    void Awake()
    {
        ActiveCamera = Camera.main;
    }

    public void SetActiveCamera(Camera camera)
    {
        if (IsActiveCamera(camera))
        {
            return;
        }
        if (ActiveCamera != null)
        {
            ActiveCamera.enabled = false;
        }
        ActiveCamera = camera;
        ActiveCamera.enabled = true;
    }

    public bool IsActiveCamera(Camera camera)
        => ActiveCamera != null && ActiveCamera == camera;
}
