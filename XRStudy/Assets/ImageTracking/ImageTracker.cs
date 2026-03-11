using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    [SerializeField] private List<GameObject> _prefabs;
    [SerializeField] private ARTrackedImageManager _arTrackedImageManager;
    [SerializeField] private VideoPlayer _speaki;
    private Dictionary<string, GameObject> _spawnedObjects = new();
    private Dictionary<string, VideoPlayer> _spawnObjects = new();
    private void OnEnable() 
        => _arTrackedImageManager.trackablesChanged.AddListener(OnTrackedImageChanged);
    private void OnDisable()
        => _arTrackedImageManager.trackablesChanged.RemoveListener(OnTrackedImageChanged);

    private void OnTrackedImageChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        // 이미지를 새로 인식했을 때
        foreach (ARTrackedImage trackedImage in args.added)
        {
            // OnImageAdded(trackedImage);
            OnVideoPlay(trackedImage);
        }
        // 이미 인식된 이미지(트래킹중인) 업데이트할 때
        foreach (ARTrackedImage trackedImage in args.updated)
        {
            // OnImageUpdated(trackedImage);
            OnVideoUpdated(trackedImage);
        }
        // 트래킹에서 제거된 이미지 처리
        foreach (KeyValuePair<TrackableId, ARTrackedImage> removed in args.removed)
        {
            // OnImageRemoved(removed.Value);
            OnVideoRemoved(removed.Value);
        }
    }

    private void OnVideoPlay(ARTrackedImage trackedImage)
    {
        VideoPlayer spawnVideo = Instantiate(_speaki, trackedImage.transform.position, trackedImage.transform.rotation);
        string imageName = trackedImage.referenceImage.name;
        spawnVideo.transform.SetParent(trackedImage.transform);
        spawnVideo.transform.localRotation = Quaternion.Euler(90, 0, 0);
        _spawnObjects.Add(imageName, spawnVideo);
    }
    
    private void OnVideoUpdated(ARTrackedImage trackedImage)
    {
        VideoPlayer spawnObject;
        string imageName = trackedImage.referenceImage.name;
        if (!_spawnObjects.TryGetValue(imageName, out spawnObject)) return;
        // 현재 트래킹 상태에 따라
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            spawnObject.Play();
            spawnObject.transform.position = trackedImage.transform.position;
            spawnObject.transform.rotation = trackedImage.transform.rotation;
            spawnObject.transform.localRotation = Quaternion.Euler(90, 0, 0);
        }
        else
        {
            spawnObject.Pause();
        }
    }

    private void OnVideoRemoved(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;
        
        VideoPlayer spawnedObject;
        if (!_spawnObjects.TryGetValue(imageName, out spawnedObject)) return;
        
        Destroy(spawnedObject.gameObject);
        _spawnObjects.Remove(imageName);
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

        // 현재 트래킹 상태에 따라
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            spawnedObject.SetActive(true);
            spawnedObject.transform.position = trackedImage.transform.position;
            spawnedObject.transform.rotation = trackedImage.transform.rotation;
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
