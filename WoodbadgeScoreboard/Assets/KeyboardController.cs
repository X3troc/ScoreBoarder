using UnityEngine;
using TMPro;

public class KeyboardController : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField; // Reference to the TMP_InputField

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (inputField == null)
        {
            inputField = GameObject.FindWithTag("InputFieldTag").GetComponent<TMP_InputField>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (char.IsDigit(c))
            {
                inputField.text += c;
            }
            else if (c == '\n' || c == '\r') // Enter key
            {
                inputField.text = string.Empty;
            }
            else if (c == '-') // Minus key
            {
                if (!inputField.text.StartsWith("-"))
                {
                    inputField.text = "-" + inputField.text;
                }
            }
            else if (c == '+') // Plus key
            {
                if (inputField.text.StartsWith("-"))
                {
                    inputField.text = inputField.text.Substring(1);
                }
            }
        }
    }
}