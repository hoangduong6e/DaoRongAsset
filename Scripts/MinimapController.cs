
using UnityEngine;
using UnityEngine.EventSystems;

public class MinimapController : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private Camera mainCamera;
    private RectTransform minimapRectTransform;
    private SpriteRenderer mapRenderer;

    private float mapMinX;
    private float mapMaxX;
    private RectTransform mapIndicator;

    void Start()
    {
        // Giả sử bản đồ nằm giữa các giá trị giới hạn X
        mapRenderer = VienChinh.vienchinh.imgMap;
        mapMinX = mapRenderer.bounds.min.x + 10.3f;
        mapMaxX = mapRenderer.bounds.max.x - 10.3f;
     
        minimapRectTransform = GetComponent<RectTransform>();
        mapIndicator = minimapRectTransform.transform.GetChild(0).GetComponent<RectTransform>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        MoveCamera(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        MoveCamera(eventData);
    }

    private void MoveCamera(PointerEventData eventData)
    {
        Vector2 localPoint;
        // Chuyển đổi điểm màn hình sang điểm cục bộ của RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(minimapRectTransform, eventData.position, eventData.pressEventCamera, out localPoint);

        // Tính toán tỷ lệ vị trí X trong minimap
        float normalizedX = Mathf.InverseLerp(minimapRectTransform.rect.xMin, minimapRectTransform.rect.xMax, localPoint.x);

        // Tính toán vị trí X trong thế giới thực từ vị trí tỷ lệ
        float targetX = Mathf.Lerp(mapMinX, mapMaxX, normalizedX);

        // Cập nhật vị trí của camera
        Vector3 cameraPosition = mainCamera.transform.position;
        cameraPosition.x = targetX;
        mainCamera.transform.position = cameraPosition;
        UpdateMapIndicator();
    }
   // public float maxX, minX;
    public void UpdateMapIndicator()
    {
        // Tính toán vị trí tỷ lệ của camera trên trục X
        float normalizedX = Mathf.InverseLerp(mapMinX, mapMaxX, mainCamera.transform.position.x);

        // Tính toán vị trí cục bộ của chỉ báo trên minimap
        Vector2 indicatorPosition = new Vector2(
            Mathf.Lerp(minimapRectTransform.rect.xMin + 131, minimapRectTransform.rect.xMax -131, normalizedX),
            mapIndicator.anchoredPosition.y
        );

        // Cập nhật vị trí của chỉ báo
        mapIndicator.anchoredPosition = indicatorPosition;
    }
    public GameObject mapDotPrefab; // Prefab của chấm đỏ trên minimap
    public Transform InitializeChamDo()
    {
        GameObject mapDot = Instantiate(mapDotPrefab, minimapRectTransform);
        mapDot.transform.SetParent(minimapRectTransform.transform,false);
        return mapDot.transform;
    }
    private float yMin = 600;
    private float yMax = 80;
    private float boundsminy = 40;
    private float boundsmaxy = 0;
    public void UpdateMapDots(Transform tf,Transform chamdo)
    {
        Vector3 objectPosition;
        if (tf.transform.parent.name == "TeamXanh")
        {
            objectPosition = new Vector3(tf.transform.position.x + 1, tf.transform.position.y, tf.transform.position.z);
        }
        else objectPosition = new Vector3(tf.transform.position.x - 1, tf.transform.position.y, tf.transform.position.z);
        float normalizedX = Mathf.InverseLerp(mapMinX - 5, mapMaxX+ 5, objectPosition.x);
        float normalizedY = Mathf.InverseLerp(mapRenderer.bounds.min.y - boundsminy, mapRenderer.bounds.max.y + boundsmaxy, -objectPosition.y);

        // Điều chỉnh vị trí trên minimap
        Vector2 dotPosition = new Vector2(
            Mathf.Lerp(minimapRectTransform.rect.xMin, minimapRectTransform.rect.xMax, normalizedX),
            Mathf.Lerp(minimapRectTransform.rect.yMin + yMin, minimapRectTransform.rect.yMax - yMax, normalizedY)
        );
        chamdo.transform.localPosition = dotPosition;
    }
}
