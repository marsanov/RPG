using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private Image joystickBG;
    [SerializeField] private Image joystick;
    private Vector2 inputVector;

    void Update()
    {
        RotateWithCamera();
    }

    private void RotateWithCamera()
    {
        float cameraRotationY = Camera.main.transform.localEulerAngles.y;
        transform.rotation = Quaternion.Euler(0, 0, cameraRotationY);
    }

    public void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector2.zero;
        joystick.rectTransform.anchoredPosition = Vector2.zero;
    }

    public void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBG.rectTransform, ped.position,
            ped.pressEventCamera, out pos))
        {
            pos.x = pos.x / joystickBG.rectTransform.sizeDelta.x;
            pos.y = pos.y / joystickBG.rectTransform.sizeDelta.x;

            inputVector = new Vector2(pos.x, pos.y);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            joystick.rectTransform.anchoredPosition = new Vector2(inputVector.x*(joystickBG.rectTransform.sizeDelta.x / 2), inputVector.y * (joystickBG.rectTransform.sizeDelta.x / 2));

            //чтобы движение не было прерывистым
            inputVector = inputVector.normalized;
        }
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;
        else
            return Input.GetAxis("Horizontal");
    }
    public float Vertical()
    {
        if (inputVector.x != 0)
            return inputVector.y;
        else
            return Input.GetAxis("Vertical");
    }
}
