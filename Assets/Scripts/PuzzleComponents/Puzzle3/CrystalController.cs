using UnityEngine;

public class CrystalController : MonoBehaviour
{
    [SerializeField]
    private GameObject pickup;
    private GameObject Pickup => pickup;
    [SerializeField]
    private AnimationClip fallClip;
    private AnimationClip FallClip => fallClip;


    private Animator Animator { get; set; }

    public void DropCrystal()
    {
        Animator = GetComponent<Animator>();
        Animator.CrossFade(FallClip.name, 0);
        Pickup.SetActive(true);
    }
}
