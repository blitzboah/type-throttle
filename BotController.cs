using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class BotController : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField]float speed = BotSpeedManager.botSpeed; //get the speed from botspeedmanager
    [SerializeField] float accelerationTime = 3f;
    [SerializeField] float currentSpeed = 0f;

    float distanceAlongSpline = 0f;
    float speedVelocity;
    private bool isOnSpline = true;
    private Vector3 freeMovementDirection;
    [SerializeField] float timer;
    [SerializeField] private float speedNormalizer;

    void Start()
    {
        //align car position with spline 
        float3 startPos = splineContainer.EvaluatePosition(0f);
        transform.position = new Vector3(startPos.x, startPos.y, startPos.z);

        distanceAlongSpline = 0f;
    }
    void Update()
    {
        if (!RaceCountdown.raceStarted) return;

        currentSpeed = Mathf.SmoothDamp(currentSpeed, speed, ref speedVelocity, accelerationTime);

        if (isOnSpline)
        {
            //move along spline
            distanceAlongSpline += currentSpeed * Time.deltaTime * speedNormalizer;

            //get position and rotation from spline 
            float normalizedDistance = (distanceAlongSpline / splineContainer.CalculateLength()) % 1f;

            //get position along spline
            float3 position = splineContainer.EvaluatePosition(normalizedDistance);

            //rotation shit
            float3 tangent = splineContainer.EvaluateTangent(normalizedDistance);
            Quaternion rotation = Quaternion.LookRotation(new Vector3(tangent.x, tangent.y, tangent.z));

            //update transform
            transform.position = new Vector3(position.x, position.y, position.z);
            transform.rotation = rotation;
        }
        else
        {
            Vector3 movement = freeMovementDirection * currentSpeed * Time.deltaTime;
            transform.position += movement;
        }
    }
}
