using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour, ICollidable
{
    public float _speedPlayerToWin = 1;

    public void OnCollisionEnter2D(Collision2D collision)
    {
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!UiController.Instance._uiWinGame._popUP_WinGame.activeSelf)
            {
                UiController.Instance._uiWinGame._popUP_WinGame.SetActive(true);
                UiController.Instance._uiWinGame._listButtonMove.ForEach(item => item.SetActive(false));
                PlayerController.Instance._playerComponent._skeletonAnimation.AnimationState.SetAnimation(0, "Run",
                    true);
                float _scaleX = PlayerController.Instance._playerComponent._skeletonAnimation.skeleton.ScaleX;
                PlayerController.Instance._playerComponent._rigidbody.velocity = new Vector2(
                    _scaleX == 1 ? -_speedPlayerToWin : _speedPlayerToWin,
                    PlayerController.Instance._playerComponent._rigidbody.velocity.y);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            #region Vô hiệu hóa velocity ở player

            PlayerController.Instance._playerMove.isMovingRight = false;
            PlayerController.Instance._playerMove.isMovingLeft = false;

            #endregion
            PlayerController.Instance._playerComponent._skeletonAnimation.AnimationState.SetAnimation(0, "Idle_1",
                true);
            PlayerController.Instance._playerComponent._rigidbody.velocity = Vector2.zero;
            PlayerController.Instance.gameObject.GetComponent<PlayerController>().enabled = false;
            StartCoroutine(UiController.Instance.AnimationWinGame());
        }
    }
}