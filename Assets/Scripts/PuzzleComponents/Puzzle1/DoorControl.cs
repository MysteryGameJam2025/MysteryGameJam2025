using FriedSynapse.FlowEnt;
using UnityEngine;

public class DoorControl : SymbolActivatableBase
{
    [SerializeField]
    private Transform leftDoorPivot;
    private Transform LeftDoorPivot => leftDoorPivot;
    [SerializeField]
    private Transform rightDoorPivot;
    private Transform RightDoorPivot => rightDoorPivot;

    private AbstractAnimation DoorOpenAnimation { get; set; }

    public void Open()
    {
        DoorOpenAnimation?.Stop();
        DoorOpenAnimation = new Flow()
            .Queue(new Tween(2)
                .For(LeftDoorPivot)
                .RotateLocalYTo(-85f)
                .SetEasing(Easing.EaseInOutQuad))
            .At(0, new Tween(2)
                .For(RightDoorPivot)
                .RotateLocalYTo(85f)
                .SetEasing(Easing.EaseInOutQuad))
            .Start();
    }
}
