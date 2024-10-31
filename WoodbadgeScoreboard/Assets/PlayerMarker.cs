using UnityEngine;
using TMPro;

public class PlayerMarker : MonoBehaviour
{
    public float moveDistance = 5f; // Distance to move the object
    public float moveDuration = 0.5f; // Duration of the movement
    public TMP_InputField scoreInputField; // Reference to the TMP_InputField
    public TMP_Text scoreText; // Reference to the TMP_Text component to display the score

    private bool isMoving = false;
    private Vector3 targetPosition;
    private float totalScore = 0f; // Total score
    private float currentScore = 0f; // Current displayed score

    void Start()
    {
        targetPosition = transform.position;
        UpdateScoreText();
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / moveDuration);
            if (Mathf.Abs(Vector3.Distance(transform.position, targetPosition)) <= Mathf.Abs(0.005f * moveDistance))
            {
                transform.position = targetPosition;
                isMoving = false;
                UpdateScoreText();
            }
        }

        // Interpolate the current score towards the total score
        if (currentScore != totalScore)
        {
            if (Mathf.Abs(totalScore - currentScore) <= Mathf.Abs(0.01f * currentScore))
            {
                currentScore = totalScore;
            }
            else
            {
                currentScore = Mathf.Lerp(currentScore, totalScore, Time.deltaTime / moveDuration);
            }
            UpdateScoreText();
        }
    }

    public void OnButtonClick()
    {
        if (!isMoving)
        {
            if (float.TryParse(scoreInputField.text, out float score))
            {
                moveDistance = score;
                totalScore += moveDistance; // Update total score
                targetPosition = transform.position + Vector3.up * moveDistance;
                isMoving = true;
            }
            else
            {
                Debug.LogError("Invalid score input");
            }
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + currentScore.ToString("F2");
        scoreText.color = isMoving ? Color.red : Color.white;
    }
}