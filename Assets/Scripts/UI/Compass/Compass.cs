using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class Compass : MonoBehaviour
{
    public GameObject PrefabIcon;
    List<CompassMarker> compassMarkers = new List<CompassMarker>();

    public RawImage CompassImage;
    public Transform Player;
    public TextMeshProUGUI CompassDirectionText;

    float _compassUnit;
    public List<CompassMarker> markers = new List<CompassMarker>();

    Dictionary<int, string> displayAngle = new Dictionary<int, string>();

    private void Start()
    {
        #region test
        displayAngle.Add(0, "N");
        displayAngle.Add(360, "N");
        displayAngle.Add(45, "NE");
        displayAngle.Add(90, "E");
        displayAngle.Add(135, "SE");
        displayAngle.Add(180, "S");
        displayAngle.Add(225, "SW");
        displayAngle.Add(270, "W");
        displayAngle.Add(315, "NW");
        #endregion
        _compassUnit = CompassImage.rectTransform.rect.width / 360;

        foreach (CompassMarker item in markers)
        {
            AddCompassMarker(item);
        }

    }

    public void Update()
    {
        CompassImage.uvRect = new Rect(Player.localEulerAngles.y / 360, 0, 1, 1);

        foreach (CompassMarker marker in compassMarkers)
        {
            marker.image.rectTransform.anchoredPosition = GetPosOnCompass(marker);
        }

        #region test
        Vector3 forward = Player.transform.forward;

        forward.y = 0;

        float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
        headingAngle = 5 * Mathf.RoundToInt((headingAngle / 5)) + 5;

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
    public void AddCompassMarker(CompassMarker marker)
    {
        GameObject newMarker = Instantiate(PrefabIcon, CompassImage.transform);
        marker.image = newMarker.GetComponent<Image>();
        marker.image.sprite = marker.icon;

        compassMarkers.Add(marker);
    }


    Vector2 GetPosOnCompass(CompassMarker marker)
    {
        Vector2 playerPos = new Vector2(Player.position.x, Player.position.z);
        Vector2 playerFwd = new Vector2(Player.forward.x, Player.forward.z);

        float angle = Vector2.SignedAngle(marker.position - playerPos, playerFwd);

        return new Vector2(_compassUnit * angle, 0f);
    }
}