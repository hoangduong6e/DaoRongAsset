using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonEvents : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent OnClick;    // Sự kiện OnClick có thể thêm từ Inspector
    public UnityEvent OnHold;     // Sự kiện OnHold có thể thêm từ Inspector
    public UnityEvent OnRelease;  // Sự kiện OnRelease khi thả nút, có thể thêm từ Inspector

    public float holdTime = 0.5f; // Thời gian để xác định giữ nút
    private bool isHolding = false;
    private float holdTimer = 0f;

    public Image buttonImage;     // Tham chiếu đến Image của button
    public Color normalColor = Color.white;  // Màu bình thường
    public Color pressedColor = Color.gray;  // Màu khi nhấn xuống

    void Start()
    {
        // Đảm bảo màu khởi đầu là màu bình thường
        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }
        else if(GetComponent<Image>())
        {
            buttonImage = GetComponent<Image>();
            buttonImage.color = normalColor;
        }   
    }

    void Update()
    {
        // Kiểm tra nếu nút đang được giữ
        if (isHolding)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= holdTime)
            {
                OnHold?.Invoke(); // Gọi sự kiện OnHold từ Inspector
                isHolding = false; // Ngăn không cho sự kiện giữ nút lặp lại
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Khi người dùng bắt đầu giữ nút, đổi màu sang pressedColor
        if (buttonImage != null)
        {
            buttonImage.color = pressedColor;
        }

        isHolding = true;
        holdTimer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Khi người dùng thả nút, trả lại màu bình thường
        if (buttonImage != null)
        {
            buttonImage.color = normalColor;
        }

        // Nếu nhấn nút nhanh hơn thời gian giữ, sẽ gọi sự kiện nhấn
        if (holdTimer < holdTime)
        {
            OnClick?.Invoke(); // Gọi sự kiện OnClick từ Inspector
        }

        // Gọi sự kiện thả nút OnRelease
        OnRelease?.Invoke(); // Gọi sự kiện OnRelease từ Inspector

        isHolding = false;
        holdTimer = 0f;
    }
}
