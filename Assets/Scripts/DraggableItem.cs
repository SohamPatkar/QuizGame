using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class DraggableItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Image item;
    [HideInInspector] public Transform parentSlot;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private GameObject activityPanel;
    [SerializeField] private int scoreToAdd, scoreToSubtract;
    [SerializeField] private AudioSource voiceOver;
    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Dragging");
        parentSlot = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        if (item)
        {
            item.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Dragging");
        transform.SetParent(parentSlot);
        if (item)
        {
            item.raycastTarget = true;
        }
        voiceOver.Stop();
        GameManager.Instance.changeQuestionState(QuestionState.CORRECT);
        UpdateScore();
        StartCoroutine(VideoLoadCoolDown());
    }

    void LoadVideo()
    {
        GameManager.Instance.PlayVideo();
    }

    IEnumerator VideoLoadCoolDown()
    {
        yield return new WaitForSeconds(3f);
        LoadVideo();
        activityPanel.SetActive(false);
    }

    void UpdateScore()
    {
        Player.playerInstance.AddScore(scoreToAdd);
        score.text = "" + Player.playerInstance.GetCoinCount();
    }
}
