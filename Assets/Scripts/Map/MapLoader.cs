using UnityEngine;


namespace SceneLoad
{
    public class MapLoader : MonoBehaviour
    {
        JsonMapLoader sceneLoader;

        private void Start()
        {
            sceneLoader = new("TestMap");
            sceneLoader.Load();
        }

    }
}
