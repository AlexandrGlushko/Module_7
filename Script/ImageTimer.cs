using UnityEngine;
using UnityEngine.UI;

public class ImageTimer : MonoBehaviour
{
    [SerializeField] Image _image;
    private bool isStart;
    public bool timeTick;
    private float currentTime;
    private float maxTime;

    private void Start()
    {
        currentTime = maxTime;
    }
    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            timeTick = false;
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                timeTick = true;
                currentTime = maxTime;
            }
            _image.fillAmount = currentTime / maxTime;
        }
        else
        {
            _image.fillAmount = 1;
        }
    }

    public void TimerSetStart(float maxTimeSet, bool isStartTime)
    {
        currentTime = maxTime = maxTimeSet;
        isStart = isStartTime; 
    }

}
