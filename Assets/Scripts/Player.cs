using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int score;
    public static Player playerInstance { get; private set; }

    void Awake()
    {
        if (playerInstance == null)
        {
            playerInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetCoinCount()
    {
        return score;
    }

    public void AddScore(int score)
    {
        this.score += score;
    }

    public void SubtractScore(int score)
    {
        this.score -= score;
    }
}
