using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

public class ImageDetection : MonoBehaviour
{
    [SerializeField]
    private Text CurrentDebug;
    [SerializeField]
    private Text currentImageText;
    [SerializeField]
    private GameObject prefabOnTrack;
    [SerializeField]
    private Vector3 ImageSF = new Vector3(0.1f,0.1f,0.1f);
    [SerializeField]
    XRReferenceImageLibrary xrRefenceImageLibary;

    private ARTrackedImageManager trackImageManager;

    void Start()
    {
        CurrentDebug.text += "Creating Image Library/n";
        trackImageManager = gameObject.AddComponent<ARTrackedImageManager>();
        trackImageManager.referenceLibrary = trackImageManager.CreateRuntimeLibrary(xrRefenceImageLibary);
        trackImageManager.requestedMaxNumberOfMovingImages = 4;
        trackImageManager.trackedImagePrefab = prefabOnTrack;

        trackImageManager.enabled = true;
        trackImageManager.trackedImagesChanged += OnTrackedImagesChanged;

        ShowTrackInfo();
    }
    public void ShowTrackInfo()
    {
        var runtimeReferenceImageLibrary = trackImageManager.referenceLibrary as MutableRuntimeReferenceImageLibrary;
        CurrentDebug.text += $"TextureFormat.RGBA32 supported: {runtimeReferenceImageLibrary.IsTextureFormatSupported(TextureFormat.RGBA32)}\n";
        CurrentDebug.text += $"Supported Texture Count{(runtimeReferenceImageLibrary.supportedTextureFormatCount)}\n";
        CurrentDebug.text += $"trackImageManager.trackables.count ({trackImageManager.trackables.count})\n";
        CurrentDebug.text += $"trackImageManager.trackImagePrefab.name({trackImageManager.trackedImagePrefab.name})\n";
        CurrentDebug.text += $"trackImageManager.maxNumberOfMovingImages ({trackImageManager.requestedMaxNumberOfMovingImages})\n";
        CurrentDebug.text += $"trackImageManager.supportMultableLibrary ({trackImageManager.subsystem.subsystemDescriptor.supportsMutableLibrary})\n";
        CurrentDebug.text += $"trackImageManager.requiresPhysicalImageDimensions ({trackImageManager.subsystem.subsystemDescriptor.requiresPhysicalImageDimensions})\n";

    }

    void OnDisable()
    {
        trackImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {   //Display the name of the tracked image in the canvas
            currentImageText.text = trackedImage.referenceImage.name;
            // Give the intial image a reasonable default scale
            trackedImage.transform.localScale = new Vector3(-trackedImage.referenceImage.size.x, 0.005f, -trackedImage.referenceImage.size.y);
        }
        foreach(ARTrackedImage trackedImage in eventArgs.updated)
        {
            currentImageText.text = trackedImage.referenceImage.name;

            trackedImage.transform.localScale = new Vector3(-trackedImage.referenceImage.size.x, 0.005f, -trackedImage.referenceImage.size.y);
        }
    }
}
