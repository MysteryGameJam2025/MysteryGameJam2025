using FriedSynapse.FlowEnt;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController _instance;
    public static SceneController Instance => _instance;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        FlowEntController.Instance.Stop();
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int sceneIndex)
    {
        FlowEntController.Instance.Stop();
        SceneManager.LoadScene(sceneIndex);
    }
}
