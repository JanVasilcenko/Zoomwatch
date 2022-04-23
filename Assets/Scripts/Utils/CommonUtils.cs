using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonUtils {
    public static float RandomBetweenTwoFloats(float minValue, float maxValue) {
        return Random.Range(minValue, maxValue + 1);
    }
    public static int RandomBetweenTwoIntegers(int minValue, int maxValue) {
        return Random.Range(minValue, maxValue + 1);
    }
}
