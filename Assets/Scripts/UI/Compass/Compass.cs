﻿using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class Compass : MonoBehaviour
{
    public RawImage CompassImage;
    public Transform Player;
    public TextMeshProUGUI CompassDirectionText;

    #region test
    Dictionary<int, string> displayAngle = new Dictionary<int, string>();

    private void Start()
    {
        displayAngle.Add(0, "N");
        displayAngle.Add(360, "N");
        displayAngle.Add(45, "NE");
        displayAngle.Add(90, "E");
        displayAngle.Add(135, "SE");
        displayAngle.Add(180, "S");
        displayAngle.Add(225, "SW");
        displayAngle.Add(270, "W");
        displayAngle.Add(315, "NW");
    }
    #endregion

    public void Update()
    {
        CompassImage.uvRect = new Rect(Player.localEulerAngles.y / 360, 0, 1, 1);

        #region test
        Vector3 forward = Player.transform.forward;

        forward.y = 0;

        float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
        headingAngle = 5*Mathf.RoundToInt((headingAngle/5))+5;

        if (headingAngle > 360)
        {
            headingAngle -= 360;
        }


        if (displayAngle.ContainsKey((int)headingAngle))
            CompassDirectionText.text = displayAngle[(int)headingAngle];
        else
            CompassDirectionText.text = headingAngle.ToString();
           
        #endregion

    }
}