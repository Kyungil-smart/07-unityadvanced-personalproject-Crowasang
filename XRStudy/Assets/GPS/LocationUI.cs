using UnityEngine;
using TMPro;

public class LocationUI : MonoBehaviour
{
    [SerializeField] private GPSManager _gpsManager;
    [SerializeField] private TextMeshProUGUI _lat;
    [SerializeField] private TextMeshProUGUI _lng;
    [SerializeField] private TextMeshProUGUI _refreshCount;

    private int _count = 0;

    private void OnEnable() => _gpsManager.OnLocationChanged += RefreshUI;
    private void OnDisable() => _gpsManager.OnLocationChanged -= RefreshUI;

    private void RefreshUI(EarthPos earthPos)
    {
        _count++;

        _lat.text = $"Lat : {earthPos.Latitude}";
        _lng.text = $"Lng : {earthPos.Longitude}";
        _refreshCount.text = $"Count : {_count}";
    }
}
