using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text statusText;

    private float infection, cure, fear;
    private int samples;

    public float Infection { get => infection; set { infection = value; UpdateText(); } }
    public float Cure { get => cure; set { cure = value; UpdateText(); } }
    public float Fear { get => fear; set { fear = value; UpdateText(); } }
    public int Samples { get => samples; set { samples = value; UpdateText(); } }

    // Start is called before the first frame update
    void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        statusText.text = FormatText();
    }

    private string FormatText()
    {
        return FormatLine("green", "Infection", Infection) + "\n" +
               FormatLine("darkblue", "Cure", Cure) + "\n" +
               FormatLine("brown", "Fear", Fear) + "\n" +
               FormatLine("teal", "Samples", samples, false);
    }

    private string FormatLine(string color, string name, float value, bool isPercentage = true)
    {
        string percent = isPercentage ? "%" : "";
        value = isPercentage ? value * 100 : value;

        return "<color=" + color + ">" + name + ": " + Mathf.RoundToInt(value).ToString() + percent + "</color>";
    }
}
