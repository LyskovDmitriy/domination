using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private new Camera camera;

    private Vector3 minPosition;
    private Vector3 maxPosition;

    private Vector2 lastFrameTouchPosition;


    public Camera Camera => camera;


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
            transform.position = ApplyRestrictionsToPosition(position);
        }

        lastFrameTouchPosition = Input.mousePosition;
    }


    public void Init(Vector2 playerCastlePosition, Vector3 bottomLeftCorner, Vector3 topRightCorner)
    {
        Vector3 cameraOffsetFromCorner = new Vector3(camera.orthographicSize * camera.aspect, camera.orthographicSize);
        minPosition = bottomLeftCorner + cameraOffsetFromCorner;
        maxPosition = topRightCorner - cameraOffsetFromCorner;

        if (minPosition.x > maxPosition.x)
        {
            minPosition.x = 0.0f;
            maxPosition.x = 0.0f;
        }
        if (minPosition.y > maxPosition.y)
        {
            minPosition.y = 0.0f;
            maxPosition.y = 0.0f;
        }

        Vector3 newPosition = new Vector3(playerCastlePosition.x, playerCastlePosition.y, transform.position.z);
        transform.position = ApplyRestrictionsToPosition(newPosition);
    }


    private Vector3 ApplyRestrictionsToPosition(Vector3 position)
    {
        Vector3 newPosition = position;
        newPosition.x = Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minPosition.y, maxPosition.y);
        return newPosition;
    }
}
