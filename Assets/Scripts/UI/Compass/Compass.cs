using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class Compass : MonoBehaviour
{
    public RawImage CompassImage;
    public Transform Player;
    public TextMeshProUGUI CompassDirectionText;

    public void Update()
    {
        CompassImage.uvRect = new Rect(Player.localEulerAngles.y / 360, 0, 1, 1);

        #region test
        //Vector3 forward = Player.transform.forward;

        //forward.y = 0;

        //float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
        //headingAngle = 5 * (Mathf.RoundToInt(headingAngle / 5.0f));

        //int displayangle;
        //displayangle = Mathf.RoundToInt(headingAngle);

        //switch (displayangle)
        //{
        //    case 0:
        //        //Do this
        //        CompassDirectionText.text = "N";
        //        break;
        //    case 360:
        //        //Do this
        //        CompassDirectionText.text = "N";
        //        break;
        //    case 45:
        //        //Do this
        //        CompassDirectionText.text = "NE";
        //        break;
        //    case 90:
        //        //Do this
        //        CompassDirectionText.text = "E";
        //        break;
        //    case 130:
        //        //Do this
        //        CompassDirectionText.text = "SE";
        //        break;
        //    case 180:
        //        //Do this
        //        CompassDirectionText.text = "S";
        //        break;
        //    case 225:
        //        //Do this
        //        CompassDirectionText.text = "SW";
        //        break;
        //    case 270:
        //        //Do this
        //        CompassDirectionText.text = "W";
        //        break;
        //    default:
        //        CompassDirectionText.text = headingAngle.ToString();
        //        break;
        //}
        #endregion

    }
}