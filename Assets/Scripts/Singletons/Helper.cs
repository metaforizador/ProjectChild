using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Helper : MonoBehaviour
{
    // Make Helper static and destroy if script already exists
    private static Helper _instance; // **<- reference link to Helper
    public static Helper Instance { get { return _instance; } }

    private void Awake() {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    // Writing complete method
    public delegate void WritingComplete();
    private float writeSpeed = 0.05f;
    private float writeSpeedLittlePause = 0.3f;

    /// <summary>
    /// Writes out text 1 letter at a time.
    /// </summary>
    /// <param name="textToWrite">Text to write on the text view</param>
    /// <param name="textView">Text view where to write</param>
    /// <param name="methodToCall">Method to call after the writing is complete</param>
    public void WriteOutText(string textToWrite, TextMeshProUGUI textView, WritingComplete methodToCall) {
        StartCoroutine(TypeSentence(textToWrite, textView, methodToCall));
    }

    private IEnumerator TypeSentence(string textToWrite, TextMeshProUGUI textView, WritingComplete methodToCall) {
        textView.text = "";
        foreach (char letter in textToWrite.ToCharArray()) {
            textView.text += letter;

            // If all the letters are written, sentence is complete
            if (textView.text.Length == textToWrite.Length)
                methodToCall();

            float speedToWrite = this.writeSpeed;

            // If letter is something which requires little "pause", wait a little bit longer
            if (letter.Equals(',') || letter.Equals('.') || letter.Equals('?') || letter.Equals('!'))
                speedToWrite = this.writeSpeedLittlePause;

            yield return new WaitForSeconds(speedToWrite);
        }
    }
}
