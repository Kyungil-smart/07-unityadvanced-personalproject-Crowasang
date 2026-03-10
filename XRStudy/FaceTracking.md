# 페이스 트래킹

- [Github: arcore-unity-sdk canonical_face_mesh](https://github.com/google-ar/arcore-unity-sdk/blob/master/Assets/GoogleARCore/Examples/AugmentedFaces/Models/canonical_face_mesh.fbx)

## Camera 설정

- MainCamera -> AR Camera Manager -> Facing Direciton -> User
  - 전면카메라로 설정

## 소스코드

```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FacePointSpawner : MonoBehaviour
{
    [SerializeField] private ARFaceManager _faceManager;
    [SerializeField] private GameObject _pointPrefab;

    private List<GameObject> _spawnedPoints = new();

    private void OnEnable() => _faceManager.trackablesChanged.AddListener(OnFaceChanged);
    private void OnDisable() => _faceManager.trackablesChanged.RemoveListener(OnFaceChanged);

    private void OnFaceChanged(ARTrackablesChangedEventArgs<ARFace> args)
    {
        // 얼굴을 새로 인식했을 때
        foreach (ARFace face in args.added)
        {
            SpawnPoints(face);
        }
        // 얼굴이 업데이트 됐을 때
        foreach (ARFace face in args.updated)
        {
            UpdatePoints(face);
        }
        // 얼굴이 사라졌을 때(소환한 Point들 일괄 정리)
        if (args.removed.Count > 0)
        {
            DestroyAllPoints();
        }
    }

    private void SpawnPoints(ARFace face)
    {
        foreach (Vector3 localVertex in face.vertices)
        {
            Vector3 worldPosition = face.transform.TransformPoint(localVertex);
            GameObject point = Instantiate(_pointPrefab, worldPosition, Quaternion.identity);
            _spawnedPoints.Add(point);
        }
    }

    private void UpdatePoints(ARFace face)
    {
        if (face.vertices.Length != _spawnedPoints.Count)
        {
            DestroyAllPoints();
            SpawnPoints(face);
            return;
        }

        if (face.trackingState == TrackingState.Tracking)
        {
            SetPointsActive(true);

            for (int i = 0; i < face.vertices.Length; i++)
            {
                _spawnedPoints[i].transform.position = face.transform.TransformPoint(face.vertices[i]);
            }
        }
        else
        {
            SetPointsActive(false);
        }
    }

    private void SetPointsActive(bool isActive)
    {
        foreach (GameObject point in _spawnedPoints)
        {
            point.SetActive(isActive);
        }
    }

    private void DestroyAllPoints()
    {
        foreach (GameObject point in _spawnedPoints)
        {
            Destroy(point);
        }
        
        _spawnedPoints.Clear();
    }
}
```