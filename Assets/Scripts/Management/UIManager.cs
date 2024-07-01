//Author: Kim Effie Proestler
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] //score text elements
    private TMP_Text scoreText,
        highscoreText;

    //UI element for visualization of weapon delay
    //[SerializeField] private Image delayFill;

    [SerializeField] //Game object with all weapon UI elements as children
    private GameObject[] weaponUis;

    //static instance of UIManager
    public static UIManager UIMainStatic;

    private void Awake()
    {
        //Create static instance of this, delete this if instance already exists
        if (UIMainStatic == null)
            UIMainStatic = this;
        else
            Destroy(this);
    }

    /// <summary>
    /// Set Delay bar's fill amount to percentage
    /// </summary>
    /// <param name="percentage">fill amount</param>
    /* public void SetDelayBar(float percentage)
     {
         delayFill.fillAmount = percentage;
     }*/

    /// <summary>
    /// displays the ammo amount of weapon at position "index"
    /// </summary>
    /// <param name="index">index of weapon</param>
    /// <param name="ammo">current ammo count</param>
    public void SetAmmoText(int index, int ammo)
    {
        string ammoText;
        //if magazine empty
        if (ammo <= 0)
        {
            //Tell player weapon is reloading
            ammoText = "Reloading...";
        }
        else //if magazine isn't empty, set string to ammo count
            ammoText = ammo.ToString();
        //Update UI for weapon at position "index"
        weaponUis[index].GetComponentInChildren<TMP_Text>().SetText(ammoText);
    }

    /// <summary>
    /// Updates UI for the score
    /// </summary>
    /// <param name="score">the new score value</param>
    public void SetScoreText(int score)
    {
        //Updates UI
        scoreText.SetText("Score: \n" + score.ToString());
    }

    /// <summary>
    /// Updates UI for the highscore
    /// </summary>
    /// <param name="score">the new highscore value</param>
    public void SetHighscoreText(int score)
    {
        //Updates UI
        highscoreText.SetText("Highscore: \n" + score.ToString());
    }
}
