namespace Script.Other
{
    using System;
    using UnityEngine;
    using System.Collections.Generic;

    
    public class PrefabSetupManager : MonoBehaviour
    {
        public List<SetupData> SetupList = new List<SetupData>();

        public void SaveSetup()
        {
            
            List<TransformData> transformList = new List<TransformData>();
            transformList.Add(new TransformData(transform));
            foreach (Transform child in transform)
            {
                transformList.Add(new TransformData(child.transform));
                foreach (Transform childChild in child)
                {
                    transformList.Add(new TransformData(childChild.transform));
                }
            }

            SetupData data = new SetupData(transformList);
            SetupList.Add(data);
        }

        public void LoadSetup(int x)
        {
            if (x >= SetupList.Count)
            {
                return;
            }

            Vector3 basePosition = transform.position;
            Vector3 baseScale = transform.localScale;
            
            int childNumber = 0;
            
            SetupList[x].Data[childNumber].ApplyTo(transform);
            childNumber++;
            foreach (Transform child in transform)
            {
                SetupList[x].Data[childNumber].ApplyTo(child);
                childNumber++;
                foreach (Transform childChild in child)
                {
                    SetupList[x].Data[childNumber].ApplyTo(childChild);
                    childNumber++;
                }
            }

            transform.position = basePosition;
            transform.localScale = baseScale;
        }

        public void DeleteSetup(int x)
        {
            SetupList.Remove(SetupList[x]);
        }
        
        public void DebugAllChild()
        {
            foreach (Transform child in transform)
            {
                Debug.Log(child.name);
                foreach (Transform childChild in child)
                {
                    Debug.Log($"-> {childChild.name}");
                }
            }
        }
    }
    
    [Serializable]
    public class TransformData
    {
        public Vector3 LocalPosition = Vector3.zero;
        public Vector3 LocalEulerRotation = Vector3.zero;
        public Vector3 LocalScale = Vector3.one;

        public TransformData(Transform transform)
        {
            LocalPosition = transform.localPosition;
            LocalEulerRotation = transform.localEulerAngles;
            LocalScale = transform.localScale;
        }

        public void ApplyTo(Transform transform)
        {
            transform.localPosition = LocalPosition;
            transform.localEulerAngles = LocalEulerRotation ;
            transform.localScale = LocalScale;
        }
    }


    [Serializable]
    public class SetupData
    {
        public string Name = "name";
        public List<TransformData> Data = new List<TransformData>();

        public SetupData(List<TransformData> datas )
        {
            Data = datas;
        }
    }
}

