using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AnnouncementManager : MonoBehaviour
{
    public RectTransform textContainer; // RectTransform của Text container (chứa văn bản)
    public RectTransform parentContainer; // RectTransform của khung chứa

    public float scrollSpeed = 100f; // Tốc độ chạy của chữ

    private float endX = 0;

    Queue<string> alltb = new Queue<string>();
    public void ShowAnnouncement(string message,bool setluon = false)
    {
        if(parentContainer.gameObject.activeSelf && !setluon)
        {
            alltb.Enqueue(message);
            return;
        }
        // Kích hoạt parentContainer trước khi hiển thị thông báo
        parentContainer.gameObject.SetActive(true);

        StopAllCoroutines(); // Dừng thông báo cũ nếu đang chạy
        Text announcementText = textContainer.GetComponent<Text>();
        announcementText.text = message;

        // Cập nhật kích thước textContainer sau khi thay đổi văn bản
        LayoutRebuilder.ForceRebuildLayoutImmediate(textContainer);

        // Tính toán vị trí bắt đầu của textContainer
        float startX = parentContainer.rect.width;
        textContainer.anchoredPosition = new Vector2(startX + 20, textContainer.anchoredPosition.y);

        endX = textContainer.rect.width / -3.3f;
        // Bắt đầu hiệu ứng chữ chạy
        StartCoroutine(ScrollText());
    }

    private IEnumerator ScrollText()
    {
        // Vị trí kết thúc: Khi cạnh phải của textContainer vừa chạm đến cạnh trái của parentContainer

        while (textContainer.anchoredPosition.x > endX)
        {
            // Di chuyển textContainer qua trái
            textContainer.anchoredPosition -= new Vector2(scrollSpeed * Time.deltaTime, 0);
            yield return null;
        }

        // Khi kết thúc, ẩn parentContainer sau khi thông báo đã di chuyển xong
        if(alltb.Count > 0)
        {
            ShowAnnouncement(alltb.Dequeue(),true);
        }
        else parentContainer.gameObject.SetActive(false);

    }
}
