using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ZoneManager : MonoBehaviour
    {
        [SerializeField] private List<TMP_Text> _textList;
        [SerializeField, Range(0,5)] private float _showTime, holdTime, _hideTime;

        private void Start()
        {
            _textList.ForEach(x => x.DOFade(0,0));
        }

        public void ShowZone(string zoneName)
        {
            _textList.ForEach(x => x.DOFade(1,_showTime).OnComplete(HideZone));
        }
    
        private void HideZone()
        {
            StartCoroutine(HideZoneTexts());
        }

        private IEnumerator HideZoneTexts()
        {
            yield return new WaitForSeconds(holdTime);
            _textList.ForEach(x => x.DOFade(0,_hideTime));
        }
    }
}
