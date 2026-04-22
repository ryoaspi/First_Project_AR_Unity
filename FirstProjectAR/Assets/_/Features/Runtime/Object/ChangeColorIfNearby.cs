using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ChangeColorIfNearby : MonoBehaviour
{
    #region Unity API

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _textMeshPro = GetComponentInChildren<TextMeshPro>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        _camera = Camera.main;
        _meshRenderer.sharedMaterial = _defaultMaterial;
        
        _lineRenderer.positionCount = 2;
        _lineRenderer.startWidth = 0.05f;
        _lineRenderer.endWidth = 0.05f;

        _allPoints.Add(this);

        if (_allPoints.Count == 4)
        {
            DrawQuadrilateral();
            CalculateArea();
        }
    }

    private void Update()
    {
        HandleDistance();
        HandleTextRotation();
        HandleVisuals();
        HandleMeasureLine();
    }

    #endregion
    
    
    #region Utils

    public void SetTarget(Transform target)
    {
        _targetObject = target;
    }
    
    #endregion

    #region Main Methods

    private void HandleDistance()
    {
        _distance = Vector3.Distance(_camera.transform.position, transform.position);

        float t = Mathf.InverseLerp(0f, _distanceMaxForScale, _distance);
        t = Mathf.Pow(t, 2f);
        _textMeshPro.fontSize = Mathf.Lerp(_maxFontSize, _minFontSize, t * _speedSizeChanged);

        _textMeshPro.text = _distance.ToString("0.00");
    }

    private void HandleTextRotation()
    {
        Vector3 direction = _textMeshPro.transform.position - _camera.transform.position;
        direction = Vector3.ProjectOnPlane(direction,Vector3.up);
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        // Quaternion rotation = Quaternion.Lerp(_textMeshPro.transform.rotation, targetRotation, Time.deltaTime * _lerp);
        _rotation =
            Quaternion.RotateTowards(_textMeshPro.transform.rotation, 
                targetRotation, _rotationSpeed * Time.deltaTime);
        
    }

    private void HandleVisuals()
    {
        if (_distance <= _colorSwapThreshold)
        {
            if (_meshRenderer.sharedMaterial != _nearbyMaterial)
                _meshRenderer.sharedMaterial = _nearbyMaterial;
            _textMeshPro.transform.rotation = _rotation;
            Tween.Scale(transform, 1.5f, 1);
        }
        else
        {
            if (_meshRenderer.sharedMaterial != _defaultMaterial)
                _meshRenderer.sharedMaterial = _defaultMaterial;
            _textMeshPro.transform.rotation *= Quaternion.Euler(0f, _rotationSpeed*Time.deltaTime,0f );
            Tween.Scale(transform, 1f, 1f, Ease.OutCubic);
        }
    }

    private void HandleMeasureLine()
    {
        _allPoints.RemoveAll(item => item == null);
        
        int myIndex = _allPoints.IndexOf(this);
        if (myIndex == 0)
        {
            _lineRenderer.enabled = false;
            _textMeshPro.enabled = false;
            return;
        }
        
        _lineRenderer.enabled = true;
        _textMeshPro.enabled = true;
        
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1,_allPoints[myIndex - 1].transform.position);

        if (_allPoints.Count == 4 && myIndex == 3)
        {
            _lineRenderer.positionCount = 3;
            _lineRenderer.SetPosition(2, _allPoints[0].transform.position);
            
            CalculateArea();
        }

        else
        {
            float d =Vector3.Distance(transform.position, _allPoints[myIndex - 1 ].transform.position);
            _textMeshPro.text = $"{d:F2}m";
            
            _textMeshPro.transform.position = (transform.position 
                                               + _allPoints[myIndex - 1].transform.position)
                                                / 2f + Vector3.up * 0.05f;
        }

        // float distBetween = Vector3.Distance(transform.position, _targetObject.position);
        //
        // Vector3 middlePoint = (transform.position + _targetObject.position) / 2f;
        //
        // _textMeshPro.transform.position = middlePoint + Vector3.up * 0.05f;
        // _textMeshPro.text = $"{distBetween:F2} m";
    }

    private void DrawQuadrilateral()
    {
        _lineRenderer.positionCount = 5;
        _lineRenderer.loop = true;

        for (int i = 0; i < 4; i++)
        {
            _lineRenderer.SetPosition(i, _allPoints[i].transform.position);
        }
        
        _lineRenderer.SetPosition(4, _allPoints[0].transform.position);
    }

    private void CalculateArea()
    {
        Vector3 a = _allPoints[0].transform.position;
        Vector3 b = _allPoints[1].transform.position;
        Vector3 c = _allPoints[2].transform.position;
        Vector3 d = _allPoints[3].transform.position;
        
        float area1 = Vector3.Cross(b - a, c - a).magnitude * 0.5f;
        float area2 = Vector3.Cross(c - a, d -a).magnitude * 0.5f;
        float totalArea = area1 + area2;

        _textMeshPro.enabled = true;

        Vector3 center = (a + b + c + d) / 4f;
        _textMeshPro.transform.position = center +  Vector3.up * 0.1f;

        _textMeshPro.text = $"Aire: {totalArea:F2} m²";
    }

    private void OnDestroy()
    {
        _allPoints.Remove(this);
    }

    #endregion

    #region Private Fields

    private Camera _camera;
    private MeshRenderer _meshRenderer;
    private TextMeshPro _textMeshPro;
    private LineRenderer _lineRenderer;
    private float _distance;
    private Quaternion _rotation;
    
    [Header("Mesure entre objets")]
    [SerializeField] private Transform _targetObject;

    [Header("Materials")]
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _nearbyMaterial;

    [Header("Distance Settings")]
    [SerializeField] private float _colorSwapThreshold = 1.5f;
    [SerializeField] private float _distanceMaxForScale = 5f;

    [Header("Text Settings")]
    [SerializeField] private float _minFontSize = 0.1f;
    [SerializeField] private float _maxFontSize = 1f;
    [SerializeField] private float _speedSizeChanged = 10f;
    [SerializeField] private float _lerp = 0.2f;
    [SerializeField] private float _rotationSpeed = 60f;
    
    [Header("Linear Text")]
    [SerializeField] private TextMeshPro _textMeshProLinear;
    
    private static List<ChangeColorIfNearby> _allPoints = new List<ChangeColorIfNearby>();
    
    
    
    #endregion
}