using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    public bool isThirdPerson = false;
    public Transform thirdPersonLookPos;
    public Transform shoulderContainer;
    public float smoothness = 0.5f;
    public float distance = 5.0f;
    public float zoomSpeed = 5f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float minDistance = 5f;
    public float maxDistance = 20f;

    public float orthoSizeMin = 5f;
    public float orthoSizeMax = 15f;

    float x = 0.0f;
    float y = 0.0f;

    private float orthoSize = 5;
    private float scroll = 0f;
    [HideInInspector] public bool isFreeCamera = false;
    [HideInInspector] public bool isEscapingCamera = false;
    [HideInInspector] public bool isMouseDown = false;
    [HideInInspector] public bool isCameraFrozen = false;
    [HideInInspector] public float mouseX;
    [HideInInspector] public float mouseY;

    Vector3 localPos;
    Vector3 position;
    Quaternion rotation;
    
    // Use this for initialization
    void Start()
    {
        orthoSize = Camera.main.orthographicSize;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        // Lock the cursor on Start
        LockCursor(true);
        localPos = transform.localPosition;
    }
    
    void GetInput()
    {
        scroll = Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        isFreeCamera = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
        isEscapingCamera = Input.GetKeyDown(KeyCode.Escape);
        isMouseDown = Input.GetMouseButtonDown(0);
        isThirdPerson = Input.GetKeyDown(KeyCode.V) ? !isThirdPerson : isThirdPerson;
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        if (Camera.main.orthographic)
        {
            orthoSize = Mathf.Clamp(orthoSize - scroll, orthoSizeMin, orthoSizeMax);
            Camera.main.orthographicSize = orthoSize;
        }
        else
        {
            distance = Mathf.Clamp(distance - scroll, minDistance, maxDistance);
        }

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        position = rotation * negDistance + (thirdPersonLookPos.position);

        x += mouseX * xSpeed * 0.02f;
        y -= mouseY * ySpeed * 0.02f;
        y = ClampAngle(y, yMinLimit, yMaxLimit);
        rotation = Quaternion.Euler(y, x, 0);
    }

    public void UpdateCamera()
    {
        if (isCameraFrozen) return;

        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, smoothness);
        shoulderContainer.rotation = rotation;

        if (isThirdPerson && thirdPersonLookPos)
        {
            transform.position = position;
        }
    }

    void UpdateCursor()
    {
        // If user selects escale key
        if(isEscapingCamera)
        {
            LockCursor(false);
        }

        if(isMouseDown)
        {
            LockCursor(true);
        }
    }

    void LateUpdate()
    {
        GetInput();
        UpdateCursor();
        UpdateCamera();
    }
        
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public void LockCursor(bool isLocked)
    {
        if(isLocked)
        {
            isCameraFrozen = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            isCameraFrozen = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

}
