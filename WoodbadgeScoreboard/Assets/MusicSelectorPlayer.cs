using UnityEngine;
using TMPro;

public class MusicSelectorPlayer : MonoBehaviour
{
    public AudioClip[] audioClips; // Array of AudioClip objects
    private TMP_Dropdown dropdown; // Reference to the TMP_Dropdown UI component
    private AudioSource audioSource;
    public TMP_Text timeText; // Reference to the TMP_Text UI component

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        dropdown = GetComponent<TMP_Dropdown>();
        audioSource.clip = audioClips[0];

        if (dropdown == null)
        {
            Debug.LogError("TMP_Dropdown component not found on the GameObject.");
            return;
        }

        // Populate the dropdown with audio clip names
        dropdown.ClearOptions();
        foreach (AudioClip clip in audioClips)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(clip.name));
        }

        // Add listener for dropdown value change
        dropdown.onValueChanged.AddListener(delegate { OnDropdownValueChanged(dropdown); });
        
        // Set the dropdown value to the first item
        dropdown.value = 1;
        dropdown.value = 0;
        
        // Call the OnDropdownValueChanged method to set the initial audio clip
        OnDropdownValueChanged(dropdown);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (audioSource.isPlaying)
            {
                PauseAudio();
            }
            else
            {
                StartAudio();
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StopAudio();
            //rewind the audio clip to the beginning
            audioSource.time = 0;
        }

        // Update the time text
        if (audioSource.clip != null)
        {
            string currentTime = FormatTime(audioSource.time);
            string totalTime = FormatTime(audioSource.clip.length);
            timeText.text = $"{currentTime} / {totalTime}";
        }
    }

    // Method to handle dropdown value change
    void OnDropdownValueChanged(TMP_Dropdown change)
    {
        int index = change.value;
        if (index >= 0 && index < audioClips.Length)
        {
            audioSource.clip = audioClips[index];
        }
    }

    // Function to start playing the audio clip
    public void StartAudio()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
            timeText.color = Color.red; // Set text color to red when playing
            
        }
    }

    // Function to stop playing the audio clip
    public void StopAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            timeText.color = Color.white; // Set text color to white when stopped
        }
    }

    // Function to pause the audio clip
    public void PauseAudio()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            timeText.color = Color.white; // Set text color to white when stopped
        }
    }

    // Function to format time in minutes and seconds
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}