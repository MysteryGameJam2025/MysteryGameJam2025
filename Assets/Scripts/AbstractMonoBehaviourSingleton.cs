using UnityEngine;

public abstract class AbstractMonoBehaviourSingleton<TSingleton> : MonoBehaviour
    where TSingleton : AbstractMonoBehaviourSingleton<TSingleton>
{
    private static TSingleton instance;
    public static TSingleton Instance
    {
        get
        {
            return instance ??= GameObject.FindObjectOfType<TSingleton>();
        }
    }

    protected void OnDestroy()
    {
        instance = null;
    }
}