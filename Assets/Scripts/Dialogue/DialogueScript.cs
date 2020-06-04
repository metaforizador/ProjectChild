using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class DialogueScript : MonoBehaviour {

    public enum WordsType {Stoic, Nurturing, Idealistic, Nihilistic, Rational, Beligerent};

    [SerializeField]
    private TextMeshProUGUI questionView;

    [SerializeField]
    private GameObject questionObject, answerListView, answersObject;

    [SerializeField]
    private Button answerButtonPrefab;

    [SerializeField]
    private float answersYStartPosition;

    public LeanTweenType tweenType;

    private string curArea = "Level1";
    private string question;
    private string reply;

    void OnEnable() {
        // Hide panels
        questionObject.transform.localScale = new Vector3(0, 0, 0);
        answersObject.transform.LeanMoveLocalY(answersYStartPosition, 0f);
    }

    void Start() {
        // Set question text and answers
        Answer[] answers = new Answer[0];
        Question q = XMLDialogueParser.GetRandomQuestion(curArea);  // Load random question from xml
        question = q.questionText;
        answers = q.answers;

        // Loop through all the answers
        for (int i = 0; i < answers.Length; i++) {
            string text = answers[i].answerText;
            WordsType type = answers[i].answerType;

            Button button = Instantiate(answerButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            // Add button to the list and set the scale to 1 (parent.transform changes it to 0,6)
            button.transform.parent = answerListView.transform;
            button.transform.localScale = Vector3.one;

            // Set text to answer button
            button.GetComponentInChildren<TextMeshProUGUI>().text = text;

            // Set on click listener to the button
            button.onClick.AddListener(() => AnswerClicked(type));
        }

        // Delay for testing purposes
        Invoke("DelayStart", 1f);
    }

    private void DelayStart() {
        LeanTween.scale(questionObject, new Vector3(1, 1, 1), 0.5f).
            setEase(tweenType).
            setOnComplete(() => WriteOutChildTalking(question,                          // Write out child talk
            () => LeanTween.moveLocalY(answersObject, 0, 0.5f).setEase(tweenType)));    // Show answers on complete
    }

    /// <summary>
    /// Writes out the text when child talks.
    /// </summary>
    /// <param name="text">text to write</param>
    /// <param name="methodOnComplete">method to do after writing is complete</param>
    private void WriteOutChildTalking(string text, Helper.WritingComplete methodOnComplete) {
        Helper.Instance.WriteOutText(text, questionView, methodOnComplete);
    }

    /// <summary>
    /// Shows random reply when an answer is clicked.
    /// </summary>
    /// <param name="type">WordsType of the reply to show</param>
    private void AnswerClicked(WordsType type) {
        reply = XMLDialogueParser.GetRandomReply(type);
        LeanTween.moveLocalY(answersObject, answersYStartPosition, 0.5f).
            setEase(tweenType).
            setOnComplete(() => Helper.Instance.WriteOutText(reply, questionView,       // Write out child talk
            () => Invoke("CloseDialogue", 1)));                                         // Invoke close dialogue on complete
    }

    /// <summary>
    /// Closes dialogue after child has replied.
    /// </summary>
    private void CloseDialogue() {
        LeanTween.scale(questionObject, new Vector3(0, 0, 0), 0.5f).
            setEase(tweenType).
            setOnComplete(() => Destroy(gameObject));   // Destroy after dialogue is closed
    }
}
