using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public string playerName = "Player1";  // Cài đặt tên mặc định

    // Start is called before the first frame update
    void Start()
    {
        // Dùng DontDestroyOnLoad để giữ lại đối tượng khi chuyển cảnh
        DontDestroyOnLoad(gameObject);
    }

    // Hàm này sẽ trả về tên người chơi
    public string GetPlayerName()
    {
        return playerName;
    }
}
