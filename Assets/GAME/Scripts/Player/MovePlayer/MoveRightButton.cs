using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightButton : BaseButton
{
    protected override void OnPress()
    {
        if (PortalManager.Instance._portalSpawn.isCheckingMoveWhenTele)//nếu player đang tele thì ko cho di chuyển cho tới khi chạm tới ground hoặc wallportal
        {
            PlayerController.Instance.StartMovingRight();
        }
    }

    protected override void OnRelease()
    {
        if (PortalManager.Instance._portalSpawn.isCheckingMoveWhenTele)//nếu player đang tele thì ko cho di chuyển cho tới khi chạm tới ground hoặc wallportal
        {
            PlayerController.Instance.StopMovingRight();
        }
    }
}