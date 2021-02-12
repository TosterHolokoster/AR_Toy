using System.Collections.Generic;
using UnityEngine;

public enum TrackingMode { Plane, Image }

// Base class for tracking mods
public abstract class TrackingComponent : MonoBehaviour
{
    public GameObject modelPrefab;

    public abstract TrackingMode GetMode();
    public abstract void OnTouch();
}

// Entry point
public class ARManager : MonoBehaviour
{
    TrackingMode trackingMode = TrackingMode.Plane;

    Dictionary<TrackingMode, TrackingComponent> TrackingComponents = new Dictionary<TrackingMode, TrackingComponent>();
    TrackingComponent currentTrackingComp;

    void Start()
    {
        List<TrackingComponent> TrackingComps = new List<TrackingComponent>();
        GetComponents(TrackingComps);
        foreach (TrackingComponent comp in TrackingComps)
        {
            TrackingComponents.Add(comp.GetMode(), comp);
            if (comp.GetMode() == trackingMode)
            {
                currentTrackingComp = comp;
            }
        }
        currentTrackingComp.enabled = true;
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            currentTrackingComp.OnTouch();
        }
    }

    public void SetTrackingMode(TrackingMode mode)
    {
        if(trackingMode != mode)
        {
            TrackingComponent trackingComponent;
            if (TrackingComponents.TryGetValue(mode, out trackingComponent))
            {
                currentTrackingComp.enabled = false;
                currentTrackingComp = trackingComponent;
                currentTrackingComp.enabled = true;
                trackingMode = mode;
            }
        }
    }

    // For UI buttons 
    public void EnablePlaneTracking() => SetTrackingMode(TrackingMode.Plane);
    public void EnableImageTracking() => SetTrackingMode(TrackingMode.Image);
}
