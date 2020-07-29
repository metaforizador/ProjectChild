using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuCanvas : MonoBehaviour {

    [SerializeField]
    private Button continueButton, loadGameButton;

    // Speech
    [SerializeField]
    private AudioClip[] speeches;

    void Start() {
        bool saveExists = GameMaster.Instance.CheckIfSaveExists();
        continueButton.interactable = saveExists;
        loadGameButton.interactable = saveExists;

        Invoke("PlayRandomAudio", 0.5f);
    }

    private void PlayRandomAudio() {
        AudioClip sound = speeches[Random.Range(0, speeches.Length)];
        CanvasMaster.Instance.canvasSounds.PlaySound(sound);
    }

    public void NewGame() {
        // Number 1 should be the first scene in game after the main menu
        gameObject.SetActive(false);
        SceneManager.LoadScene(1);
    }

    public void Continue() {
        gameObject.SetActive(false);
        GameMaster.Instance.LoadGame();
    }

    public void ExitGame() {
        Application.Quit();
    }
}
