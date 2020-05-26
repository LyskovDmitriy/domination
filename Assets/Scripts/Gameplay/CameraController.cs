using UnityEngine;


public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector2 additionalCameraOffsetFromCorner = default;

    private Vector3 minPosition;
    private Vector3 maxPosition;

    private Vector2 lastFrameTouchPosition;


    public Camera Camera { get; private set; }


    private void Awake()
    {
        Camera = GetComponent<Camera>();
    }


    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 touchWorldPosition = Camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 lastFrameTouchWorldPosition = Camera.ScreenToWorldPoint(lastFrameTouchPosition);

            Vector3 position = transform.position - (touchWorldPosition - lastFrameTouchWorldPosition);
            transform.position = ApplyRestrictionsToPosition(position);
        }

        lastFrameTouchPosition = Input.mousePosition;
    }


    public void Init(Vector2 playerCastlePosition, Vector3 bottomLeftCorner, Vector3 topRightCorner)
    {
        Vector3 cameraOffsetFromCorner = new Vector3(Camera.orthographicSize * Camera.aspect, Camera.orthographicSize);
        minPosition = bottomLeftCorner + cameraOffsetFromCorner - additionalCameraOffsetFromCorner.ToVector3();
        maxPosition = topRightCorner - cameraOffsetFromCorner + additionalCameraOffsetFromCorner.ToVector3();

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
