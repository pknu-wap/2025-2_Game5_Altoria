using GameUI;
using UnityEngine;
using UnityEngine.Analytics;

public class CharacterCustomizer : MonoBehaviour
{
    [Header("Set Model")]
    [SerializeField] SkinnedMeshRenderer head;
    [SerializeField] SkinnedMeshRenderer topBody;
    [SerializeField] SkinnedMeshRenderer bottomBody;
    [SerializeField] SkinnedMeshRenderer[] partsModels;

    void Awake()
    {
        SetModel();
        SetColor();
    }

    void SetModel()
    {
        var gender = ((EGender)Manager.UserData.GetUserData<UserPlayerData>().GetGender()).ToString();

        head.sharedMesh = Manager.Resource.Load<Mesh>($"{gender}[{gender[0]}_Head]");
        topBody.sharedMesh = Manager.Resource.Load<Mesh>($"{gender}[{gender[0]}_TopBody]");
        bottomBody.sharedMesh = Manager.Resource.Load<Mesh>($"{gender}[{gender[0]}_BottomBody]");

        for (int i = 0; i < (int)Define.CustomizationType.COUNT; i++)
        {
            if (gender == "Female" && i == 3)
            {
                partsModels[i].enabled = false;
                continue;
            }

            var id = Manager.UserData.GetUserData<UserPlayerData>().GetID((Define.CustomizationType)i);
            if (id == "null")
                partsModels[i].sharedMesh = null;
            else
                partsModels[i].sharedMesh = Manager.Resource.Load<Mesh>(id);

        }
    }

    void SetColor()
    {
        for(int i = 0; i < 6; i++)
        {
            ColorData colorData = Manager.UserData.GetUserData<UserPlayerData>().GetColor(i);
            if (i == 0)
            {
                head.material.SetColor(colorData.Name, colorData.Color);
                topBody.material.SetColor(colorData.Name, colorData.Color);
                bottomBody.material.SetColor(colorData.Name, colorData.Color);
            }
            else
                partsModels[i - 1].material.SetColor(colorData.Name, colorData.Color);
        }
    }
}
