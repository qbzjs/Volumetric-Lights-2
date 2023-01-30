using System;
using System.Collections.Generic;
using Character.Camera;
using Character.State;
using DG.Tweening;
using Dialog;
using Kayak;
using UnityEngine;
using UnityEngine.Events;

public class DialogCreator : MonoBehaviour
{
    #region Enums

    [Serializable]
    private enum LaunchType
    {
        TriggerZone = 0,
        Method = 1
    }

    private enum DialogState
    {
        NotLaunched = 0,
        Showing = 1,
        Holding = 2,
        WaitingForInput = 3,
    }

    #endregion

    [SerializeField] private LaunchType _launchType;
    [SerializeField] private List<DialogStruct> _dialog;
    [SerializeField] private bool _canBeReplayed;
    [SerializeField, ReadOnly] private bool _hasEnded;
    [SerializeField] private bool _blockPlayerMovement, _blockCameraMovement;

    [Space(20), Header("Events")] public UnityEvent OnDialogLaunch = new UnityEvent();
    public UnityEvent OnDialogEnd = new UnityEvent();

    private int _dialogIndex;
    private float _currentDialogCooldown;
    private DialogState _currentDialogState = DialogState.NotLaunched;
    private GameplayInputs _gameplayInputs;

    private void Start()
    {
        _gameplayInputs = new GameplayInputs();
        _gameplayInputs.Enable();
        DialogManager.Instance.ToggleDialog(false);
    }

    private void Update()
    {
        if (_currentDialogState == DialogState.NotLaunched)
        {
            return;
        }

        CoolDownManagement();
    }

    private void OnTriggerEnter(Collider other)
    {
        KayakController kayakController = other.GetComponent<KayakController>();
        if (_launchType == LaunchType.TriggerZone && kayakController != null)
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
                    CheckForDialogEnd();
                    _dialogIndex++;
                    ShowDialog(_dialogIndex);
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

                    if (_dialogIndex < _dialog.Count - 1)
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
            FindObjectOfType<CharacterManager>().CurrentStateBase.CanCharacterMove = false;
        }

        if (_blockCameraMovement)
        {
            FindObjectOfType<CameraController>().CanMoveCameraMaunally = false;
        }

        //visual
        DialogManager.Instance.PressButtonImage.DOFade(0f, 0f);
        GameObject dialog = DialogManager.Instance.DialogUIGameObject;
        Vector3 scale = dialog.transform.localScale;
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
            DialogManager.Instance.TypeWriterText.Delay =  _dialog[index].TextShowTime / _dialog[index].Text.Length;
            StartCoroutine(DialogManager.Instance.TypeWriterText.ShowText());
        }
        else
        {
            DialogManager.Instance.TypeWriterText.DisplayText.text = _dialog[index].Text;
        }

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
        FindObjectOfType<CharacterManager>().CurrentStateBase.CanCharacterMove = true;
        FindObjectOfType<CameraController>().CanMoveCameraMaunally = true;
    }

    private void CheckForDialogEnd()
    {
        if (_dialogIndex >= _dialog.Count - 1)
        {
            EndDialog();
        }
    }

    private void DeactivateDialogObject()
    {
        DialogManager.Instance.ToggleDialog(false);
    }
}