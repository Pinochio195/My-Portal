using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightButton : BaseButton
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnPress()
    {
        PlayerController.Instance.StartMovingRight();
    }

    protected override void OnRelease()
    {
        PlayerController.Instance.StopMovingRight();
    }
}
