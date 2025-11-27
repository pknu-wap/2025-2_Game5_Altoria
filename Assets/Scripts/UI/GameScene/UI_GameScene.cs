using Common;
using GameInteract;
using GameItem;
using TMPro;
using UnityEngine;


namespace GameUI
{
    public class UI_GameScene : UIHUD
    {
        [SerializeField] GameObject interaction;
        [SerializeField] TextMeshProUGUI interactText;

        PlayerInteractComponent playerInteract;

        public override bool Init()
        {
            if (!base.Init()) return false;

            InitializePrompt();
            return true;
        }

        public void OnClickMenu()
        {
            Manager.UI.ShowPopup<MainMenuPopUp>();
        }
        void InitializePrompt()
        {
            SetPromptActive(false);
            playerInteract = GameObject.FindAnyObjectByType<PlayerInteractComponent>();

            if (playerInteract == null) return;

            playerInteract.OnInteractableNearby += SetPromptActive;
            SetPromptActive(playerInteract.HasNearbyInteractable);
        }
        string GenerateMessage(MonoBehaviour target)
        {
            if(target is CollectInteractComponent)
                return "채집하기";

            if (target is UpgradeInteractComponent)
                return "강화하기";

            if (target is CraftInteractComponent)
                return "제작하기";

            if (target is FishInteractComponent)
                return "낚시하기";

            if (target is InteractableAnimalComponent)
                return "돌봐주기";

            return "상호작용하기";
        }

        // 상호작용 문구 보이기 
        void SetPromptActive(bool active)
        {
            if (interaction != null)
                interaction.SetActive(active);

            if (!active)
                return;

            if (playerInteract == null)
                return;

            var target = playerInteract.CurrentTarget as MonoBehaviour;
            if (target == null)
                return;

            interactText.text = GenerateMessage(target);
        }

    }
}
