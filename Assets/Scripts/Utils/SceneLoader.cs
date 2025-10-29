using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;

/// <summary>
/// timeScale을 1로 다시 설정하는 이유는 시간 정지 등 고려했습니다.
/// timeScale을 사용하지 않는다면 삭제하겠습니다.
/// </summary>
/// 
namespace SceneLoader
{


    public class SceneLoader
    {
        public void LoadScene(SceneType sceneType)
        {
            Time.timeScale = 1f;
            Manager.UserData.SaveAllUserData();
            SceneManager.LoadScene(sceneType.ToString());
        }

        public void ReLoadScene()
        {
            Debug.Log($"[{GetType()}] '{SceneManager.GetActiveScene().name}' scene loading");

            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public AsyncOperation LoadSceneAsync(SceneType sceneType)
        {
            Debug.Log($"[{GetType()}] '{sceneType}' scene async loading");

            Time.timeScale = 1f;
            return SceneManager.LoadSceneAsync(sceneType.ToString());
        }
    }
}
