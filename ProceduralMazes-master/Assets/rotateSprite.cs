using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceFollowCursor2D : MonoBehaviour
{
    public float rotationSpeed = 5f; // Adjust the speed of the face rotation
    public Vector3 difference;

    void Update()
    {
        difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotZ + 90);
    }
}