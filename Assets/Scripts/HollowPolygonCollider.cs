using UnityEngine;

public class SetGeometry : MonoBehaviour
{
    [SerializeField] private float _thickness = 0.01f;

    void Start()
    {
        var collider = GetComponent<PolygonCollider2D>();

        // Get the vertices of the outer shape
        Vector2[] outerPath = collider.GetPath(0);

        // Compute the inner path by adding a small offset (dist) to the outer vertices
        Vector2[] innerPath = new Vector2[outerPath.Length];
        for (int i = 0; i < outerPath.Length; i++)
        {
            innerPath[i] = outerPath[i] + (outerPath[i] - collider.offset).normalized * -_thickness;
        }

        collider.pathCount = 2;

        // Set the outer and inner paths
        collider.SetPath(0, outerPath);
        collider.SetPath(1, innerPath);
    }
}