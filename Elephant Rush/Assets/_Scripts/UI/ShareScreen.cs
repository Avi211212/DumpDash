using UnityEngine;
using UnityEngine.UI;

public class ShareScreen : MonoBehaviour
{
    [SerializeField] private GameOverState gameOverState;

    public GameObject shareScreenImage;
    public Text characterName;
    public Text playerName;
    public Text score;
    public Transform characterPosition;
}
