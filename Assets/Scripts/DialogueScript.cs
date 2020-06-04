using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class DialogueScript : MonoBehaviour
{

    public enum AnswerType {Stoic, Nurturing, Idealistic, Nihilistic, Rational, Beligerent};

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
            Button button = Instantiate(answerButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            // Add button to the list and set the scale to 1 (parent.transform changes it to 0,6)
            button.transform.parent = answerListView.transform;
            button.transform.localScale = Vector3.one;

            // Set text to answer button
            button.GetComponentInChildren<TextMeshProUGUI>().text = answers[i].answerText;

            // Set on click listener to the button
            button.onClick.AddListener(() => AnswerClicked());
        }

        // Delay for testing purposes
        Invoke("DelayStart", 1f);
    }

    void DelayStart() {
        // Animate question
        LeanTween.scale(questionObject, new Vector3(1, 1, 1), 0.5f).setEase(tweenType).setOnComplete(WriteOutQuestion);
    }

    /// <summary>
    /// Starts writing out question.
    /// 
    /// This method is needed so that it can be passed as an OnComplete parameter to LeanTween.
    /// </summary>
    void WriteOutQuestion() {
        Helper.Instance.WriteOutText(question, questionView, ShowAnswers);
    }

    void AnswerClicked() {
        Debug.Log("You have clicked the button!");
    }

    /// <summary>
    /// Moves answers so that user can see them.
    /// </summary>
    void ShowAnswers() {
        LeanTween.moveLocalY(answersObject, 0, 0.5f).setEase(tweenType);
    }
}
