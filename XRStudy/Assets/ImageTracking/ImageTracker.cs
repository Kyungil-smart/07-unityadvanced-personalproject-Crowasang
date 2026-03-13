using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    [SerializeField] private List<GameObject> _prefabs;
    [SerializeField] private ARTrackedImageManager _arTrackedImageManager;
    private Dictionary<string, GameObject> _spawnedObjects = new();
    private void OnEnable() 
        => _arTrackedImageManager.trackablesChanged.AddListener(OnTrackedImageChanged);
    private void OnDisable()
        => _arTrackedImageManager.trackablesChanged.RemoveListener(OnTrackedImageChanged);

    private void OnTrackedImageChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        // 이미지를 새로 인식했을 때
        foreach (ARTrackedImage trackedImage in args.added)
        {
            OnImageAdded(trackedImage);
        }
        // 이미 인식된 이미지(트래킹중인) 업데이트할 때
        foreach (ARTrackedImage trackedImage in args.updated)
        {
            OnImageUpdated(trackedImage);
        }
        // 트래킹에서 제거된 이미지 처리
        foreach (KeyValuePair<TrackableId, ARTrackedImage> removed in args.removed)
        {
            OnImageRemoved(removed.Value);
        }
    }
    
    private void OnImageAdded(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;
        
        GameObject prefab = FindPrefabByName(imageName);
        if (prefab == null) return;

        GameObject spawnedObject = Instantiate(prefab);
        _spawnedObjects.Add(imageName, spawnedObject);
    }

    private void OnImageUpdated(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        GameObject spawnedObject;
        if (!_spawnedObjects.TryGetValue(imageName, out spawnedObject)) return;
        VideoPlayer video = spawnedObject.GetComponent<VideoPlayer>();

        // 현재 트래킹 상태에 따라
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            if (video != null) 
            {
                spawnedObject.SetActive(true);
                spawnedObject.transform.position = trackedImage.transform.position;
                spawnedObject.transform.rotation = trackedImage.transform.rotation * Quaternion.Euler(90, 0, 0);
            }
            else 
            {
                spawnedObject.SetActive(true);
                spawnedObject.transform.position = trackedImage.transform.position;
                spawnedObject.transform.rotation = trackedImage.transform.rotation * Quaternion.Euler(0, 180, 0);
            }
        }
        else
        {
            spawnedObject.SetActive(false);
        }
    }

    private void OnImageRemoved(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;
        
        GameObject spawnedObject;
        if (!_spawnedObjects.TryGetValue(imageName, out spawnedObject)) return;
        
        _spawnedObjects.Remove(imageName);
        Destroy(spawnedObject);
    }

    private GameObject FindPrefabByName(string imageName)
    {
        foreach (GameObject prefab in _prefabs)
        {
            if (prefab.name == imageName) return prefab;
        }

        return null;
    }
}
