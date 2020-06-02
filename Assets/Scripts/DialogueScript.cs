﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueScript : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI question;

    [SerializeField]
    private GameObject answerListView;

    [SerializeField]
    private Button answerButtonPrefab;

    private string questionText = "Hmm, so you lived before the war. What was it like, mom?";

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
        Helper.Instance.WriteOutText(questionText, question);
    }

    void AnswerClicked() {
        Debug.Log("You have clicked the button!");
    }
}
