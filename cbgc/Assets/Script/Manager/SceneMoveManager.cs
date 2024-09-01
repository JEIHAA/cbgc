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
    //버튼에 연결
    private void Start()
    {
        moveButton?.onClick.AddListener(LoadScene);
        if (moveDelay > 0) Invoke("LoadScene", moveDelay);
    }
    //인수 string ( string -> SceneName 변환 가능한지 검증 )
    public void LoadScene(string name) { if (!Enum.TryParse(name, out moveScene)) Debug.LogWarning("해당 씬이 없음 : " + name); else LoadScene(); }
    //인수 SceneName
    public void LoadScene(SceneName name) { moveScene = name; LoadScene(); }
    //인수 없음 ( 현재 할당된 이름으로 이동 )
    public void LoadScene()
    {
        string sceneName = moveScene.ToString();
        //이름을 가진 씬 있나 확인
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            nowScene = moveScene;
            Debug.Log("씬 이동 중 : " + sceneName);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
        //없으면 경고 ( 이동 안 됨 )
        else Debug.LogWarning("해당 씬이 없음 : " + sceneName);
    }
}