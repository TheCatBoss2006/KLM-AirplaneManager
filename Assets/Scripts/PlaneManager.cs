using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    // Plane's Light component
    public Light planeLight;

    // Speed of the plane's forward movement (horizontal movement)
    public float speed = 2f;

    // Distance at which the plane starts lifting off
    public float liftOffDistance = 5f;

    // Height at which the plane will straighten out (Y-axis)
    public float straightenHeight = 5f;

    // Max angle at which the plane will lift off (in degrees)
    public float maxLiftOffAngle = 20f;

    // The speed at which the plane changes its pitch angle (slower for smoother transition)
    public float liftOffSpeed = 2f;

    // Vertical speed for the plane's ascension (only for Y-axis)
    public float verticalLiftSpeed = 1f;

    // Distance at which the plane will stop moving forward
    public float stopDistance = 50f;

    // Initial position (used to know where to park)
    private Vector3 initialPosition;

    // Initial rotation (used to know the original rotation)
    private Quaternion initialRotation;

    // Whether the plane is moving
    private bool isMoving = false;

    // Whether the plane is parking (public to be used in UIManager)
    public bool isParking = false;

    // Whether the plane is parked
    public bool isParked = false;

    // Distance traveled
    private float traveledDistance = 0f;

    void Start()
    {
        // Set initial state
        isMoving = false;
        isParking = false;
        isParked = false;
        traveledDistance = 0f;

        // Save the initial position and rotation
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Ensure the light is turned off by default
        if (planeLight != null)
        {
            planeLight.enabled = false;
        }
    }

    void Update()
    {
        // Handle plane movement if it is not in parking mode
        if (isMoving && !isParking)
        {
            // Move forward logic (same as before)
            float distanceThisFrame = speed * Time.deltaTime;
            transform.Translate(transform.forward * distanceThisFrame);
            traveledDistance += distanceThisFrame;

            // Lift-off logic (same as before)
            if (traveledDistance >= liftOffDistance)
            {
                float targetHeight = Mathf.Lerp(0f, straightenHeight, (traveledDistance - liftOffDistance) / (25f - liftOffDistance));

                if (transform.position.y < targetHeight)
                {
                    transform.Translate(Vector3.up * verticalLiftSpeed * Time.deltaTime);
                }

                float angleToTarget = Mathf.Lerp(0f, maxLiftOffAngle, (traveledDistance - liftOffDistance) / (12.5f - liftOffDistance));
                if (traveledDistance >= 12.5f)
                {
                    angleToTarget = Mathf.Lerp(maxLiftOffAngle, 0f, (traveledDistance - 12.5f) / (25f - 12.5f));
                }

                angleToTarget = Mathf.Clamp(angleToTarget, 0f, maxLiftOffAngle);
                Quaternion targetRotation = Quaternion.Euler(angleToTarget, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, liftOffSpeed * Time.deltaTime);
            }

            // Stop the plane after reaching the stop distance
            if (traveledDistance >= stopDistance)
            {
                isMoving = false;  // Stop the plane
                Debug.Log("Plane has reached the stop distance.");

                // Rotate the plane 180 degrees around the Z axis after stop
                transform.rotation = Quaternion.Euler(initialRotation.eulerAngles.x, transform.rotation.eulerAngles.y, 180f);
            }
        }

        // If the plane is parking, we need to do the parking actions
        if (isParking)
        {
            // Move the plane back to the starting position
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, step);

            // Move the plane to its original Y position (assuming you want to return to the ground level)
            if (transform.position.y > initialPosition.y)
            {
                transform.position = new Vector3(transform.position.x, initialPosition.y, transform.position.z);
            }

            // Once the plane passes the starting position, reset position and rotation directly
            if (transform.position.x == initialPosition.x && transform.position.z == initialPosition.z)
            {
                // Reset position and rotation to original
                transform.position = initialPosition;
                transform.rotation = initialRotation;

                // Mark the plane as parked
                isParked = true;
                isParking = false; // Stop parking state
                Debug.Log("Plane has returned to its starting position and is now parked.");
            }
        }
    }

    // Function to start moving the plane
    public void StartMoving()
    {
        if (isParked)
        {
            // If the plane is parked, reinitialize and start moving again
            isParked = false; // Unmark the plane as parked
            traveledDistance = 0f; // Reset traveled distance
        }

        isMoving = true;
        isParking = false;
    }

    // Function to stop moving the plane
    public void StopMoving()
    {
        isMoving = false;
        isParked = true;
    }

    // Function to turn the light on
    public void TurnLightOn()
    {
        if (planeLight != null)
        {
            planeLight.enabled = true;
        }
    }

    // Function to turn the light off
    public void TurnLightOff()
    {
        if (planeLight != null)
        {
            planeLight.enabled = false;
        }
    }

    // Function to park the plane (triggered by UIManager)
    public void ParkPlane()
    {
        if (!isParked)
        {
            // Start the parking process (move the plane back to its original position)
            Debug.Log("Plane is now parking.");
            isParking = true;
        }
    }

    // Function to check if the plane has reached the stop distance
    public bool HasReachedStopDistance()
    {
        return traveledDistance >= stopDistance;
    }
}
