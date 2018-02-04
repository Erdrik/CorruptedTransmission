using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Polygon2D {

    public List<Vector2> _points;
    public static string PROPERTY_POINTS = "_points";

    public Polygon2D(Vector2[] points)
    {
        _points.AddRange(points);
    }

    public Polygon2D()
    {
        _points = new List<Vector2>{
            new Vector2(-0.5f, 0),
            new Vector2(0.5f, 0),
            new Vector2(0,1f)
        };
    }

    public void AddPoint(int i)
    {
        int nextPoint = (i + 1 == _points.Count) ? 0 : i + 1;
        Vector2 newPoint = _points[i] + (_points[nextPoint] / 2);
        _points.Insert(i, newPoint);
    }

    public void AddPoint(int i, Vector2 point)
    {
        _points.Insert(i, point);
    }

    public void RemovePoint(int i)
    {
        if (_points.Count > 3)
        {
            _points.RemoveAt(i);
        }
    }

    

    public bool IsInPolygon(Vector2 point)
    {
        Vector3 p1, p2;
        bool inside = false;

        if(_points.Count < 3)
        {
            return inside;
        }

        Vector3 lastPoint = _points[_points.Count-1];
        for (int i = 0; i < _points.Count; i++)
        {
            Vector3 currentPoint = _points[i];
            if(currentPoint.x > lastPoint.x)
            {
                p1 = lastPoint;
                p2 = currentPoint;
            }
            else
            {
                p1 = currentPoint;
                p2 = lastPoint;
            }

            if(currentPoint.x < point.x == point.x <= lastPoint.x)
            {
                if((point.y - p1.y)*(p2.x - p1.x) < (p2.y - p1.y)*(point.x - p1.x))
                {
                    inside = !inside;
                }
            }
            lastPoint = currentPoint;
        }
        return inside;
    }

}
