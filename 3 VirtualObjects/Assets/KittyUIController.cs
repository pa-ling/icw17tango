using UnityEngine;
using System.Collections;

public class KittyUIController : MonoBehaviour
{
    public GameObject m_kitten;
    private TangoPointCloud m_pointCloud;

    void Start()
    {
        //Create a reference to the TangoPointCloud script on the TangoPointCloud gameObject
        m_pointCloud = FindObjectOfType<TangoPointCloud>();
    }

    void Update()
    {
        //Check touch count
        if (Input.touchCount == 1)
        {
            // Trigger place kitten function when single touch ended.
            Touch t = Input.GetTouch(0);
            //Check touch state
            if (t.phase == TouchPhase.Ended)
            {
                PlaceKitten(t.position);
            }
        }
    }

    void PlaceKitten(Vector2 touchPosition)
    {
        // Code to query the AR Camera's location, and then call FindPlane()
        // based on the AR Camera's position and the touch position.
        // FindPlane() returns an estimated plane from the touch point,
        // and then places the kitten on the plane if it's not too steep.

        // Find the plane.
        Camera cam = Camera.main;
        Vector3 planeCenter;
        Plane plane;
        if (!m_pointCloud.FindPlane(cam, touchPosition, out planeCenter, out plane))
        {
            Debug.Log("cannot find plane.");
            return;
        }

        // Place kitten on the surface, and make it always face the camera.
        if (Vector3.Angle(plane.normal, Vector3.up) < 30.0f)
        {
            Vector3 up = plane.normal;
            Vector3 right = Vector3.Cross(plane.normal, cam.transform.forward).normalized;
            Vector3 forward = Vector3.Cross(right, plane.normal).normalized;
            Instantiate(m_kitten, planeCenter, Quaternion.LookRotation(forward, up));
        }
        else
        {
            Debug.Log("surface is too steep for kitten to stand on.");
        }
    }
}
