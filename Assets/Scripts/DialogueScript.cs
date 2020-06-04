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

    void Start() {
        // Create an array for the questions and answers
        List<Question> questions = new List<Question>();
        Answer[] answers = new Answer[0];

        XMLDialogueParser data = XMLDialogueParser.Load(Path.Combine(Application.dataPath, "Resources/XMLFiles/Dialogues.xml"));
        // Add all questions to list which belong to this area
        foreach (Question o in data.questions) {
            if (o.area.Equals(this.curArea)) {
                questions.Add(o);
            }
            question = o.questionText;
            answers = o.answers;
        }

        int questionIndex = 0;
        // If there are more than 1 questions in this area, randomize question
        if (questions.Count > 1) {
            questionIndex = Random.Range(0, questions.Count);   // If count is 5, random returns values between 0 and 4
        }

        // Set question text and answers
        Question q = questions[questionIndex];
        question = q.questionText;
        answers = q.answers;


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
