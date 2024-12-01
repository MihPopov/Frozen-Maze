using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class CameraBounds : MonoBehaviour
{
    public int width = 11;
    public int height = 11;

    void Awake()
    {
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();
        float halfWidth = width / 2f;
        float halfHeight = height / 2f;
        Vector2[] points = new Vector2[]
        {
            new Vector2(-halfWidth, -halfHeight),
            new Vector2(-halfWidth, halfHeight),
            new Vector2(halfWidth, halfHeight),
            new Vector2(halfWidth, -halfHeight),
            new Vector2(-halfWidth, -halfHeight)
        };
        collider.pathCount = 1;
        collider.SetPath(0, points);
    }
}
