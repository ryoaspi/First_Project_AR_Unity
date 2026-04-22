using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeColorIfNearby : MonoBehaviour
{
    #region Unity API

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    private void Start()
    {
        _camera = Camera.main;
        _meshRenderer.sharedMaterial = _defaultMaterial;
    }

    private void Update()
    {
        HandleDistance();
        HandleTextRotation();
        HandleVisuals();
    }

    #endregion

    #region Core Logic

    private void HandleDistance()
    {
        _distance = Vector3.Distance(_camera.transform.position, transform.position);

        float t = Mathf.InverseLerp(0f, _distanceMaxForScale, _distance);
        t = Mathf.Pow(t, 2f);
        _textMeshPro.fontSize = Mathf.Lerp(_maxFontSize, _minFontSize, t * _speedSizeChanged);
        Debug.Log(_textMeshPro.fontSize);

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
        }
        else
        {
            if (_meshRenderer.sharedMaterial != _defaultMaterial)
                _meshRenderer.sharedMaterial = _defaultMaterial;
            _textMeshPro.transform.rotation *= Quaternion.Euler(0f, _rotationSpeed*Time.deltaTime,0f );
        }
    }

    #endregion

    #region Private Fields

    private Camera _camera;
    private MeshRenderer _meshRenderer;
    private TextMeshPro _textMeshPro;
    private float _distance;
    private Quaternion _rotation;

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
    
    
    
    #endregion
}