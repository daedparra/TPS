using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneByName : MonoBehaviour
{

    [SerializeField]
    private string _SceneName;

    [SerializeField]
    private bool _LoadOnEnable;

    [SerializeField]
    private bool _Additive;

    private void OnEnable()
    {
        if(_LoadOnEnable)
        {
            LoadScene();
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene
        (
            _SceneName,
            _Additive ? LoadSceneMode.Additive : LoadSceneMode.Single
        );
    }

}
