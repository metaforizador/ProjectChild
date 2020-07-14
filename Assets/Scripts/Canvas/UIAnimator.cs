using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimator : MonoBehaviour {

    public LTDescr MoveX(GameObject obj, float to, float time, LeanTweenType ease) {
        return LeanTween.moveLocalX(obj, to, time).setEase(ease).setIgnoreTimeScale(true);
    }

    public LTDescr MoveY(GameObject obj, float to, float time, LeanTweenType ease) {
        return LeanTween.moveLocalY(obj, to, time).setEase(ease).setIgnoreTimeScale(true);
    }

    public LTDescr Scale(GameObject obj, Vector3 to, float time, LeanTweenType ease) {
        return LeanTween.scale(obj, to, time).setEase(ease).setIgnoreTimeScale(true);
    }
}
