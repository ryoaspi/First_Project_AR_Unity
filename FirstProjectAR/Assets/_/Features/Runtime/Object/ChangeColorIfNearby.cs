using System;
using UnityEngine;

public class ChangeColorIfNearby : MonoBehaviour
{
        #region Api Unity

        private void Awake()
        {
                _meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        private void Start()
        {
                _camera = Camera.main;
                _meshRenderer.material = _defaultMaterial;
        }

        private void Update()
        {
                _direction = transform.position -  _camera.transform.position; 
                float distance = _direction.magnitude;
                
                if (distance <= _ColorSwapThreshold)
                {
                        _meshRenderer.material = _nearbyMaterial;
                        Debug.Log( _meshRenderer.material.name );
                }

                else
                {
                        _meshRenderer.material = _defaultMaterial;
                        Debug.Log( _meshRenderer.material.name );
                        
                }
                
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
        [SerializeField] private 

        #endregion
}
