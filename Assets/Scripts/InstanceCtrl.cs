using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class InstanceCtrl : MonoBehaviour {

	public Object myInstancedObject;
    public GameObject myo = null;
    private Pose _lastPose = Pose.Unknown;

    void update()
    {
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo>();
        Debug.Log("Pose" + thalmicMyo.pose);
        Debug.Log("Pose" + thalmicMyo.pose);
        bool updateReference = false;
        if (thalmicMyo.pose != _lastPose)
        {
            Debug.Log("Entré después del if lastPose");
            _lastPose = thalmicMyo.pose;

            if (thalmicMyo.pose == Pose.Fist)
            {
                updateReference = true;
            }
        }
    }


    //myInstancedObject.transform.position += new Vector3(-1, 0, 0);
}


