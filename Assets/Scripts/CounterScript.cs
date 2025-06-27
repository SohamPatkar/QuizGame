using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CounterScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI counter_text;
    private float count;
    [SerializeField]
    private Button btn_score_add;

    // Start is called before the first frame update
    void Start()
    {
        count = 0f;
        counter_text.text = "Count: " + count;
        btn_score_add.onClick.AddListener(PressedButton);
    }

    void PressedButton()
    {
        count += 1;
        counter_text.text = "Count: " + count;
    }
}
