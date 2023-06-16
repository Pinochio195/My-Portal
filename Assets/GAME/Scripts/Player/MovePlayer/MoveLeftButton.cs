using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftButton : BaseButton
{
    protected override void OnPress()
    {
        Debug.Log(123);
        if (PortalManager.Instance._portalSpawn.isCheckingMoveWhenTele)//nếu player đang tele thì ko cho di chuyển cho tới khi chạm tới ground hoặc wallportal
        {
            PlayerController.Instance.StartMovingLeft();
        }
    }

    protected override void OnRelease()
    {
        if (PortalManager.Instance._portalSpawn.isCheckingMoveWhenTele)//nếu player đang tele thì ko cho di chuyển cho tới khi chạm tới ground hoặc wallportal
        {
            PlayerController.Instance.StopMovingLeft();
        }
    }
}