using Common;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;
using static UnityEngine.Rendering.DebugUI;

namespace GameUI
{
    public enum EGender
    {
        Male,
        Female,
    }

    public class CustomizingMenu : UIHUD
    {
        [Header("Set UI Slot")]
        [SerializeField] List<Transform> slotRoots;

        [Header("Set Gender")]
        [SerializeField] TMP_Dropdown genderDropDown;
        [SerializeField] SkinnedMeshRenderer modelHead;
        [SerializeField] SkinnedMeshRenderer modelTop;
        [SerializeField] SkinnedMeshRenderer modelBottom;

        [Header("Set Root")]
        [SerializeField] SkinnedMeshRenderer eyebrowRoot;
        [SerializeField] SkinnedMeshRenderer eyeRoot;
        [SerializeField] SkinnedMeshRenderer mouthRoot;
        [SerializeField] SkinnedMeshRenderer facehairRoot;
        [SerializeField] SkinnedMeshRenderer hairRoot;

        [Header("FacialHair Object")]
        [SerializeField] GameObject FacialHairObject;

        readonly int[] malePartsCount = { 5, 5, 5, 8, 14 };
        readonly int[] femalePartsCount = { 5, 5, 5, 0, 14 };

        List<GameObject> slots = new();
        int gender;

        public void Awake()
        {
            gender = 0;
            SetSlot();
        }

        void SetSlot()
        {
            if (slots.Count != 0)
            {
                for (int i = 0; i < slots.Count; i++)
                {
                    Destroy(slots[i]);
                }
                slots.Clear();
            }

            for (int i = 0; i < (int)CustomizationType.COUNT; i++)
            {
                var type = (CustomizationType)i;

                for (int j = 0; j < (gender == 0 ? malePartsCount[i] : femalePartsCount[i]); j++)
                {
                    GameObject CustomizingSlotPrefab = Resources.Load<GameObject>(nameof(CustomizingSlot));
                    var newGO = Instantiate(CustomizingSlotPrefab, slotRoots[i]);
                    if (newGO.TryGetComponent<CustomizingSlot>(out var slot))
                    {
                        var firstFormat = ((EGender)gender).ToString();
                        var secondFormat = i == 3 ? $"" : $"{firstFormat[0]}_";
                        var index = (i == 3 || i == 4) ? j + 1 : j;
                        var id = $"{firstFormat}[{secondFormat}{type}{index}]";

                        slot.SlotInit(type, id, j + 1);
                        slot.OnClickAction = () => ApplyMesh(type, id);
                    }
                    slots.Add(newGO);
                }
            }
        }

        void ApplyMesh(CustomizationType type, string id)
        {
            if (gender == 1 && type == CustomizationType.facialHair_) return;
            
            var value = Manager.Resource.Load<Mesh>(id);
            Manager.UserData.GetUserData<UserPlayerData>().SetID(type, id);
            switch (type)
            {
                case CustomizationType.eyebrows:
                    eyebrowRoot.sharedMesh = value;
                    break;
                case CustomizationType.eyes:
                    eyeRoot.sharedMesh = value;
                    break;
                case CustomizationType.mouth:
                    mouthRoot.sharedMesh = value;
                    break;
                case CustomizationType.facialHair_:
                    facehairRoot.sharedMesh = value;
                    break;
                case CustomizationType.hair_:
                    hairRoot.sharedMesh = value;
                    break;
            }
        }

        void InitMesh()
        {
            for (int i = 0; i < (int)CustomizationType.COUNT; i++)
            {
                var type = (CustomizationType)i;
                var firstFormat = ((EGender)gender).ToString();
                var secondFormat = i == 3 ? $"" : $"{firstFormat[0]}_";
                var index = (i == 3 || i == 4) ? "1" : "0";
                var id = $"{firstFormat}[{secondFormat}{type}{index}]";

                Debug.Log(id);
                ApplyMesh(type, id);
            }
        }

        #region Button Event
        public void OnClickEmptyOption(int type)
        {
            Manager.UserData.GetUserData<UserPlayerData>().SetID((CustomizationType)type, "null");

            switch ((CustomizationType)type)
            {
                case CustomizationType.eyebrows:
                    eyebrowRoot.sharedMesh = null;
                    break;
                case CustomizationType.facialHair_:
                    facehairRoot.sharedMesh = null;
                    break;
                case CustomizationType.hair_:
                    hairRoot.sharedMesh = null;
                    break;
            }
        }
        public void ChangedGender()
        {
            gender = genderDropDown.value;
            Manager.UserData.GetUserData<UserPlayerData>().SetGender(gender);

            FacialHairObject.SetActive(gender == 0);
            facehairRoot.enabled = gender == 1 ? false : true;

            var curGender = ((EGender)gender).ToString();
            modelHead.sharedMesh = Manager.Resource.Load<Mesh>($"{curGender}[{curGender[0]}_Head]");
            modelTop.sharedMesh = Manager.Resource.Load<Mesh>($"{curGender}[{curGender[0]}_TopBody]");
            modelBottom.sharedMesh = Manager.Resource.Load<Mesh>($"{curGender}[{curGender[0]}_BottomBody]");

            Manager.UserData.GetUserData<UserPlayerData>().SetGender(gender);

            InitMesh();
            SetSlot();
        }

        public void OnClickStartButton() => Manager.Scene.LoadScene(Define.SceneType.GameScene);
        #endregion
    }
}