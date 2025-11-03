using GameUI;
using SceneLoad;
using System;
using System.Collections;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

namespace SceneLoade
{
    public class LoadingUI : UIPopUp
    {
        [Header("Progress")]
        [SerializeField] Image progressImage;
        [SerializeField] TextMeshProUGUI progressTxt;
        UnityEngine.AsyncOperation _asyncOperation;
        JsonMapLoader currentMapLoader;

        void Awake()
        {
            progressImage.fillAmount = 0;
            progressTxt.text = "0%";
        }

        public void StartLoding(JsonMapLoader loader)
        {
            currentMapLoader = loader;
            currentMapLoader.OnProgress += UpdateProgress;
            currentMapLoader.OnSuccess += OnLoadSuccess;
            currentMapLoader.OnFailure += OnLoadFailure;
        }

        void UpdateProgress(float progress)
        {
            Debug.Log($"{progress}");
            progressImage.fillAmount = progress;
            progressTxt.text = $"{(int)(progress * 100)}%";
        }

        void OnLoadSuccess()
        {
            progressImage.fillAmount = 1f;
            progressTxt.text = "100%";
            EndLoading();
        }

        void OnLoadFailure()
        {
            Debug.LogError("Loading failed!");
            EndLoading();
        }

        void EndLoading()
        {
            currentMapLoader.OnProgress -= UpdateProgress;
            currentMapLoader.OnSuccess -= OnLoadSuccess;
            currentMapLoader.OnFailure -= OnLoadFailure;
            Manager.UI.ClosePopup(this);
        }
    }
}

