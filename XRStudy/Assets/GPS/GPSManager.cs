using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;

public class GPSManager : MonoBehaviour
{
    [SerializeField] private float _interval;
    [SerializeField] private int _defaultWaitCount;

    private EarthPos _earthPos;
    private Coroutine _coroutine;
    private WaitForSeconds _wait;

    public event Action<EarthPos> OnLocationChanged;

    private void Awake() => Init();
    private void Start() => PermissionHandler.Request(Permission.FineLocation, OnGrantedFineLocation);
    
    private void OnGrantedFineLocation(string fineLocation) => GPSOn();

    private void GPSOn()
    {
        // 코루틴 시작
        if (_coroutine != null) StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(GpsRoutine());
    }

    private IEnumerator GpsRoutine()
    {
        // 1. 디바이스에서 '위치'가 사용중이지 않다면?
        if (!Input.location.isEnabledByUser)
        {
            _coroutine = null;
            yield break;
        }
        
        // 2. Gps Run
        Input.location.Start();
        
        // 초기화 단계라면 될때까지 조금 기다려보자.
        int waitCount = _defaultWaitCount;
        while (Input.location.status == LocationServiceStatus.Initializing && waitCount > 0)
        {
            yield return _wait;
            waitCount--;
        }

        // 기다릴만큼 기다렸다면. or GPS 상태가 '실패'일 경우
        if (waitCount < 1 || Input.location.status == LocationServiceStatus.Failed)
        {
            _coroutine = null;
            yield break;
        }
        
        // 위/경도 갱신만.
        while (true)
        {
            LocationInfo location = Input.location.lastData;
            _earthPos.Latitude = location.latitude;
            _earthPos.Longitude = location.longitude;
            
            OnLocationChanged?.Invoke(_earthPos);

            yield return _wait;
        }
    }

    private void Init()
    {
        _coroutine = null;
        _wait = new WaitForSeconds(_interval);
    }
}
