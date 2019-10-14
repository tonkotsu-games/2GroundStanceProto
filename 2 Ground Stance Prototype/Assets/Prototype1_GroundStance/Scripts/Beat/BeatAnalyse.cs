using System.Collections.Generic;
using UnityEngine;

public class BeatAnalyse : MonoBehaviour
{
    float[] spectrum;
    public static List<int> beatStarts = new List<int>();

    [SerializeField] int windowTrigger;
    [SerializeField] float drawWidth;
    [SerializeField] float limit, waitSamples;
    [SerializeField] AudioClip wave;
    [SerializeField] AudioSource sourceWave;
    bool beat;

    private float timeSample;


    void Start()
    {
        int amount = wave.samples;
        spectrum = new float[amount];
        wave.GetData(spectrum, 0);
        

        for (int i = 0; i < spectrum.Length; i++)
        {
            spectrum[i] = Mathf.Abs(spectrum[i]);
        }

        for (int i = 1; i < spectrum.Length -1; i++)
        {
            if (spectrum[i] > limit)
            {
                if(spectrum[i] <= spectrum[i-1] && spectrum[i] >= spectrum[i+1])
                {                    
                    beatStarts.Add(i);
                    i += (int)waitSamples;
                }
            }
        }
    }
    private void Update()
    {
        if (IsOnBeat(1000) && beat == false)
        {
            beat = true;
        }
        else if (!IsOnBeat(1000))
        {
            beat = false;
        }
    }
    public bool IsOnBeat(int preStart)
    {
        timeSample = sourceWave.timeSamples - preStart;
        for (int i = 0; i < BeatAnalyse.beatStarts.Count; i++)
        {
            if (timeSample >= (BeatAnalyse.beatStarts[i] - windowTrigger) &&
                timeSample <= (BeatAnalyse.beatStarts[i] + windowTrigger))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if(spectrum == null)
        {
            return;
        }
            Vector3 displacement = Camera.main.ScreenToWorldPoint(new Vector3(100, 100, 5));
            float heightMulti = 1;
            float widthMulti = 0.000005f;
            Gizmos.color = new Color(0.5f, 0, 0.5f, 1);

            for (int i = 0; i < spectrum.Length; i += 100)
            {
                Gizmos.DrawLine(displacement + new Vector3(i * widthMulti, 0, 0),
                                displacement + new Vector3(i * widthMulti, heightMulti * spectrum[i], 0));
            }
        Gizmos.color = Color.green;
        for (int i = 0; i < BeatAnalyse.beatStarts.Count; i++)
        {
            Gizmos.DrawLine(displacement + new Vector3((BeatAnalyse.beatStarts[i] - windowTrigger + 1000) * widthMulti, 0, 0),
                            displacement + new Vector3((BeatAnalyse.beatStarts[i] + windowTrigger + 1000) * widthMulti, 0, 0));
        }
        Gizmos.color = Color.red;
        Gizmos.DrawLine(displacement + new Vector3(timeSample * widthMulti, 0, 0), displacement + new Vector3(timeSample * widthMulti, heightMulti, 0));

    }
}