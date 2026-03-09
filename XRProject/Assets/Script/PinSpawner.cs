using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _pinPrefab;
    [SerializeField] private ARRaycastManager _raycastManager;
    [SerializeField] private LayerMask _planeLayerMask;
    [SerializeField] private float _rayDistance;
    
    private ARInputAction _inputAction;
    private Transform _currentPin;

    private void Awake() => Init();

    private void OnEnable() => SubscribeInputActions();
    
    private void OnDisable() => UnsubscribeInputActions();

    private void OnDestroy() => CleanUp();

    private void CreatePin(InputAction.CallbackContext context)
    {
        if (_currentPin != null) return;

        _currentPin = Instantiate(_pinPrefab).transform;
    }

    private void HandlePintTransform(InputAction.CallbackContext context)
    {
        if (_currentPin == null) return;
        
        Vector2 touchPosition = context.ReadValue<Vector2>();
        // SetPinTransformFromRaycast(touchPosition); // 일반 Raycast
        SetPinTransformFromARRaycast(touchPosition); // ARRaycast
    }

    private void SetPinTransformFromARRaycast(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        List<ARRaycastHit> hits = new();
        
        if (_raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;
            _currentPin.position = pose.position;
            _currentPin.rotation = pose.rotation;
        }
    }
    
    private void SetPinTransformFromRaycast(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _rayDistance, _planeLayerMask))
        {
            _currentPin.transform.position = hit.point;
            // Pin의 Transform.up을 범선 방향으로 설정 : 핀이 표면에 수직으로 꽂힘
            _currentPin.up = hit.normal;
        }
    }
    private void FixPinPosition(InputAction.CallbackContext context)
    {
        if (_currentPin == null) return;
        _currentPin = null;
    }

    private void Init()
    {
        _inputAction = new ARInputAction();
    }
    
    private void CleanUp()
    {
        _inputAction.Dispose();
    }

    private void SubscribeInputActions()
    {
        _inputAction.UserTouch.Enable();
        _inputAction.UserTouch.TouchPress.started += CreatePin;
        _inputAction.UserTouch.TouchPress.canceled += FixPinPosition;
        _inputAction.UserTouch.TouchPosition.performed += HandlePintTransform;
    }

    private void UnsubscribeInputActions()
    {
        _inputAction.UserTouch.Disable();
        _inputAction.UserTouch.TouchPress.started -= CreatePin;
        _inputAction.UserTouch.TouchPress.canceled -= FixPinPosition;
        _inputAction.UserTouch.TouchPosition.performed -= HandlePintTransform;
    }
}
