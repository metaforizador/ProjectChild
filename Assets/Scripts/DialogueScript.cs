using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class DialogueScript : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI questionView;

    [SerializeField]
    private GameObject questionObject, answerListView, answersObject;

    [SerializeField]
    private Button answerButtonPrefab;

    [SerializeField]
    private float answersYStartPosition;

    public LeanTweenType tweenType;

    private string question;

    void Start() {
        // Create an array for the answers
        Answer[] answers = new Answer[0];

        XMLDialogueParser data = XMLDialogueParser.Load(Path.Combine(Application.dataPath, "Resources/XMLFiles/Dialogues.xml"));
        foreach (DialogueFile o in data.questions) {
            question = o.questionText;
            answers = o.answers;
        }

        // Hide panels
        questionObject.transform.localScale = new Vector3(0, 0, 0);
        answersObject.transform.LeanMoveLocalY(answersYStartPosition, 0f);

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
        LeanTween.scale(questionObject, new Vector3(1, 1, 1), 0.5f).setEase(tweenType).setOnComplete(writeOutQuestion);
    }

    void writeOutQuestion() {
        Helper.Instance.WriteOutText(question, questionView, ShowAnswers);
    }

    void AnswerClicked() {
        Debug.Log("You have clicked the button!");
    }

    void ShowAnswers() {
        LeanTween.moveLocalY(answersObject, 0, 0.5f).setEase(tweenType);
    }
}
