using UnityEngine;
using UnityEngine.EventSystems;

public class Controls : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
	public static Vector2 Vector = new Vector2(0, 0);
	public RectTransform Joystick, Point, LeftUp;
	public GameObject JoystickObject;

	private bool Touching;
	private int TouchId;

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!Touching)
		{
			Touching = true;
			TouchId = eventData.pointerId;
			JoystickObject.SetActive(true);
			Joystick.position = eventData.position;
			OnDrag(eventData);
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (Touching && eventData.pointerId == TouchId)
		{
			Point.position = (Vector2)Joystick.position + Vector2.ClampMagnitude(eventData.position - (Vector2)Joystick.position, (LeftUp.position - Joystick.position).x);
			Vector = (Vector2)(Joystick.position - Point.position) / (Vector2)(Joystick.position - LeftUp.position);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (Touching && eventData.pointerId == TouchId)
		{
			Touching = false;
			TouchId = -1;
			JoystickObject.SetActive(false);
			Vector = new Vector2(0, 0);
		}
	}
}