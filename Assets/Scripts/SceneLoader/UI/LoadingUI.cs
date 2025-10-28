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

        private void Awake()
        {
            progressImage.fillAmount = 0;
            progressTxt.text = "0%";
        }

        public void StartLoding(SceneType sceneType)
        {
            StartCoroutine(Loading(sceneType));
        }

        IEnumerator Loading(SceneType sceneType)
        {
            _asyncOperation = Manager.Scene.LoadSceneAsync(sceneType);
            if (_asyncOperation == null)
            {
                Debug.LogError($"{GetType()} : {sceneType} async loading error");
                yield break;
            }

            _asyncOperation.allowSceneActivation = false;

            progressTxt.text = $"{((int)(progressImage.fillAmount * 100))}%";
            yield return new WaitForSeconds(0.5f);

            while (!_asyncOperation.isDone) // Loading
            {
                progressImage.fillAmount = _asyncOperation.progress < 0.5f ? 0.5f : _asyncOperation.progress;
                progressTxt.text = $"{((int)(progressImage.fillAmount * 100))}%";

                // End Load
                if (_asyncOperation.progress >= 0.9f)
                {
                    progressTxt.text = "100%";
                    _asyncOperation.allowSceneActivation = true;
                    EndLoding();
                    yield break;
                }

                yield return null;
            }

            if (_asyncOperation.progress >= 0.9f)
            {
                progressTxt.text = "100%";
                _asyncOperation.allowSceneActivation = true;
                EndLoding();
                yield break;
            }
        }

        void EndLoding()
        {
            Close();
        }
    }
}

