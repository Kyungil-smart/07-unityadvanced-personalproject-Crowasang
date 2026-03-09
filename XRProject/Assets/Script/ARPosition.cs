using System;
using TMPro;
using UnityEngine;

public class ARPosition : MonoBehaviour
{
    [SerializeField] private Transform _xrOrigin;

    [SerializeField] private TextMeshProUGUI _x;
    [SerializeField] private TextMeshProUGUI _y;
    [SerializeField] private TextMeshProUGUI _z;

    private void Update() => RefreshUI();

    private void RefreshUI()
    {
        _x.text = $"x : {_xrOrigin.position.x}";
        _y.text = $"x : {_xrOrigin.position.y}";
        _z.text = $"x : {_xrOrigin.position.z}";
    }
}
