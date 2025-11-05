using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyBindingDatabase", menuName = "Game/Database/KeyBindingDatabase")]
public class KeyBindingDatabase : ScriptableObject
{
    [System.Serializable]
    public class KeyBind
    {
        public string actionName;   // 예: "앞으로 이동"
        public KeyCode defaultKey;  // 예: W
    }

    public List<KeyBind> keyBinds;
}
