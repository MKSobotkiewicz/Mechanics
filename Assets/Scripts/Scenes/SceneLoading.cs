using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Project.Resources
{
    public class SceneLoading : MonoBehaviour
    {
        public string SceneToLoad;
        AsyncOperation AsyncOperation;

        public void Start()
        {
            Application.backgroundLoadingPriority = ThreadPriority.High;
            AsyncOperation = SceneManager.LoadSceneAsync(SceneToLoad);
            AsyncOperation.allowSceneActivation = false;
        }

        public void Update()
        {
            if (AsyncOperation == null)
            {
                return;
            }
            Debug.Log(AsyncOperation.progress);
            if (AsyncOperation.progress >= 0.99f)
            {
                AsyncOperation.allowSceneActivation = true;
            }
        }
    }
}
