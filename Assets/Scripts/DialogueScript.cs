using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueScript : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI questionView;

    [SerializeField]
    private GameObject answerListView, answersObject;

    [SerializeField]
    private Button answerButtonPrefab;

    [SerializeField]
    private float answersYPosition;

    public LeanTweenType tweenType;

    private string question = "Hmm, so you lived before the war. What was it like, mom?";

    void Start() {
        for (int i = 0; i < 5; i++) {
            Button button = Instantiate(answerButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            // Add button to the list and set the scale to 1 (parent.transform changes it to 0,6)
            button.transform.parent = answerListView.transform;
            button.transform.localScale = Vector3.one;

            // Set text to answer button
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Answer: " + i;

            // Set on click listener to the button
            button.onClick.AddListener(() => AnswerClicked());
        }

        Helper.Instance.WriteOutText(question, questionView, ShowAnswers);
    }

    void AnswerClicked() {
        Debug.Log("You have clicked the button!");
    }

    void ShowAnswers() {
        LeanTween.moveLocalY(answersObject, answersYPosition, 0.5f).setEase(tweenType);
    }
}
