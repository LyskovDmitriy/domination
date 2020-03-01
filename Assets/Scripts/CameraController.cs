using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private new Camera camera;

    private Vector3 minPosition;
    private Vector3 maxPosition;

    private Vector2 lastFrameTouchPosition;


    private void Awake()
    {
        camera = GetComponent<Camera>();
    }


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 touchWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 lastFrameTouchWorldPosition = camera.ScreenToWorldPoint(lastFrameTouchPosition);

            Vector3 position = transform.position - (touchWorldPosition - lastFrameTouchWorldPosition);
            position.x = Mathf.Clamp(position.x, minPosition.x, maxPosition.x);
            position.y = Mathf.Clamp(position.y, minPosition.y, maxPosition.y);
            transform.position = position;
        }

        lastFrameTouchPosition = Input.mousePosition;
    }


    public void SetRestrictions(Vector3 bottomLeftCorner, Vector3 topRightCorner)
    {
        Vector3 cameraOffsetFromCorner = new Vector3(camera.orthographicSize * camera.aspect, camera.orthographicSize);
        minPosition = bottomLeftCorner + cameraOffsetFromCorner;
        maxPosition = topRightCorner - cameraOffsetFromCorner;
    }
}
