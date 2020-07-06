﻿using System.Collections;
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

    // Writing complete method
    public delegate void WritingComplete();
    private float writeSpeed = 0.05f;
    private float writeSpeedLittlePause = 0.3f;

    /// <summary>
    /// Writes out text 1 letter at a time.
    /// </summary>
    /// <param name="textToWrite">Text to write on the text view</param>
    /// <param name="textView">Text view where to write</param>
    /// <param name="methodToCall">Method to call after the writing is complete</param>
    public void WriteOutText(string textToWrite, TextMeshProUGUI textView, WritingComplete methodToCall) {
        StartCoroutine(TypeSentence(textToWrite, textView, methodToCall));
    }

    private IEnumerator TypeSentence(string textToWrite, TextMeshProUGUI textView, WritingComplete methodToCall) {
        textView.text = textToWrite;
        textView.maxVisibleCharacters = 0;

        for (int i = 1; i <= textToWrite.Length; i++) {
            textView.maxVisibleCharacters = i;
            char curLetter = textToWrite[i - 1];    // Get current character to know when to pause

            float speedToWrite = this.writeSpeed;

            // If letter is something which requires little "pause", wait a little bit longer
            if (curLetter.Equals(',') || curLetter.Equals('.') || curLetter.Equals('?') || curLetter.Equals('!'))
                speedToWrite = this.writeSpeedLittlePause;

            yield return new WaitForSeconds(speedToWrite);
        }

        // Call listener method when writing is complete
        methodToCall();
    }

    /// <summary>
    /// Checks if provided percentage is below randomized percentage.
    /// </summary>
    /// <param name="percentage">percentage to check</param>
    /// <returns>true if success</returns>
    public static bool CheckPercentage(float percentage) {
        // Randomize percentage
        int randomPercentValue = Random.Range(1, 101); // 101 since then it returns values from 1 to 100

        if (randomPercentValue <= percentage)
            return true;

        return false;
    }

    /// <summary>
    /// Creates an object and makes it a child of the parent.
    /// </summary>
    /// <param name="prefab">prefab to add</param>
    /// <param name="parent">parent to get transform from</param>
    /// <returns>parented child</returns>
    public GameObject CreateObjectChild(GameObject prefab, GameObject parent) {
        GameObject layout = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        // Set parent and fix scale
        layout.transform.SetParent(parent.transform);
        layout.transform.localPosition = Vector3.zero;
        layout.transform.localScale = Vector3.one;

        return layout;
    }

    /// <summary>
    /// Randomizes the order of an array.
    /// </summary>
    /// <typeparam name="T">typeof object</typeparam>
    /// <param name="array">array to randomize</param>
    public void RandomizeArrayOrder<T>(T[] array) {
        for (int i = 0; i < array.Length - 1; i++) {
            int rnd = Random.Range(i, array.Length);
            T tempGO = array[rnd];
            array[rnd] = array[i];
            array[i] = tempGO;
        }
    }

    /// <summary>
    /// Shows weapon stats in UI.
    /// </summary>
    /// <param name="holder">Object which holds the references to UI elements</param>
    /// <param name="weapon">weapon to get the stats from</param>
    public void SetupWeaponStats(WeaponStatHolder holder, WeaponSO weapon) {
        if (holder.name != null)
            holder.name.text = weapon.name;
        holder.type.text = weapon.weaponType.ToString();
        holder.damage.text = weapon.damagePerBullet.ToString();
        holder.bulletSpeed.text = weapon.bulletSpeed.ToString();
        holder.ammoSize.text = weapon.ammoSize.ToString();
        holder.rateOfFire.text = (60 / weapon.rateOfFire).ToString();   // Rounds per minute
    }

    /// <summary>
    /// Shows armor stats in UI.
    /// </summary>
    /// <param name="holder">Object which holds the references to UI elements</param>
    /// <param name="armor">armor to get the stats from</param>
    public void SetupArmorStats(ArmorStatHolder holder, ArmorSO armor) {
        if (holder.name != null)
            holder.name.text = armor.name;
        holder.decreaseShieldDelay.text = armor.decreaseShieldRecoveryDelay.ToString();
        holder.increaseShield.text = armor.increaseShield.ToString();
        holder.lowerOpponentsCritChance.text = armor.decreaseOpponentCriticalRate.ToString();
        holder.lowerOpponentsCritMultiplier.text = armor.decreaseOpponentCriticalMultiplier.ToString();
        holder.decreaseMovementSpeed.text = armor.reduceMovementSpeed.ToString();
        holder.decreaseStaminaRecoveryRate.text = armor.reduceStaminaRecoveryRate.ToString();
    }
}
