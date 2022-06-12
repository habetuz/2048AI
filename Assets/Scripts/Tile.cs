using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] Text valueText;
    [SerializeField] float speed;

    private static Dictionary<uint, (Color32, Color32)> Colors = new Dictionary<uint, (Color32, Color32)>()
    {
        {2, (new Color32(238, 228, 218, 255), new Color32(119, 110, 101, 255)) },
        {4, (new Color32(237, 224, 200, 255), new Color32(119, 110, 101, 255)) },
        {8, (new Color32(242, 177, 121, 255), new Color32(255, 255, 255, 255)) },
        {16, (new Color32(245, 149, 99, 255), new Color32(255, 255, 255, 255)) },
        {32, (new Color32(246, 124, 96, 255), new Color32(255, 255, 255, 255)) },
        {64, (new Color32(237, 207, 115, 255), new Color32(255, 255, 255, 255)) },
        {128, (new Color32(237, 207, 115, 255), new Color32(255, 255, 255, 255)) },
        {256, (new Color32(237, 204, 98, 255), new Color32(255, 255, 255, 255)) },
        {512, (new Color32(237, 200, 80, 255), new Color32(255, 255, 255, 255)) },
        {1024, (new Color32(237, 197, 63, 255), new Color32(255, 255, 255, 255)) },
        {2048, (new Color32(237, 194, 45, 255), new Color32(255, 255, 255, 255)) },
        {0, (new Color32(61, 58, 51, 255), new Color32(255, 255, 255, 255)) }
    };

    public uint Value;

    public void UpdateValue(uint value)
    {
        Value = value;
        valueText.text = value.ToString();
        valueText.color = value <= 2048 ? Colors[value].Item2 : Colors[0].Item2;
        GetComponent<Image>().color = value <= 2048 ? Colors[value].Item1 : Colors[0].Item1;
    }

    private void Update()
    {
        if (transform.localPosition != Vector3.zero)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed * Time.deltaTime);
            gameObject.transform.parent.transform.parent.GetComponent<GridController>().AnimationsFinished = false;
        }
    }
}
