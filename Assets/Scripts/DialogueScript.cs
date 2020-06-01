using UnityEngine;
using TMPro;

public class DialogueScript : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI question;

    [SerializeField]
    private GameObject answerListView, answerButtonPrefab;
    
    void Start() {
        for (int i = 0; i < 5; i++) {
            GameObject button = Instantiate(answerButtonPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            // Add button to the list and set the scale to 1 (parent.transform changes it to 0,6)
            button.transform.parent = answerListView.transform;
            button.transform.localScale = Vector3.one;

            // Set text to answer button
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Answer: " + i;
        }
    }

    void Update() {
        
    }
}
