using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartScreenController : MonoBehaviour
{
    public Button startButton;
    public TMP_InputField input;

    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(startGame);
        input.text = "Player One";    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void startGame() {
        GameObject playerDataObj = new GameObject("PlayerData");
        PlayerData playerData = playerDataObj.AddComponent<PlayerData>();
        if (input.text.Length > 15) playerData.playerName = input.text.Substring(0, 15);
        else playerData.playerName = input.text;
        
        SceneManager.LoadScene("MainScene");
    }
}
