using System;
using System.Collections;
using System.Collections.Generic;
using Ring;
using UnityEngine;

public class UiController : MonoBehaviour
{
    #region Singleton

    private static UiController instance;

    public static UiController Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    #endregion
    [Space(5)] [HeaderTextColor(0.2f, 1, 1, headerText = "UiWinGame")]
    public UI_WinGame _uiWinGame;

    private void Start()
    {
    }

    public IEnumerator  AnimationWinGame()//hiệu ứng 3 star di chuyển về vị trí 3 star đen
    {
        foreach (GameObject obj in _uiWinGame._listBlackStar)
        {
            // Kiểm tra xem object có thành phần RectTransform hay không
            RectTransform rectTransform = obj.transform.GetChild(0).GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                // Bắt đầu coroutine để di chuyển từ vị trí hiện tại về (0, 0)
                yield return StartCoroutine(MoveObject(rectTransform, Vector2.zero));

                // Chờ một khoảng thời gian trước khi di chuyển object tiếp theo
                //yield return new WaitForSeconds(0f); // Thời gian chờ giữa các object
            }
        }
        _uiWinGame._buttonCONTINUE.SetActive(true);
    }
    private IEnumerator MoveObject(RectTransform obj, Vector2 targetPosition)
    {
        Vector2 startPosition = obj.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < _uiWinGame.moveDurationTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / _uiWinGame.moveDurationTime);
            float curveValue = _uiWinGame.moveCurve.Evaluate(t);

            obj.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, curveValue);

            yield return null;
        }

        obj.anchoredPosition = targetPosition;
    }
}
