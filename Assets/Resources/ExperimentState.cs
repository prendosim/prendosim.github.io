using UnityEngine;
using System.Linq; 

[CreateAssetMenu(fileName = "ExperimentState", menuName = "State of Experiment")]
public class ExperimentState : ScriptableObject
{
    [Header("Target Motion Parameters")]
    public int numTargets; 
    public float dialAngleTarget = 1f;

    [Header("Trial States")]
    public int numTrials; 
    public bool endTrial;
    public bool inTrial;
    public bool pause;
    public bool uploadData;
    public float trialDuration;
    public int currentTrial;
    public int[] startTrial = new int[3];
    public int actualTrialNum; 

    [Header("Other Game States")]
    public float dialAngle = 1f;
    public float exPonent = 1.0f;
    public float initDelay = 10f;
    public float trackDifficulty = 1f;
    public float virtDialAngle = 1f;
    public float[] bendState = new float[2];
    public int addtoScore = 1;
    public int group; // 0 = no reward group and 1 = reward group
    public int score = 0;
    public Vector3 scaleFactor = new Vector3(1f, 1f, 1f);

    [Range(0f, 2f)]
    public float[] bendPerc = new float[2];

    public int block; // baseline = 0; adaptation = 1; and washout = 2 blocks
    public int startExperiment = 0;

    [Range(0.9f, 1.025f)]
    public float handGain = 0.95f;

    [Range(1f, 180f)]
    public float[] targetRange = new float[2];

    public float angularSpeed = 0.5f;

    public float dialGaining = 2.4f;

    // Functions 
    public float Remap(float value, float[] selfMinMax, float[] otherMinMax)
    {
        return otherMinMax[0] + (value - selfMinMax[0]) * ((otherMinMax[1] - otherMinMax[0]) / (selfMinMax[1] - selfMinMax[0]));
    }

    public int[] SequenceLooper(int numTrials, int numConditions)
    {
        int conditionCycle = 0;
        int[] result = new int[numTrials];

        for (int i = 0; i < numTrials; i++)
        {
            result[i] = conditionCycle;
            conditionCycle++;

            if (conditionCycle >= numConditions)
            {
                conditionCycle = 0;
            }
        }

        return result;
    }

    public T[] ShuffleArray<T>(T[] array)
    {
        System.Random r = new System.Random();
        for (int i = array.Length; i > 0; i--)
        {
            int j = r.Next(i);
            T k = array[j];
            array[j] = array[i - 1];
            array[i - 1] = k;
        }

        return array;
    }

    public float Sigmoid(float inp)
    {
        float k = Mathf.Exp(inp);
        return k / (1.0f + k);
    }

    public void ResetTrial()
    {
        for (int i = 0; i < startTrial.Length; i++)
        {
            startTrial[i] = 0;
        }
    }

    public void StartTrial()
    {
        for (int i = 0; i < startTrial.Length; i++)
        {
            startTrial[i] = 1;
        }
    }
    
    public float[] FloatRange(float min, float max, int steps)
    {
        return Enumerable.Range(0, steps)
                         .Select(i => min + (max - min) * ((float)i / (steps - 1))).ToArray();
    }

    public AnimationCurve BellCurve(float movementDuration)
    {
        AnimationCurve curve = new AnimationCurve();

        for (float i = 0f; i < movementDuration; i += Time.fixedDeltaTime)
        {
            float val = Gauss01(i, 1f, (movementDuration / 2f), 0.25f); //(i, 1f, 0f, 0.5f) // (i, 1f, (movementDuration / 2f), 0.5f);
            curve.AddKey(i, val);
        }

        return curve;
    }

    public float Gauss01(float x, float amp, float mean, float sd)
    {
        var v1 = (x - mean);
        var v2 = (v1 * v1) / (2f * (sd * sd));
        var v3 = amp * Mathf.Exp(-v2);

        return v3;
    }

    public float Gauss(float x, float amp, float mean, float sd)
    {
        var v1 = (x - mean) / (2f * sd * sd);
        var v2 = -v1 * v1 / 2f;
        var v3 = amp * Mathf.Exp(v2);

        return v3;
    }

    //public static float Map(this float value, float fromSource, float toSource, float fromTarget, float toTarget)
    //{
    //    return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    //}
}

public static class ExtensionMethods
{
    public static float Normalize(this float inputVal, float maxVal)
    {
        return Mathf.Clamp01((inputVal) / maxVal);
    }

    public static float Map(this float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
}