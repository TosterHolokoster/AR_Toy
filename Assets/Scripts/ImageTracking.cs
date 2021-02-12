using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : TrackingComponent
{
    private ARTrackedImageManager imageManager;

    private GameObject model;

    void Awake()
    {
        imageManager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        imageManager.enabled = true;
        imageManager.trackedImagesChanged += OnChanged;

        model = Instantiate(modelPrefab);
        model.SetActive(false);
    }

    void OnDisable()
    {
        imageManager.enabled = false;
        imageManager.trackedImagesChanged -= OnChanged;

        if (model)
            Destroy(model);
    }

    public override TrackingMode GetMode() => TrackingMode.Image;

    public override void OnTouch()
    {
        if(model)
        {
            model.GetComponent<Animator>().Play("Walk", -1, 0);
        }
    }

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            model.transform.position = newImage.transform.position;
            model.transform.rotation = newImage.transform.rotation;
            model.SetActive(true);
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            model.transform.position = updatedImage.transform.position;
            model.transform.rotation = updatedImage.transform.rotation;
        }

        foreach (var removedImage in eventArgs.removed)
        {
            model.SetActive(false);
        }
    }
}
