using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : MonoBehaviour,
    IPointerDownHandler,
    IDragHandler,
    IPointerUpHandler
{
    [Header("Joystick UI")]
    [SerializeField] private RectTransform bg;
    [SerializeField] private RectTransform knob;

    private Vector2 inputVector;

    public Vector2 InputVector => inputVector;

    public float Horizontal => inputVector.x;
    public float Vertical => inputVector.y;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            bg,
            eventData.position,
            eventData.pressEventCamera,
            out position
        );

        position.x = position.x / (bg.sizeDelta.x / 2);
        position.y = position.y / (bg.sizeDelta.y / 2);

        inputVector = new Vector2(position.x, position.y);

        inputVector = Vector2.ClampMagnitude(inputVector, 1f);

        knob.anchoredPosition = new Vector2(
            inputVector.x * (bg.sizeDelta.x / 2),
            inputVector.y * (bg.sizeDelta.y / 2)
        );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        knob.anchoredPosition = Vector2.zero;
    }
}