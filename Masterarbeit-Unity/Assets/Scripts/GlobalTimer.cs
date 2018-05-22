using UnityEngine;

public class GlobalTimer : MonoBehaviour
{

    public string startTime;
    public string endTime;
    public float playTime;
    public float totalTime;
    public static GlobalTimer instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Use this for initialization
    void Start()
    {
        //Für die Spielzeiten
        startTime = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
    }

    // Update is called once per frame
    void Update()
    {
        setPlayTime();
        setTotalTime();
    }

    public void setEndTime()
    {
        endTime = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
    }

    void setPlayTime()
    {
        playTime += Time.deltaTime;
    }
    void setTotalTime()
    {
        totalTime += Time.unscaledDeltaTime;
    }
}
