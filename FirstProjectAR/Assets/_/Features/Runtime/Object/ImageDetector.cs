using System;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageDetector : MonoBehaviour
{
    private void Awake()
    {
        _trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        _trackedImageManager.trackablesChanged.AddListener(OnChaged);
    }

    private void OnDisable()
    {
        _trackedImageManager.trackablesChanged.RemoveListener(OnChaged);
    }
    
    private void ReactToTrackableChanges(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        
    }
    
    private void OnChaged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (ARTrackedImage image in eventArgs.added)
        {
            string imageName = image.referenceImage.name;
            Vector3 spawnPosition = image.transform.position;
            Debug.Log("Image Add: " + image.referenceImage.name);
            Shader shader = Shader.Find("Universal Render Pipeline/Lit");
            Renderer Renderer = GetComponent<Renderer>();
            switch (imageName)
            {
                case "one":
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    Renderer.material = new Material(shader);
                    cube.transform.SetParent(image.transform,false);
                    cube.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    Debug.Log("Instantiate Cube: " + cube.name);
                    break;
                case "Unity Logo":
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    Renderer.material = new Material(shader);
                    sphere.transform.transform.SetParent(image.transform,false);
                    sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    Debug.Log("Instantiate Sphere: " + sphere.name);
                    break;
                
                case "Rafflesia":
                    GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    Renderer.material = new Material(shader);
                    capsule.transform.transform.SetParent(image.transform,false);
                    capsule.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    capsule.transform.localRotation = Quaternion.Euler(90f, 0, 0);
                    Debug.Log("Instantiate Capsule: " + capsule.name);
                    break;
                default:
                    Debug.Log("Prefab not found ");
                    break;
            }
            

        }
        foreach (ARTrackedImage image in eventArgs.updated)
        {
            bool isTracked = image.trackingState == TrackingState.Tracking;

            if (image.transform.childCount > 0)
            {
                image.transform.GetChild(0).gameObject.SetActive(isTracked);
            }
        }
        // foreach (ARTrackedImage image in eventArgs.removed)
        // {
        //     
        // }
        
        // eventArgs.added;
    }
    
    private ARTrackedImageManager _trackedImageManager;
    
    
}
