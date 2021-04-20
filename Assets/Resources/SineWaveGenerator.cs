using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "SineWaveGenerator", menuName = "Generate Sine Wave")]
public class SineWaveGenerator : ScriptableObject
{
    //public float fps = 60f; // Sampling frequency i.e. frames per second
    [Tooltip("In Seconds")]
    public float Duration = 5f; // in seconds

    [Tooltip("Frequency in Hz")]
    public float sinFreq1 = 3f; // sine wave frequency

    [Tooltip("Frequency in Hz")]
    public float sinFreq2 = 5f; // sine wave frequency

    public float fps = 250f;

    [Tooltip("Resultant sine wave")]
    public float[] sumOfSins; // sin(2*pi*F*t);

    private float[] t; // (0:timeStep:Duration)
    public float[] sin1; // sin(2*pi*F*t);
    public float[] sin2; // sin(2*pi*F*t);
    private float timeStep = 0f; // 1/fps = seconds per sample

    public int arrayLength;

    public float[] Init()
    {
        timeStep = 1f / fps;
        //timeStep = Time.fixedDeltaTime;
        arrayLength = Mathf.RoundToInt(Duration / timeStep);
        //Debug.Log("Arr length: "+ arrayLength);
        t = new float[arrayLength + 1];
        sin1 = new float[arrayLength + 1];
        sin2 = new float[arrayLength + 1];
        sumOfSins = new float[arrayLength + 1];

        //Debug.Log("Array length: " + arrayLength.ToString()); // +

        int dex = 0;
        for (float i = 0; i < Duration; i += timeStep)
        {
            try
            {
                t[dex] = i;
                dex++;
            }
            catch
            { }
        }

        dex = 0;
        foreach (float d in t)
        {
            try
            {
                sin1[dex] = Mathf.Sin(2 * Mathf.PI * sinFreq1 * t[dex]);
                sin2[dex] = Mathf.Sin(2 * Mathf.PI * sinFreq2 * t[dex]);
                sumOfSins[dex] = sin1[dex] + sin2[dex];
                dex++;
            }
            catch
            { }
        }

        return sumOfSins;
    }

    public float CreateSine(float x, float amp, float flatness, float midline)
    {
        return amp * Mathf.Sin(x + flatness) + midline;
    }

    public float CreateCosine(float x, float amp, float flatness, float midline)
    {
        return amp * Mathf.Cos(x + flatness) + midline;
    }


    // New approach 
    //private static Random rnd = new Random();
    public float[] RandomTerrarain(int length, int sinuses, int cosinuses, float amplsin, float amplcos, float noise)
    {
        //if (length <= 0)
        //    throw new ArgumentOutOfRangeException("length", length, "Length must be greater than zero!");
        float[] returnValues = new float[length];

        for (int i = 0; i < length; i++)
        {
            // sin
            for (int sin = 1; sin <= sinuses; sin++)
            {
                returnValues[i] += amplsin * Mathf.Sin((2 * sin * i * Mathf.PI) / (float)length);
            }
            // cos
            for (int cos = 1; cos <= cosinuses; cos++)
            {
                returnValues[i] += amplcos * Mathf.Cos((2 * cos * i * Mathf.PI) / (float)length);
            }
            // noise
            returnValues[i] += (noise * NexFloat()) - (noise * NexFloat());
        }
        // give offset so it be higher than 0
        float ofs = returnValues.Min();
        if (ofs < 0)
        {
            ofs *= -1;
            for (int i = 0; i < length; i++)
            {
                returnValues[i] += ofs;
            }
        }
        // resize to be fit in 100
        float max = returnValues.Max();
        if (max >= 100f)
        {
            float scaler = max / 100.0f;
            for (int i = 0; i < length; i++)
            {
                returnValues[i] /= scaler;
            }
        }
        return returnValues;
    }

    float NexFloat()
    {
        return Random.value;
    }

    public float SampleGaussian(float mean, float stddev)
    {
        float x1, x2, S;
        // The method requires sampling from a uniform random of (0,1]

        do
        {
            x1 = 2f * NexFloat() - 1f;
            x2 = 2f * NexFloat() - 1f;
            S = x1 * x1 + x2 * x2;
        } while (S >= 1.0f);

        float fac = Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);

        return x1 * fac; 
    }

    float[] NormalizedRandom(float mean, float sigma,float minValue, float maxValue)
    {
        mean = (minValue + maxValue) / 2f;
        sigma = (maxValue - mean) / 3f;

        float[] res = new float[2] { mean, sigma }; 
        return res; 
    }

    public float[] Linspace(float StartValue, float EndValue, int numberofpoints)
    {
        float[] parameterVals = new float[numberofpoints];
        float crement = 0f; 

        if (StartValue < EndValue) // Decrement 
        {
            crement = (EndValue - StartValue) / (numberofpoints - 1f);
        }
        else // Increment 
        {
            crement = Mathf.Abs(StartValue - EndValue) / (numberofpoints - 1f);
        }
        int j = 0; //will keep a track of the numbers 
        float nextValue = StartValue;

        for (int i = 0; i < numberofpoints; i++)
        {
            parameterVals.SetValue(nextValue, j);
            j++;
            if (j > numberofpoints)
            {
                throw new System.Exception(); //.IndexOutOfRangeException();
            }

            if (StartValue < EndValue) // Decrement 
            {
                nextValue = nextValue - crement;
            }
            else
            {
                nextValue = nextValue + crement;
            }
        }

        return parameterVals;



    }

}

//"Current trial: " + expState.currentTrial.ToSting());
/*
 * +
          "t: " + t.ToString() +
          "sum of sines: " + sumOfSins.ToString() +
          "Sine1: " + sin1.ToString());
*/ 