using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]
[RequireComponent(typeof(ARRaycastManager))]
public class PlaneTracking : TrackingComponent
{
    public GameObject pointerPrefab;

    private ARPlaneManager planeManager;
    private ARRaycastManager raycastManager;

    private GameObject pointer;
    private GameObject model;

    void Awake()
    {
        planeManager = GetComponent<ARPlaneManager>();
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void OnEnable()
    {
        planeManager.enabled = raycastManager.enabled = true;
        pointer = Instantiate(pointerPrefab);
        pointer.SetActive(false);
    }

    void OnDisable()
    {
        planeManager.enabled = raycastManager.enabled = false;
        if(pointer)
            Destroy(pointer);
        if (model)
            Destroy(model);
    }

    void Update()
    {
        List<ARRaycastHit> hit = new List<ARRaycastHit>();
        if (raycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hit, TrackableType.Planes))
        {
            pointer.transform.position = hit[0].pose.position;
            pointer.SetActive(true);
        }
    }

    public override TrackingMode GetMode() => TrackingMode.Plane;

    public override void OnTouch()
    {
        if (model)
            Destroy(model);
        model = Instantiate(modelPrefab, pointer.transform.position, new Quaternion());
        model.GetComponent<Animator>().Play("Walk", -1);
    }
}
