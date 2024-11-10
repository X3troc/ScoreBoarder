using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerMarker : MonoBehaviour
{
    public float moveDistance = 5f; // Distance to move the object
    public float moveDuration = 0.5f; // Duration of the movement
    public TMP_InputField scoreInputField; // Reference to the TMP_InputField
    public TMP_Text scoreText; // Reference to the TMP_Text component to display the score
    public float rotationSpeed = 60f; // Speed of rotation in degrees per second
    public AudioClip[] MoveSoundFX; // Array of AudioClip objects for move sounds
    private AudioSource audioSource; // Reference to the AudioSource component

    private bool isMoving = false;
    private Vector3 targetPosition;
    private float totalScore = 0f; // Total score
    private float currentScore = 0f; // Current displayed score
    [SerializeField] private bool isWinning = false;
    private Coroutine rotationCoroutine;
    [SerializeField] ParticleSystem fxStartMove; // Reference to the particle system
    [SerializeField] ParticleSystem fxLeader; // Reference to the particle system

    void Start()
    {
        targetPosition = transform.position;
        UpdateScoreText();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
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
                PlayMoveSound(); // Play a move sound
                if (!fxStartMove.isPlaying)
                {
                    fxStartMove.Play(); // Play the particle system
                }
            }
            else
            {
                Debug.LogError("Invalid score input");
            }
        }
    }

    private void PlayMoveSound()
    {
        if (MoveSoundFX.Length > 0)
        {
            int randomIndex = Random.Range(0, MoveSoundFX.Length);
            audioSource.PlayOneShot(MoveSoundFX[randomIndex]);
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + currentScore.ToString("F2");
        scoreText.color = isMoving ? Color.red : Color.white;
    }

    public void SetIsWinning(bool winning)
    {
        isWinning = winning;
        if (isWinning)
        {
            if (rotationCoroutine == null)
            {
                rotationCoroutine = StartCoroutine(RotateWhileWinning());
                fxLeader.Play(); // Play the particle system
            }
        }
    }

    private IEnumerator RotateWhileWinning()
    {
        while (true)
        {
            float rotationAmount = 0f;
            while (rotationAmount < 360f)
            {
                float rotationStep = rotationSpeed * Time.deltaTime;
                transform.Rotate(Vector3.up, rotationStep);
                rotationAmount += rotationStep;
                Debug.Log(rotationAmount);
                yield return null;
            }

            if (!isWinning)
            {
                rotationCoroutine = null;
                fxLeader.Stop(); // Stop the particle system
                break;
            }
        }
    }
}