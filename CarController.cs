using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class CarController : MonoBehaviour
{
    [SerializeField] private TypingSpeedCalculator typingSpeedCalculator;
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private float speedNormalizer;
    float speed = 0f;
    float distanceAlongSpline = 0f;
    private bool isOnSpline = true;
    private Vector3 freeMovementDirection;

    private void Start()
    {
        float3 startPos = splineContainer.EvaluatePosition(0f);
        transform.position = new Vector3(startPos.x-1f, startPos.y, startPos.z);
    }
    private void Update()
    {
        if (typingSpeedCalculator != null)
            speed = typingSpeedCalculator.GetWpm();

        if (isOnSpline)
        {
            //move along spline
            distanceAlongSpline += speed * Time.deltaTime * speedNormalizer;

            //get position and rotation from spline 
            float normalizedDistance = (distanceAlongSpline / splineContainer.CalculateLength()) %1f;

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
            Vector3 movement = freeMovementDirection * speed * Time.deltaTime;
            transform.position += movement;
        }

    }

    public float GetDistanceAlongSpline()
    {
        return distanceAlongSpline;
    }
}
