using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PixelPerfect : MonoBehaviour
{
    [SerializeField] float PixelsPerUnit =1;
    Camera cam;
    Quaternion rotation;
	// Start is called before the first frame update
	private void Awake()
	{
		cam = GetComponent<Camera>();
        rotation = Camera.main.transform.rotation;
	}

	// Update is called once per frame
	void FixedUpdate()
    {

        Vector3 cameraPosition = cam.transform.position;
        Vector3 roundedCameraPosition = RoundVect(cameraPosition,PixelsPerUnit);
        Vector3 offset = roundedCameraPosition - cameraPosition;
        offset.z = -offset.z;
        Matrix4x4 offsetMatrix = Matrix4x4.TRS(-offset, Quaternion.identity, new Vector3(1.0f, 1.0f, -1.0f));

        cam.worldToCameraMatrix = offsetMatrix * cam.transform.worldToLocalMatrix;
    }
    Vector3 RoundVect(Vector3 v,float s)
	{
        float xScale = Screen.width /s;
        float yScale = Screen.height /s;

        float x = Mathf.Round(v.x * xScale) / xScale;
        float y = Mathf.Round(v.y * s) / s;
        float z = Mathf.Round(v.z * yScale) / yScale;
        return new Vector3(x,y,z);
	}
}
