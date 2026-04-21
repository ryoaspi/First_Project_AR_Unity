using TMPro;
using UnityEngine;

public class ChangeColorIfNearby : MonoBehaviour
{
    #region Api Unity

    private void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    private void Start()
    {
        _camera = Camera.main;
        _meshRenderer.material = _defaultMaterial;
    }

    private void FixedUpdate()
    {
        // Vector3 direction = _textMeshPro.transform.position - _camera.transform.position;
        // Quaternion rotation = Quaternion.LookRotation(direction);
        
        _direction = _camera.transform.position - transform.position;
        float distance = _direction.magnitude;
        
        float t = Mathf.InverseLerp(0f, _ColorSwapThreshold, distance);
        
        _textMeshPro.fontSize = Mathf.Lerp(_maxFontSize, _minFontSize, t);
        
        _textMeshPro.text = distance.ToString("0.00");
        
        _textMeshPro.transform.forward = _camera.transform.forward;
        
        if (distance <= _ColorSwapThreshold)
        {
            _meshRenderer.material = _nearbyMaterial;
        }
        else
        {
            _meshRenderer.material = _defaultMaterial;
        }
    }

    #endregion

    #region Private

    private Camera _camera;
    private Vector3 _direction;
    private MeshRenderer _meshRenderer;

    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _nearbyMaterial;

    [SerializeField] private float _ColorSwapThreshold = 1.5f;
    [SerializeField] private TextMeshPro _textMeshPro;

    [SerializeField] private float _minFontSize = 0.1f;
    [SerializeField] private float _maxFontSize = 1f;

    #endregion
}