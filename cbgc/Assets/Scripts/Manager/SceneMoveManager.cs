using System;
using UnityEngine;
public class SceneMoveManager : MonoBehaviour
{
    public UnityEngine.UI.Button moveButton;
    public enum SceneName
    {
        Intro,
        Title,
        Field,
        GameOver,
        Scene,
        ETC
    }
    public SceneName moveScene;
    static SceneName nowScene;
    public float moveDelay = 0;
    public static SceneName NowScene { get { return nowScene; } }
    //��ư�� ����
    private void Start()
    {
        moveButton?.onClick.AddListener(LoadScene);
        if (moveDelay > 0) Invoke("LoadScene", moveDelay);
    }
    //�μ� string ( string -> SceneName ��ȯ �������� ���� )
    public void LoadScene(string name) { if (!Enum.TryParse(name, out moveScene)) Debug.LogWarning("�ش� ���� ���� : " + name); else LoadScene(); }
    //�μ� SceneName
    public void LoadScene(SceneName name) { moveScene = name; LoadScene(); }
    //�μ� ���� ( ���� �Ҵ�� �̸����� �̵� )
    public void LoadScene()
    {
        string sceneName = moveScene.ToString();
        //�̸��� ���� �� �ֳ� Ȯ��
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            nowScene = moveScene;
            Debug.Log("�� �̵� �� : " + sceneName);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
        //������ ��� ( �̵� �� �� )
        else Debug.LogWarning("�ش� ���� ���� : " + sceneName);
    }
}