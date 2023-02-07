using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    public class DialogManager : MonoBehaviour
    {
        #region Singleton
        
        public static DialogManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        #endregion

        public TypeWriter TypeWriterText;
        public GameObject DialogUIGameObject;
        public Image PressButtonImage;

        private void Start()
        {
            DialogUIGameObject.SetActive(false);
        }

        public void ToggleDialog(bool setActive)
        {
            DialogUIGameObject.SetActive(setActive);
        }
    }
}