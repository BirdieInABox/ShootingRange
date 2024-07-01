//Author: Kim Effie Proestler
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IEventListener
{
    public int score;
    private int highscore;

    [SerializeField] //Scriptable Object of all equippable weapons
    private WeaponList weaponList;

    //floats for percentual calculation of remaining delay time
    /*private float delayPosition,
        delayLength;
    private bool onDelay = false;*/

    // Start is called before the first frame update
    void Start()
    {
        //Add this as listener to the event system
        EventManager.MainStatic.AddListener(this);
        //Reset ammonition count and UI
        ResetAmmo();
        //get old highscore
        highscore = PlayerPrefs.GetInt("MasterVolume");
        //Update UI
        UpdateHighscore();
    }

    /// <summary>
    /// Iterates through list of weapons,
    /// resets current ammo to ammo max,
    /// update UI for each weapon
    /// </summary>
    private void ResetAmmo()
    {
        int i = 0; //iterate through list
        foreach (var weapon in weaponList.list)
        {
            //reset ammo amount
            weapon.currentMagazine = weapon.magazineSize;
            //update UI at index of current weapon
            UpdateAmmo(i);
            i++;
        }
    }

    /// <summary>
    /// time-based calculation of the current percentual time left in the current weapon delay
    /// </summary>
    /* private void FixedUpdate()
     {
         if (onDelay) //if delay active
         {
             delayPosition -= Time.deltaTime;
             //Fill UI image by a percentatge equal to 1- time left / total delay
             UIManager.UIMainStatic.SetDelayBar(1-delayPosition / delayLength);
             if (delayPosition <= 0) //when delay done, deactivate onDelay
                 onDelay = false;
         }
     }*/

    /// <summary>
    /// Called upon EventSystem sending an event
    /// </summary>
    /// <param name="receivedEvent">the received event, including type and content</param>
    public void OnEventReceived(EventData receivedEvent)
    {
        //if received event is of type "TargetHit"
        if (receivedEvent.Type == EventTypes.TargetHit)
        {
            //updates score and gives audio feedback
            UpdateScore((int)receivedEvent.Data);
            PlayHitSound((int)receivedEvent.Data);
        }
        //if received event is of type "BulletShot"
        else if (receivedEvent.Type == EventTypes.BulletShot)
        {
            //update ammo in UI and give audio feedback
            UpdateAmmo((int)receivedEvent.Data);
            PlayGunshot((int)receivedEvent.Data);
            //Start delay for visual delay display
            //  UpdateDelay((int)receivedEvent.Data);
        }
        //if received event is of type "Reloaded"
        else if (receivedEvent.Type == EventTypes.Reloaded)
        {
            //Update ammo
            UpdateAmmo((int)receivedEvent.Data);
        }
        //if received event is of type "GameEnded"
        else if (receivedEvent.Type == EventTypes.GameEnded)
        {
            //resets all ammo to max
            ResetAmmo();
            //update highscore
            UpdateHighscore();
        }
        //if received event is of type "GameStarted"
        else if (receivedEvent.Type == EventTypes.GameStarted)
        {
            //Reset score
            score = 0;
            UpdateScore(0);
        }
    }

    /// <summary>
    /// Updates the high score, saves it locally and updates the UI
    /// </summary>
    private void UpdateHighscore()
    {
        //if new highscore has been reached
        if (score > highscore)
        {
            highscore = score;
        }
        //Update UI
        UIManager.UIMainStatic.SetHighscoreText(highscore);
        //Save highscore locally
        PlayerPrefs.SetInt("Highscore", highscore);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Updates the ui of ammo of weapon at position "index"
    /// </summary>
    /// <param name="index">index of the weapon</param>
    private void UpdateAmmo(int index)
    {
        //updates UI to current ammo count
        UIManager.UIMainStatic.SetAmmoText(index, weaponList.list[index].currentMagazine);
    }

    /// <summary>
    /// calculates delay duration by 10/weapon speed
    /// </summary>
    /// <param name="index">index of weapon</param>
    /* private void UpdateDelay(int index)
     {
         delayLength = 10 / weaponList.list[index].weaponSpeed;
         //reset current delay
         delayPosition = delayLength;
         onDelay = true;
     }*/

    /// <summary>
    /// Plays gunshot sound of weapon at position "index"
    /// </summary>
    /// <param name="index">index of weapon</param>
    private void PlayGunshot(int index)
    {
        //Plays sound via AudioManager instance
        AudioManager.AudioMainStatic.PlayGunshot(index);
    }

    /// <summary>
    /// Plays hit sound of target
    /// </summary>
    /// <param name="value"> points gained/lost </param>
    private void PlayHitSound(int value)
    {
        //Plays sound via AudioManager instance
        AudioManager.AudioMainStatic.PlayHitSound(value);
    }

    /// <summary>
    /// Updates the score and its UI element
    /// </summary>
    /// <param name="value">points gained/lost</param>
    private void UpdateScore(int value)
    {
        //Update score
        score += value;
        //Update UI
        UIManager.UIMainStatic.SetScoreText(score);
    }
}
