using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapRenderer : MonoBehaviour
{
    public Renderer mapRenderer;
    public LayerMask viewConeLayer; // assign the ViewConeLayer in the Inspector

    private void Update()
    {
        // Set the layer mask to only render the map when the view cone is visible
        mapRenderer.enabled = ((1 << gameObject.layer) & viewConeLayer) != 0;
    }
}