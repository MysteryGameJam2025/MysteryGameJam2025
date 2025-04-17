using UnityEngine;

public class StairsController : MonoBehaviour
{
    [SerializeField]
    private GameObject realStairs;
    private GameObject RealStairs => realStairs;
    [SerializeField]
    private GameObject hologramStairs;
    private GameObject HologramStairs => hologramStairs;

    public void DestroyRealStairs()
    {
        RealStairs.SetActive(false);
    }

    public void SetHologramStarisEnabled(bool isEnabled)
    {
        HologramStairs.SetActive(isEnabled);
    }
}
