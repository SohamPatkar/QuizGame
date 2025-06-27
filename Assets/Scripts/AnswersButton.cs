using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswersButton : MonoBehaviour
{
    [SerializeField] private Toggle leftButton, rightButton;
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
        score.text = "" + Player.playerInstance.GetCoinCount();
    }

    void LeftInteraction(bool isOn)
    {
        if (isOn)
        {
            wrongAnswer.SetActive(false);
            correctAnswer.SetActive(true);
            unanswered.SetActive(false);
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
            Player.playerInstance.SubtractScore(scoreToSubtract);
            score.text = "" + Player.playerInstance.GetCoinCount();
            voiceOver.Stop();
            unanswered.SetActive(false);
            wrongAnswer.SetActive(true);
            rightButton.enabled = false;
            GameManager.Instance.changeQuestionState(QuestionState.WRONG);
        }
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
