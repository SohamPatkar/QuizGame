using System;
using System.IO;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public enum QuestionState
{
    UNANSWERED,
    CORRECT,
    WRONG
}

[Serializable]
public class QuestionsAnswers
{
    public int questionID;
    public string question;
    public AudioClip questionVoiceOver;
    public string referenceVideo;
    public QuestionState questionState = QuestionState.UNANSWERED;

    public void SetAnswerState(QuestionState answerState)
    {
        questionState = answerState;
    }

    public AudioClip GetAudioClip()
    {
        return questionVoiceOver;
    }
}

public class GameManager : MonoBehaviour
{
    private string introVideo = Path.Combine(Application.streamingAssetsPath, "S1_1.mp4");
    private string[] videos =
    {
        Path.Combine(Application.streamingAssetsPath, "S3_for OST_2.mp4"),
        null,
        Path.Combine(Application.streamingAssetsPath, "S5 Q4 Checked_new.mp4"),
        Path.Combine(Application.streamingAssetsPath, "S5_Q4_ignitionCondition.mp4"),
        Path.Combine(Application.streamingAssetsPath, "S5_Q4 parking brake.mp4"),
        Path.Combine(Application.streamingAssetsPath, "S5_Q4 emergencyswitch.mp4"),
        Path.Combine(Application.streamingAssetsPath, "S5_Q4_positionofEGSL.mp4"),
        Path.Combine(Application.streamingAssetsPath, "S8_2_OST.mp4"),
        Path.Combine(Application.streamingAssetsPath, "S8_5_cleaning with brush.mp4"),
        Path.Combine(Application.streamingAssetsPath, "S5_Q4_cleaning_done.mp4"),
        null,
        null
    };

    [SerializeField] private QuestionsAnswers[] questionsAnswers;
    [SerializeField] private GameObject[] questionPanels;
    [SerializeField] private GameObject videoPanel;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private AudioSource voiceOverSource, sfxSource, backgroundMusicSource;
    [SerializeField] private TextMeshProUGUI questionField, timerText;
    [SerializeField] private BackgroundMusicManager[] music;
    private float timer = 15f;
    private float timeRemaining;
    private int QuestionCount = 0;
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayIntroVideo();
        timeRemaining = timer * 60;
        StartCoroutine(UpdateTimer());
    }

    void UpdateTimerText()
    {
        int minutesLeft = Mathf.FloorToInt(timeRemaining / 60);
        int secondsLeft = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"{minutesLeft:D2}:{secondsLeft:D2}";
    }

    void PlayIntroVideo()
    {
        if (introVideo == null)
        {
            PlayVideo();
            return;
        }

        videoPanel.SetActive(true);
        videoPlayer.url = introVideo;
        videoPlayer.frame = 0;
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnIntroVideoEnd;
    }

    void OnIntroVideoEnd(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= OnIntroVideoEnd;
        PlayVideo();
    }

    public void PlayVideo()
    {
        if (videos[QuestionCount] == null || QuestionCount > videos.Length - 1)
        {
            OnVideoEnd(videoPlayer);
            return;
        }

        videoPlayer.frame = 0;
        videoPanel.SetActive(true);
        videoPlayer.url = videos[QuestionCount];
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
        videoPanel.SetActive(false);
        LoadQuestion(QuestionCount);
    }

    AudioClip GetSfxSound(SoundType soundType)
    {
        foreach (BackgroundMusicManager musicElement in music)
        {
            if (musicElement.soundType == soundType)
            {
                return musicElement.aClip;
            }
        }

        return null;
    }

    public void changeQuestionState(QuestionState questionState)
    {
        if (questionState == QuestionState.WRONG)
        {
            sfxSource.clip = GetSfxSound(SoundType.WRONG);
            sfxSource.Play();
            return;
        }
        else
        {
            sfxSource.clip = GetSfxSound(SoundType.RIGHTANSWER);
            sfxSource.Play();
        }

        if (QuestionCount >= questionsAnswers.Length - 1)
        {
            return;
        }

        questionsAnswers[QuestionCount].SetAnswerState(questionState);
        QuestionCount++;
    }

    public void LoadQuestion(int question)
    {
        voiceOverSource.clip = questionsAnswers[question].GetAudioClip();
        voiceOverSource.Play();
        questionPanels[question].SetActive(true);
    }

    IEnumerator UpdateTimer()
    {
        while (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
            yield return null;
        }

        timeRemaining = 0;
        UpdateTimerText();
        Debug.Log("Timer Ended!");
    }
}
