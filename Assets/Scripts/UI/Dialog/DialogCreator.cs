using System;
using System.Collections.Generic;
using Character;
using DG.Tweening;
using Dialog;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Dialog
{
    public class DialogCreator : MonoBehaviour
    {
        #region Enums

        [Serializable]
        private enum LaunchType
        {
            TriggerZone = 0,
            Method = 1
        }

        [Serializable]
        private enum DialogState
        {
            NotLaunched = 0,
            Showing = 1,
            Holding = 2,
            WaitingForInput = 3,
        }

        #endregion

        [Header("Parameters"), SerializeField] private LaunchType _launchType;
        [SerializeField] private List<DialogStruct> _dialog;
        [SerializeField] private bool _canBeReplayed;
        [SerializeField, ReadOnly] private bool _hasEnded;
        [SerializeField] private bool _blockPlayerMovement, _blockCameraMovement;

        [Space(20), Header("Events")] 
        public UnityEvent OnDialogLaunch = new UnityEvent();
        public UnityEvent OnDialogEnd = new UnityEvent();

        [Space(20), Header("References"), SerializeField] private CharacterManager _characterManager;
        [SerializeField] private CameraManager _cameraManager;

        [Header("Debug")]
        private int _dialogIndex;
        private float _currentDialogCooldown; 
        [SerializeField, ReadOnly] private DialogState _currentDialogState = DialogState.NotLaunched;
        private GameplayInputs _gameplayInputs;

        private void Start()
        {
            //OnPlayerDetected.AddListener(StartTrigger);
            
            _gameplayInputs = new GameplayInputs();
            _gameplayInputs.Enable();
            DialogManager.Instance.ToggleDialog(false);
        }

        private void OnDestroy()
        {
            //OnPlayerDetected.RemoveListener(StartTrigger);
        }

        protected void Update()
        {
            //base.Update();
            if (_currentDialogState == DialogState.NotLaunched)
            {
                return;
            }

            CoolDownManagement();
        }

        public void StartTrigger()
        {
            if (_launchType == LaunchType.TriggerZone && _currentDialogState == DialogState.NotLaunched)
            {
                if ((_hasEnded && _canBeReplayed) || (_hasEnded == false && _currentDialogState == DialogState.NotLaunched))
                {
                    LaunchDialog();
                }
            }
        }

        private void CoolDownManagement()
        {
            _currentDialogCooldown -= Time.deltaTime;

            if (_currentDialogCooldown > 0)
            {
                return;
            }

            switch (_currentDialogState)
            {
                case DialogState.Showing:
                    _currentDialogState = DialogState.Holding;
                    _currentDialogCooldown = _dialog[_dialogIndex].TextHoldTime;
                    break;
            
                case DialogState.Holding:
                    if (_dialog[_dialogIndex].SequencingTypeNext == SequencingType.Automatic)
                    {
                        _dialogIndex++;
                        CheckForDialogEnd();
                        if (_dialogIndex < _dialog.Count)
                        {
                            ShowDialog(_dialogIndex);
                        }
                    }
                    else
                    {
                        _currentDialogState = DialogState.WaitingForInput;
                        DialogManager.Instance.PressButtonImage.DOFade(1, 0.2f);
                    }
                    break;
            
                case DialogState.WaitingForInput:
                    if (_gameplayInputs.Boat.DialogSkip.triggered)
                    {
                        _dialogIndex++;
                        CheckForDialogEnd();

                        if (_dialogIndex < _dialog.Count)
                        {
                            ShowDialog(_dialogIndex);
                        }
                    }
                    break;
            }
        }

        public void LaunchDialog()
        {
            DialogManager.Instance.ToggleDialog(true);
            OnDialogLaunch.Invoke();

            _dialogIndex = 0;
            ShowDialog(_dialogIndex);

            if (_blockPlayerMovement)
            {
                _characterManager.CurrentStateBase.CanCharacterMove = false;
            }

            if (_blockCameraMovement)
            {
                _cameraManager.CanMoveCameraManually = false;
            }

            //visual
            DialogManager.Instance.PressButtonImage.DOFade(0f, 0f);
            GameObject dialog = DialogManager.Instance.DialogUIGameObject;
            Vector3 scale = Vector3.one;
            dialog.transform.localScale = Vector3.zero;
            dialog.transform.DOScale(scale, 0.25f);
        }

        private void ShowDialog(int index)
        {
            _currentDialogState = DialogState.Showing;
            _currentDialogCooldown = _dialog[index].TextShowTime;

            if (_dialog[index].ShowLetterByLetter)
            {
                DialogManager.Instance.TypeWriterText.FullText = _dialog[index].Text;
                DialogManager.Instance.TypeWriterText.DisplayText.color = _dialog[index].TextColor;
                DialogManager.Instance.TypeWriterText.Delay =  _dialog[index].TextShowTime / _dialog[index].Text.Length;
                StartCoroutine(DialogManager.Instance.TypeWriterText.ShowText());
            }
            else
            {
                DialogManager.Instance.TypeWriterText.DisplayText.text = _dialog[index].Text;
            }

            //visual
            DialogManager.Instance.TypeWriterText.transform.DOPunchScale(Vector3.one * _dialog[index].SizeEffect, 0.3f, 10, 0);
            DialogManager.Instance.PressButtonImage.DOFade(0, 0.1f);
        
            //audio
            SoundManager.Instance.PlayDialog(_dialog[index].Clip);
        }

        private void EndDialog()
        {
            OnDialogEnd.Invoke();
            _hasEnded = true;
            _currentDialogState = DialogState.NotLaunched;

            //visual
            GameObject dialog = DialogManager.Instance.DialogUIGameObject;
            dialog.transform.DOScale(Vector3.zero, 0.25f).OnComplete(DeactivateDialogObject);

            //booleans
            _characterManager.CurrentStateBase.CanCharacterMove = true;
            _cameraManager.CanMoveCameraManually = true;
        }

        private void CheckForDialogEnd()
        {
            if (_dialogIndex >= _dialog.Count)
            {
                EndDialog();
            }
        }

        private void DeactivateDialogObject()
        {
            DialogManager.Instance.ToggleDialog(false);
        }
    }
}