using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileControls : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public Image joystickHolder;
    public Image joystick;
    public Vector3 inVector;

    public virtual void OnPointerDown(PointerEventData pointerEvent)
    {
        OnDrag(pointerEvent);
    }

    public virtual void OnPointerUp(PointerEventData pointerEvent)
    {

    }

    public virtual void OnDrag(PointerEventData pointerEvent)
    {
        Vector2 position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickHolder.rectTransform, 
            pointerEvent.position, 
            pointerEvent.pressEventCamera, 
            out position))
        {
            position.x = (position.x / joystickHolder.rectTransform.sizeDelta.x);
            position.y = (position.y / joystickHolder.rectTransform.sizeDelta.y);

            inVector = new Vector3(position.x * 2 + 1, 0, position.y * 2 - 1);
            inVector = (inVector.magnitude > 1.0f) ? inVector.normalized : inVector;

            joystick.rectTransform.anchoredPosition = new Vector3(
                inVector.x * (joystickHolder.rectTransform.sizeDelta.x / 3), 
                inVector.z * (joystickHolder.rectTransform.sizeDelta.y / 3));
        }
    }
}
