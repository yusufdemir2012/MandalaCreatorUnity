using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Line
{
    private LineRenderer _lineRenderer = null;
    private Vector3[] _points;
    private GameObject _parent = null;

    public Line(GameObject _linesCollectorObject, float _width, Color _color, int _cornerVertices, int endCapVertices, bool _useWorldSpace, Material _material)
    {

        _parent = new GameObject("Line");
        _parent.transform.SetParent(_linesCollectorObject.transform);
        _parent.transform.localPosition = Vector3.zero;
        _parent.transform.localRotation = Quaternion.Euler(Vector3.zero);
        _parent.transform.localScale = Vector3.one;


        this._lineRenderer = _parent.AddComponent<LineRenderer>();
        this._lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        this._lineRenderer.receiveShadows = false;
        this._lineRenderer.positionCount = 0;
        this._lineRenderer.useWorldSpace = _useWorldSpace;
        this._lineRenderer.startWidth = _width;
        this._lineRenderer.endWidth = _width;
        this._lineRenderer.textureMode = LineTextureMode.Stretch;
        this._lineRenderer.startColor = _color;
        this._lineRenderer.endColor = _color;
        this._lineRenderer.material = _material;

        this._lineRenderer.material.color = _color;

    }




    public GameObject GetParent()
    {
        return _parent;
    }
    public int GetPointsCount()
    {
        if (_points == null)
        {
            return 0;
        }
        return _points.Length;
    }
    public void AddPoint(Vector3 _point)
    {
        this.Add(_point);
        this._lineRenderer.positionCount = _points.Length;
        this._lineRenderer.SetPositions(_points);
    }

    private void Add(Vector3 _point)
    {
        var _point0 = new Vector3(_point.x, _point.y, 0f);
        if (_points == null)
            _points = new Vector3[0];
        var _temp = _points;
        var _new = new Vector3[_temp.Length + 1];
        for (int i = 0; i < _new.Length; i++)
        {
            if (i == (_new.Length - 1))
                _new[i] = _point0;
            else
                _new[i] = _temp[i];
        }

        _points = _new;

    }

}