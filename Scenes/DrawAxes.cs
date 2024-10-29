using UnityEngine;

[ExecuteInEditMode]
public class DrawAxes : MonoBehaviour
{
    public float cellSize = 1f; // Kích thước của các ô trong grid
    public Color gridColor = Color.black; // Màu của grid
    public float lineWidth = 0.05f; // Độ dày của đường kẻ

    void OnDrawGizmos()
    {
        Gizmos.color = gridColor;
        Vector3 position = transform.position;

        // Tính toán số lượng đường kẻ theo kích thước của camera
        float cameraHeight = Camera.main.orthographicSize * 2;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        int numCellsX = Mathf.CeilToInt(cameraWidth / cellSize);
        int numCellsY = Mathf.CeilToInt(cameraHeight / cellSize);

        for (int x = -numCellsX; x <= numCellsX; x++)
        {
            float xPos = position.x + x * cellSize;
            Gizmos.DrawLine(new Vector3(xPos, position.y - cameraHeight / 2, 0), new Vector3(xPos, position.y + cameraHeight / 2, 0));
        }

        for (int y = -numCellsY; y <= numCellsY; y++)
        {
            float yPos = position.y + y * cellSize;
            Gizmos.DrawLine(new Vector3(position.x - cameraWidth / 2, yPos, 0), new Vector3(position.x + cameraWidth / 2, yPos, 0));
        }
    }
}
