using UnityEngine;


namespace SceneLoad
{
    public class TestMapLoader : MonoBehaviour
    {
        [SerializeField] int MaxTask=5;
        JsonMapLoader sceneLoader;

        private void Start()
        {
            sceneLoader = new("TestMap",MaxTask);
            sceneLoader.Load();
        }

    }
}
