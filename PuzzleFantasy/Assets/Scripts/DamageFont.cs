using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DamageFont : MonoBehaviour {

    public void FontAniEnd()
    {
        Destroy(gameObject);
    }

    public void SetText(int damage, Color color)
    {
        Text text = GetComponent<Text>();

        if (text != null)
        {
            text.text = damage.ToString();
            text.color = color;
        }
    }
}
