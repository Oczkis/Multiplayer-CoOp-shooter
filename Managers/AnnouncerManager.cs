using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AnnouncerManager : MonoBehaviour
{
    private static AnnouncerManager _instance;

    public static AnnouncerManager Instance { get { return _instance; } }

    public AudioSource audioSource;
    public AudioClip[] announcerCountdownClips;
    public AudioClip gameStartClip;
    public AudioClip gameStopClip;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        GameManager.ClientOnGameStarted += AnnouncerOnGameStarted;
        GameManager.ClientOnGameStoped += AnnouncerOnGameStopped;
    }

    private void OnDisable()
    {
        GameManager.ClientOnGameStarted -= AnnouncerOnGameStarted;
        GameManager.ClientOnGameStoped -= AnnouncerOnGameStopped;
    }

    public void AnnounceCountdown(int timeLeft)
    {
        if(timeLeft > 5 || timeLeft <= 0) { return; }

        audioSource.clip = announcerCountdownClips[timeLeft - 1];
        audioSource.Play();
    }

    private void AnnouncerOnGameStopped()
    {
        audioSource.clip = gameStopClip;
        audioSource.Play();
    }

    private void AnnouncerOnGameStarted()
    {
        audioSource.clip = gameStartClip;
        audioSource.Play();
    }

}
