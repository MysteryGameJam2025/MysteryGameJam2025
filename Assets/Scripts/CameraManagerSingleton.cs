using UnityEngine;

public class CameraManagerSingleton : AbstractMonoBehaviourSingleton<CameraManagerSingleton>
{
    [SerializeField]
    private AudioListener audioListenerController;
    private AudioListener AudioListenerController => audioListenerController;

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

        AudioListenerController.transform.SetParent(ActiveCamera.transform, false);
        AudioListenerController.transform.position = camera.transform.position;
    }

    public bool IsActiveCamera(Camera camera)
        => ActiveCamera != null && ActiveCamera == camera;
}
