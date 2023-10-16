using System.Collections.Generic;
using UnityEngine;

public class EdgeColliderShapePresets : MonoBehaviour
{
    private EdgeCollider2D _ec;
    private List<Vector2> _pointList = new List<Vector2>();

    private enum ShapeState
    {
        Triangle,
        Square,
        Pentagon,
        Hexagon,
        Octagon,
        Circle
    }

    [SerializeField] private bool _locked = false;
    [SerializeField] private ShapeState _shapeState = new ShapeState();
    [SerializeField] private float _xScale = 1f;
    [SerializeField] private float _yScale = 1f;
    [SerializeField] [Range(0,360)] private float _angleOffset = 1f;

    private int _circlePoints = 32;

    private void Awake()
    {
        _ec = GetComponent<EdgeCollider2D>();
    }
    
#if UNITY_EDITOR
    private void OnValidate()
    {
        if(!_locked)
        {
            _ec = GetComponent<EdgeCollider2D>();
            _pointList.Clear();

            switch (_shapeState)
            {
                case ShapeState.Triangle:
                {
                    _circlePoints = 3;
                    SetGeometry();
                    return;
                }
                case ShapeState.Square:
                {
                    _circlePoints = 4;
                    SetGeometry();
                    return;
                }
                case ShapeState.Pentagon:
                {
                    _circlePoints = 5;
                    SetGeometry();
                    return;
                }
                case ShapeState.Hexagon:
                {
                    _circlePoints = 6;
                    SetGeometry();
                    return;
                }
                case ShapeState.Octagon:
                {
                    _circlePoints = 8;
                    SetGeometry();
                    return;
                }
                case ShapeState.Circle:
                {
                    _circlePoints = 32;
                    SetGeometry();
                    return;
                }
            }
        }
    }

    private void SetGeometry()
    {
        // Calculate angle increment
        float angleIncrement = 2 * Mathf.PI / _circlePoints;
        float angleOffset = 0;

        // Even amount of circle points, have to divide the amount by 2 to get the correct angle
        if (_circlePoints % 2 == 0)
        {
            angleOffset = 90f / (_circlePoints / 2f);
        }
        // Had to do this because the triangle was upside down and I'm too tired of doing math to figure this out
        // through an equation that'll fit everything
        else if (_circlePoints == 3)
        {
            angleOffset = -90f / _circlePoints;
        }
        // Odd amount of circle points, Act as normal
        else
        {
            angleOffset = 90f / _circlePoints;
        }

        for (int i = 0; i < _circlePoints; i++)
        {
            // Apply the offset in radians
            float angle = i * angleIncrement + Mathf.Deg2Rad * (angleOffset - _angleOffset);
            
            // Apply Scale to the coordinates and divide by sqrt(2) to match unity units in scale
            float x = Mathf.Cos(angle) * _xScale / Mathf.Sqrt(2);
            float y = Mathf.Sin(angle) * _yScale / Mathf.Sqrt(2);
            
            _pointList.Add(new Vector2(x, y));
        }

        _pointList.Add(_pointList[0]);
        _ec.SetPoints(_pointList);
    }
#endif
}
