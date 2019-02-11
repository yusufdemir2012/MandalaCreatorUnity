using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.EventSystems;

public class MandalaCreator : MonoBehaviour 
{

  

    public LineProperties lineProperties;
    public int MarkCount = 18;
    public Camera MainCamera;

    private Vector3 _lastMousePos = new Vector3(0, 0, -2f);
    private GameObject _linesCollectorObject = null;
    public List<Line> _lines = new List<Line>();
    public List<Mark> _marks = new List<Mark>();
    
    private void Start () 
	{
        Mark.MarkParent = null;
        CreateLineCollectorObject();

        float _deltaTheta = (2f * Mathf.PI) / MarkCount;
        float _theta = 0f;
        float _radius = 20f;
        
        for (int i = 0; i < MarkCount; i++)
        {
            _marks.Add(new Mark(
               _linesCollectorObject,
               lineProperties.width,
               lineProperties.color,
               lineProperties.cornerVertices,
               lineProperties.endCapVertices,
               lineProperties.useWorldSpace,
               lineProperties.material));

            _marks[i].AddPoint(Vector3.zero);
            Vector3 _pos = new Vector3(_radius * Mathf.Cos(_theta), _radius * Mathf.Sin(_theta),0f);
            _marks[i].AddPoint(_pos);
            _theta += _deltaTheta;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {//Begin
            _lastMousePos = Input.mousePosition;
            CreateNewLineMandalaLineGroup();
            
        }

        if (Input.GetKey(KeyCode.Mouse0) && Input.mousePosition != _lastMousePos && !EventSystem.current.IsPointerOverGameObject())
        {//Drag
            _lastMousePos = Input.mousePosition;
            AddPointToLastMandalaLineGroup(MainCamera.ScreenToWorldPoint(Input.mousePosition));

        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {//Overed
            RemoveNullMandalaLineGroups();
            _lastMousePos = new Vector3(0, 0, -2f);
        }
    }

    
    public void ResetButton()
    {
        _lastMousePos = new Vector3(0, 0, -2f);
        for (int i = 0; i < _lines.Count; i++)
        {
            Destroy(_lines[i].GetParent());
        }
        _lines.Clear();
    }


    private void CreateNewLineMandalaLineGroup()
    {
        for (int i = 0; i < MarkCount; i++)
        {
            _lines.Add(new Line(
                       _linesCollectorObject,
                       lineProperties.width,
                       lineProperties.color,
                       lineProperties.cornerVertices,
                       lineProperties.endCapVertices,
                       lineProperties.useWorldSpace,
                       lineProperties.material));

        }

       
    }


    private void CreateLineCollectorObject()
    {
        _linesCollectorObject = new GameObject("Lines");
        _linesCollectorObject.transform.position = Vector3.zero;
    }
    private void RemoveNullMandalaLineGroups()
    {
        if (_lines.Count > 0)
        {
            var _targets = _lines.Where(x => x == null || x.GetPointsCount() < 2).ToList();

            for (int i = 0; i < _targets.Count; i++)
            {
                Destroy(_lines.FirstOrDefault(x => x == _targets[i]).GetParent());
            }

            _lines = _lines.Where(x => x != null && x.GetParent() != null).ToList();
        }


    }
    private void AddPointToLastMandalaLineGroup(Vector3 _point)
    {
        if (_lines.Count >= MarkCount)
        {
            int _markCount = MarkCount;
            var _targets = _lines.Skip(Math.Max(0, _lines.Count() - _markCount)).ToList();
            for (int i = 0; i < _markCount; i++)
            {

                Vector3 _angle = new Vector3(0f, 0f, (360f / (float)MarkCount) * (float)(i + 1f));
                _targets[i].AddPoint(RotatePointAroundPivot(_point, Vector3.zero, _angle));
            }

        }
    }
    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }
    private Vector3 GetSymmetry(Vector3 sourcePoint, Vector3 targetPoint)
    {
        return RotatePointAroundPivot(sourcePoint, targetPoint, new Vector3(0f, 0f, 180f));
    }
}