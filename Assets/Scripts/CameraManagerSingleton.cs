using UnityEngine;

public class CameraManagerSingleton : AbstractMonoBehaviourSingleton<CameraManagerSingleton>
{
    public Camera ActiveCamera { get; private set; }

    public void SetActiveCamera(Camera camera)
    {
        if (IsActiveCamera(camera))
        {
            return;
        }
        if (ActiveCamera != null)
        {
            ActiveCamera.enabled = false;
            PlayerController.Instance.DelayControls();
        }
        ActiveCamera = camera;
        ActiveCamera.enabled = true;
        Debug.Log($"Switched to camera: {ActiveCamera.transform.parent.gameObject.name}");
    }

    public bool IsActiveCamera(Camera camera)
        => ActiveCamera != null && ActiveCamera == camera;
}
