using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftButton : BaseButton
{
    protected override void OnPress()
    {
        PlayerController.Instance.StartMovingLeft();
    }

    protected override void OnRelease()
    {
        PlayerController.Instance.StopMovingLeft();
    }
}
