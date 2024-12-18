using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Assign this script to the indicator prefabs.
/// </summary>
public class Indicator : MonoBehaviour
{
    [SerializeField] private IndicatorType indicatorType;
    [SerializeField] private Image imageObject;
    [SerializeField] private Text textObject;
    [SerializeField] Text textName;
    private Image indicatorImage;
    private Text distanceText;

    public bool Active
    {
        get
        {
            return transform.gameObject.activeInHierarchy;
        }
    }

   
    public IndicatorType Type
    {
        get
        {
            return indicatorType;
        }
    }

    void Awake()
    {
        indicatorImage = imageObject.GetComponent<Image>();
        distanceText = textObject.GetComponent<Text>();
    }
    public void SetImageColor(Color color)
    {
        indicatorImage.color = color;
    }

    public void SetDistanceText(float value)
    {
        distanceText.text = value >= 0 ? Mathf.Floor(value) + " m" : "";
    }

   
    public void SetTextRotation(Quaternion rotation)
    {
        distanceText.rectTransform.rotation = rotation;
    }

  
    public void Activate(bool value)
    {
        transform.gameObject.SetActive(value);
    }
  
    public void SetName(string name)
    {
        textName.text = name;
    }
}

public enum IndicatorType
{
    BOX,
    ARROW
}
