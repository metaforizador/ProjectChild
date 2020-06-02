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


    // Write out text
    private TextMeshProUGUI textView;
    private string textToWrite;

    public void WriteOutText(string textToWrite, TextMeshProUGUI textView) {
        this.textToWrite = textToWrite;
        this.textView = textView;

        StartCoroutine(TypeSentence());
    }

    IEnumerator TypeSentence() {
        textView.text = "";
        foreach (char letter in textToWrite.ToCharArray()) {
            textView.text += letter;

            // If all the letters are written, sentence is complete
            //if (textView.text.Length == textToWrite.Length)

            float writeSpeed = 0.05f;

            // If letter is something which requires little "pause", wait a little bit longer
            if (letter.Equals(',') || letter.Equals('.') || letter.Equals('?') || letter.Equals('!'))
                writeSpeed = 0.3f;

            yield return new WaitForSeconds(writeSpeed);
        }
    }
}
