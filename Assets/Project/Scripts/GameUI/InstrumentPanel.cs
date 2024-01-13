using UnityEngine;

[System.Serializable]
struct InstrumentPanel
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float minSpeedArrowAngle;
    [SerializeField] private float maxSpeedArrowAngle;

    private RectTransform _arrow;

    public void SetArrow(RectTransform foundComponent)
    {
        _arrow = foundComponent;
    }

    public void SetAngles(float speed)
    {
        _arrow.localEulerAngles = new Vector3(
            0,
            0,
            Mathf.Lerp(minSpeedArrowAngle, maxSpeedArrowAngle, speed / maxSpeed)
        );
    }
}