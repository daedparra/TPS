using UnityEngine;
using UnityEngine.UI;

public class GamePlayScreen : UIScreen
{

    [SerializeField] private Text _teamBlue;
    [SerializeField] private Text _teamRed;
    [SerializeField] private Text _teamYellow;

  
    private void OnEnable()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        _teamRed.color = GameManager.Instance.TeamColors[0];
        _teamYellow.color = GameManager.Instance.TeamColors[1];
        _teamBlue.color = GameManager.Instance.TeamColors[2];
    }

    private void Update()
    {
        _teamRed.text =  GameManager.Instance.GetRed().ToString();
        _teamBlue.text =  GameManager.Instance.GetBlue().ToString();
        _teamYellow.text =  GameManager.Instance.GetYellow().ToString();
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            UIScreenManager.Instance.ShowScreen<PauseScreen>();
        }
    }

}
