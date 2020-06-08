using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public enum WordsType { Stoic, Nurturing, Idealistic, Nihilistic, Rational, Beligerent };

public class DialogueScript : MonoBehaviour {

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
    private List<Button> answerButtons = new List<Button>();

    void OnEnable() {
        // Hide panels
        questionObject.transform.localScale = new Vector3(0, 0, 0);
        answersObject.transform.LeanMoveLocalY(answersYStartPosition, 0f);
    }

    public void ShowDialogue() {
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
            answerButtons.Add(button);

            // Add button to the list and set the scale to 1 (parent.transform changes it to 0,6)
            button.transform.parent = answerListView.transform;
            button.transform.localScale = Vector3.one;

            // Set text to answer button
            button.GetComponentInChildren<TextMeshProUGUI>().text = text;

            // Set on click listener to the button
            button.onClick.AddListener(() => AnswerClicked(type));
        }

        // Delay a tiny bit to work around the initial fps hiccup (HOPEFULLY TEMPORARY SOLUTION)
        Invoke("ShowQuestion", 0.5f);
    }

    private void ShowQuestion() {
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
    /// Shows reply and increases stats when answer is clicked.
    /// </summary>
    /// <param name="type">WordsType of the reply to show</param>
    private void AnswerClicked(WordsType type) {
        // Show random reply
        reply = XMLDialogueParser.GetRandomReply(type);
        LeanTween.moveLocalY(answersObject, answersYStartPosition, 0.5f).
            setEase(tweenType).
            setOnComplete(() => Helper.Instance.WriteOutText(reply, questionView,       // Write out child talk
            () => Invoke("CloseDialogue", 1)));                                         // Invoke close dialogue on complete

        // Increase stats
        PlayerStats p = PlayerStats.Instance;
        int amount = 1;
        string statText = "";
        switch (type) {
            case WordsType.Stoic:
                p.IncreaseArmor(amount);
                p.IncreaseResistance(amount);
                statText = "Armor & Resistance";
                break;
            case WordsType.Nurturing:
                p.IncreaseMaxHp(amount);
                p.IncreaseShieldRegen(amount);
                statText = "Maximum health & Shield regen";
                break;
            case WordsType.Idealistic:
                p.IncreaseAttackSpd(amount);
                p.IncreaseFireRate(amount);
                statText = "Attack speed & Fire rate";
                break;
            case WordsType.Nihilistic:
                p.IncreaseDodge(amount);
                statText = "Dodge";
                break;
            case WordsType.Rational:
                p.IncreaseCritical(amount);
                statText = "Critical";
                break;
            case WordsType.Beligerent:
                p.IncreaseMovementSpd(amount);
                statText = "Movement speed";
                break;
        }

        CanvasMaster.Instance.ShowStatGain(statText);
    }

    /// <summary>
    /// Closes dialogue after child has replied.
    /// </summary>
    private void CloseDialogue() {
        LeanTween.scale(questionObject, new Vector3(0, 0, 0), 0.5f).
            setEase(tweenType).
            setOnComplete(ResetValues);
    }

    private void ResetValues() {
        // Reset question text
        questionView.text = "";

        // Remove buttons
        foreach (Button btn in answerButtons) {
            Destroy(btn.gameObject);
        }
        answerButtons.Clear();
    }
}
