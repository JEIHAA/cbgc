using UnityEngine;
public class Compass : MonoBehaviour
{
    const float dirArrowDistance = 0.9f;
    [SerializeField]
    SpriteRenderer campFireSR;
    SpriteRenderer campFireCompass, campFireDir;
    [SerializeField]
    Sprite[] dirSprite;
    // Start is called before the first frame update
    void Start() { MakeCompass(); }
    void Update() { CompassSet(); }
    void MakeCompass()
    {
        //make compass
        GameObject tmp = new GameObject("Compass");
        tmp.transform.SetParent(transform);
        campFireCompass = tmp.AddComponent<SpriteRenderer>();
        //make compass dir
        tmp = new GameObject("Compass_Dir");
        tmp.transform.SetParent(transform);
        campFireDir = tmp.AddComponent<SpriteRenderer>();
        //set sorting order
        campFireCompass.sortingOrder = 4;
        campFireDir.sortingOrder = 4;
    }
    // Update is called once per frame
    
    void CompassSet()
    {
        //to campfire dir
        var compassPos = -transform.position;
        //close to campfire
        if (Mathf.Abs(compassPos.x) > 25 || Mathf.Abs(compassPos.y) > 12)
        {
            //obj on
            campFireDir.gameObject.SetActive(true);
            campFireCompass.gameObject.SetActive(true);

            //compass pos set
            if (Mathf.Abs(compassPos.x) * 10 > Mathf.Abs(compassPos.y) * 19)
                compassPos = compassPos / Mathf.Abs(compassPos.x) * 19;
            else
                compassPos = compassPos / Mathf.Abs(compassPos.y) * 10;
            //compass dir pos & sprite set
            if (Mathf.Abs(compassPos.x) > 15)
            {
                if (compassPos.x < -15) campFireDir.sprite = dirSprite[2];
                if (compassPos.x > 15) campFireDir.sprite = dirSprite[3];
            }
            else
            {
                if (compassPos.y > 8) campFireDir.sprite = dirSprite[0];
                if (compassPos.y < -8) campFireDir.sprite = dirSprite[1];
            }
            campFireCompass.transform.localPosition = compassPos * dirArrowDistance;
            campFireDir.transform.localPosition = compassPos;
            campFireCompass.sprite = campFireSR.sprite;
        }
        //too far (off compass)
        else
        {
            campFireDir.gameObject.SetActive(false);
            campFireCompass.gameObject.SetActive(false);
        }
    }
}
