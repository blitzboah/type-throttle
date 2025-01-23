using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class BotController : MonoBehaviour
{
    [SerializeField] private SplineContainer splineContainer;
    [SerializeField] private float speed;
    [SerializeField] private float accelerationTime = 3f;
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private float speedNormalizer;
    [SerializeField] private int wpmOffset = 10;
    [SerializeField] private TypingSpeedCalculator typingSpeedCalculator;
    [SerializeField] private CarController carController;
    [SerializeField] private Text botWpm;
    private int wordThreshold = 3;

    private float distanceAlongSpline = 0f;
    private float speedVelocity;
    private bool isOnSpline = true;
    private Vector3 freeMovementDirection;
    private float maxBotWpm;
    private bool isAdaptive;
    private float targetSpeed;
    private bool hasReachedMaxSpeed = false;
    private float currentMovementSpeed;
    private int lastWordCount = 0;

    private void Awake()
    {
        isAdaptive = BotSpeedManager.isAdaptive;
    }

    void Start()
    {
        if(!isAdaptive)
        {
            botWpm.gameObject.SetActive(false);
        }
        if (typingSpeedCalculator == null) return;
        maxBotWpm = BotSpeedManager.botSpeed;
        speed = maxBotWpm;
        targetSpeed = maxBotWpm;

        float3 startPos = splineContainer.EvaluatePosition(0f);
        transform.position = new Vector3(startPos.x, startPos.y, startPos.z);
        distanceAlongSpline = 0f;
    }

    void Update()
    {
        if (!RaceCountdown.raceStarted) return;
        botWpm.text = currentSpeed.ToString("0");

        if (isAdaptive)
        {
            AdaptiveSpeedLogic();
        }
        else
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, speed, ref speedVelocity, accelerationTime);
        }

        MoveAlongSpline();
    }

    private void AdaptiveSpeedLogic()
    {
        float movementVelocity = 0f;
        int currentWordCount = typingSpeedCalculator.wordCount;

        if (!hasReachedMaxSpeed)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, maxBotWpm, ref speedVelocity, accelerationTime);
            currentMovementSpeed = Mathf.SmoothDamp(currentMovementSpeed, currentSpeed, ref movementVelocity, accelerationTime);

            if (currentSpeed >= maxBotWpm * 0.95f)
            {
                hasReachedMaxSpeed = true;
                currentSpeed = maxBotWpm;
            }
            return;
        }

        float playerWpm = typingSpeedCalculator.GetWpm();
        float targetWpm;

        if (currentWordCount / wordThreshold > lastWordCount / wordThreshold)
        {
            float playerToBotRatio = carController.GetDistanceAlongSpline() / GetDistanceAlongSpline();
            Debug.Log(currentWordCount);
            if (currentSpeed < playerWpm || playerToBotRatio >= 1f)
            {
                // keep up
                targetWpm = playerWpm * 1.1f;
            }
            else if(currentSpeed > playerWpm && playerToBotRatio < 1f)
            {
                targetWpm = playerWpm * 0.8f;
            }
            else
            {
                targetWpm = playerWpm + wpmOffset;
            }

            targetSpeed = (targetWpm / maxBotWpm) * speed;
            lastWordCount = currentWordCount;
        }

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, accelerationTime);
        currentMovementSpeed = Mathf.SmoothDamp(currentMovementSpeed, currentSpeed, ref movementVelocity, accelerationTime);
    }

    private void MoveAlongSpline()
    {
        if (isOnSpline)
        {
            distanceAlongSpline += currentSpeed * Time.deltaTime * speedNormalizer;

            float normalizedDistance = (distanceAlongSpline / splineContainer.CalculateLength()) % 1f;
            float3 position = splineContainer.EvaluatePosition(normalizedDistance);
            float3 tangent = splineContainer.EvaluateTangent(normalizedDistance);
            Quaternion rotation = Quaternion.LookRotation(new Vector3(tangent.x, tangent.y, tangent.z));

            transform.position = new Vector3(position.x, position.y, position.z);
            transform.rotation = rotation;
        }
        else
        {
            Vector3 movement = freeMovementDirection * currentSpeed * Time.deltaTime;
            transform.position += movement;
        }
    }

    public float GetDistanceAlongSpline()
    {
        return distanceAlongSpline;
    }
}