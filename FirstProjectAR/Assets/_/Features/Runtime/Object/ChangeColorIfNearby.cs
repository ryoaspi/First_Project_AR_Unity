using System;
using TMPro;
using UnityEngine;

public class ChangeColorIfNearby : MonoBehaviour
{
        #region Api Unity

        private void Awake()
        {
                _meshRenderer = GetComponentInChildren<MeshRenderer>();
                _textMeshPro =  GetComponentInChildren<TextMeshPro>();
        }

        private void Start()
        {
                _camera = Camera.main;
                _meshRenderer.material = _defaultMaterial;
        }

        private void Update()
        {
                // Vector3 direction = _camera.transform.position - transform.position;
                // Vector3 dir = Vector3.ProjectOnPlane(direction, Vector3.up);  
                // Quaternion rotation = Quaternion.LookRotation(dir);
                // transform.rotation = rotation;
                //
                // _direction = transform.position -  _camera.transform.position; 
                // float distance = _direction.magnitude;
                // float t = Mathf.InverseLerp(_ColorSwapThreshold,0,distance);
                //
                // _textMeshPro.fontSize = Mathf.Lerp(_minFontSize,_maxFontSize,t);
                //
                // _textMeshPro.text = distance.ToString("0.00");
                // _textMeshPro.transform.forward += _camera.transform.forward;
                //
                // if (distance <= _ColorSwapThreshold)
                // {
                //         _meshRenderer.material = _nearbyMaterial;
                //         Debug.Log( _meshRenderer.material.name );
                // }
                //
                // else
                // {
                //         _meshRenderer.material = _defaultMaterial;
                //         Debug.Log( _meshRenderer.material.name );
                //
                //         
                // }
                
        }

        private void FixedUpdate()
        {
                Vector3 direction =  transform.position -  _camera.transform.position;

                // Rotation bloquée sur Y
                Vector3 dir = Vector3.ProjectOnPlane(direction, Vector3.up);

                if (dir.sqrMagnitude > 0.001f)
                        transform.rotation = Quaternion.LookRotation(dir);

                float distance = direction.magnitude;

                float t = Mathf.InverseLerp(_ColorSwapThreshold, 0f, distance);
                _textMeshPro.fontSize = Mathf.Lerp(_minFontSize, _maxFontSize, t);

                _textMeshPro.text = distance.ToString("0.00");

                if (distance <= _ColorSwapThreshold)
                        _meshRenderer.material = _nearbyMaterial;
                else
                        _meshRenderer.material = _defaultMaterial;
        }

        #endregion
        
        
        #region Main Methods
        
        
        
        #endregion
        
        
        #region Private And Protected
        
        private Camera _camera;
        private Vector3 _direction;
        private MeshRenderer _meshRenderer;
        
        [SerializeField,Tooltip("Default Material of this cube")] private Material _defaultMaterial;
        [SerializeField,Tooltip("Material of this cube")] private Material _nearbyMaterial;
        
        [SerializeField,Tooltip("Distance at which we change the color")] private float _ColorSwapThreshold = 1.5f;
        [SerializeField] private TextMeshPro _textMeshPro;
        [SerializeField] private float _minFontSize = 0.01f;
        [SerializeField] private float _maxFontSize = 1f;

        #endregion
}
