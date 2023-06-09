using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]protected Button button;

    protected virtual void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPress();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnRelease();
    }
    protected abstract void OnPress();
    protected abstract void OnRelease();
}
