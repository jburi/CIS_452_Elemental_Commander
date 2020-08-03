using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
	public Transform cam;

    void LateUpdate()
    {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }

        transform.LookAt(transform.position + cam.forward);
    }
}
