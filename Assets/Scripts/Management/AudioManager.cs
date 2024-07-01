//Author: Kim Effie Proestler
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] //gameObject having all gunshot audio sources as children
    private Transform gunshots;

    [SerializeField] //gameObject having all hitSound audio sources as children
    private Transform hitSounds;

    //Static instance of AudioManager
    public static AudioManager AudioMainStatic;

    private void Awake()
    {
        //Create static instance of this, delete this if instance already exists
        if (AudioMainStatic == null)
            AudioMainStatic = this;
        else
            Destroy(this);
    }

    /// <summary>
    /// Play gunshot sound of weapon with index "index"
    /// </summary>
    /// <param name="index"> index or currently equipped weapon </param>
    public void PlayGunshot(int index)
    {
        //Find child at same index as current weapon and play its audio source
        gunshots.GetChild(index).GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// Play HitSound when target destroyed,
    /// positive sound when points gained positive
    /// negative sound when points gained negative
    /// </summary>
    /// <param name="value"> points gained from destroyed target </param>
    public void PlayHitSound(int value)
    {
        //Find child at index 0 for negative or
        //index 1 for positive
        //and plays the child's audio source
        hitSounds.GetChild((value <= 0) ? 0 : 1).GetComponent<AudioSource>().Play();
    }
}
