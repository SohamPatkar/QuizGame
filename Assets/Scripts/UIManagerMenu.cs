using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;

    void Start()
    {
        playButton.onClick.AddListener(OnPlay);
    }

    void OnPlay()
    {
        SceneManager.LoadScene(1);
    }
}
