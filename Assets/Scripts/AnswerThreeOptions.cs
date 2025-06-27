using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using TMPro;

public class AnswerThreeOptions : MonoBehaviour
{
    [SerializeField] private Toggle leftButton, rightButton, thirdButton;
    [SerializeField] private GameObject correctAnswer, unanswered, wrongAnswer;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private ParticleSystem coinsSpawner;
    [SerializeField] private int scoreToAdd, scoreToSubtract;
    [SerializeField] private AudioSource voiceOver;
    void Start()
    {
        InitializeUI();
    }

    void InitializeUI()
    {
        scoreToAdd = 100;
        scoreToSubtract = 50;
        leftButton.onValueChanged.AddListener(LeftInteraction);
        rightButton.onValueChanged.AddListener(RightInteraction);
        thirdButton.onValueChanged.AddListener(ThirdInteraction);
        score.text = "" + Player.playerInstance.GetCoinCount();
    }

    void LeftInteraction(bool isOn)
    {
        if (isOn)
        {
            wrongAnswer.SetActive(false);
            unanswered.SetActive(false);
            correctAnswer.SetActive(true);
            rightButton.enabled = false;
            coinsSpawner.Play();
            voiceOver.Stop();
            GameManager.Instance.changeQuestionState(QuestionState.CORRECT);
            UpdateScore();
            StartCoroutine(VideoLoadCoolDown());
        }
    }

    void RightInteraction(bool isOn)
    {
        if (isOn)
        {
            SubtractScore();
            unanswered.SetActive(false);
            wrongAnswer.SetActive(true);
            voiceOver.Stop();
            rightButton.enabled = false;
            GameManager.Instance.changeQuestionState(QuestionState.WRONG);
        }
    }

    void ThirdInteraction(bool isOn)
    {
        if (isOn)
        {
            SubtractScore();
            unanswered.SetActive(false);
            wrongAnswer.SetActive(true);
            voiceOver.Stop();
            thirdButton.enabled = false;
            GameManager.Instance.changeQuestionState(QuestionState.WRONG);
        }
    }

    void SubtractScore()
    {
        Player.playerInstance.SubtractScore(scoreToSubtract);
        score.text = "" + Player.playerInstance.GetCoinCount();
    }

    void LoadVideo()
    {
        GameManager.Instance.PlayVideo();
    }

    IEnumerator VideoLoadCoolDown()
    {
        yield return new WaitForSeconds(3f);
        LoadVideo();
        gameObject.SetActive(false);
    }

    void UpdateScore()
    {
        Player.playerInstance.AddScore(scoreToAdd);
        score.text = "" + Player.playerInstance.GetCoinCount();
    }

}
