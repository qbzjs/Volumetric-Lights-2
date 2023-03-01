using System;
using Unity.VisualScripting;
using UnityEngine;
using WaterFlowGPE.Bezier;

namespace WaterFlowGPE
{
    public class WaterFlowManager : MonoBehaviour
    {
        [SerializeField] private BezierSpline _spline;
        [SerializeField, Range(0,100)] private int _numberOfBlocks = 10;
        [SerializeField] private WaterFlowBlock _waterFlowBlockPrefab;
        [SerializeField] private float _waterFlowBlockWidth = 6f;
        [SerializeField, ReadOnly] private WaterFlowBlock[] _waterFlowBlocks;
        [SerializeField] private Transform _blockParent;
        
        public void GenerateWaterFlow()
        {
            if (_spline == null || _numberOfBlocks == 0)
            {
                Debug.LogError("Spline ref is null or number of blocks is 0");
                return;
            }

            //destroy previous blocks
            if (_waterFlowBlocks.Length >= 0)
            {
                foreach (WaterFlowBlock waterFlowBlock in _waterFlowBlocks)
                {
                    if (waterFlowBlock != null)
                    {
                        DestroyImmediate(waterFlowBlock.gameObject);
                        continue;
                    }
                    Debug.LogError("missing block");
                }
            }
            
            //generate blocks
            float distancePerBlock = 1f / _numberOfBlocks;
            Array.Resize(ref _waterFlowBlocks, _numberOfBlocks);
            int index = 0;
            for (float i = 0; i < 1-distancePerBlock; i += distancePerBlock)
            {
                //gets points
                Vector3 point = _spline.GetPoint(i);
                Vector3 nextPoint = _spline.GetPoint(i+distancePerBlock);
                
                //get direction
                Vector3 direction = (nextPoint - point).normalized;

                //instantiate
                _waterFlowBlocks[index] = Instantiate(_waterFlowBlockPrefab, _spline.GetPoint(i), Quaternion.identity, _blockParent);
                _waterFlowBlocks[index].SetupBlock(direction, this, _waterFlowBlockWidth);
                _waterFlowBlocks[index].name = Math.Round(i,2).ToString();

                index++;
            }
        }

        public void SetClosestBlockToPlayer(Transform player)
        {
            //values
            WaterFlowBlock closestBlock = _waterFlowBlocks[0];
            float closestDistance = Vector3.Distance(player.position, _waterFlowBlocks[0].transform.position);
            
            //loop
            for (int i = 1; i < _waterFlowBlocks.Length; i++)
            {
                float distance = Vector3.Distance(player.position, _waterFlowBlocks[i].transform.position);
                if (distance < closestDistance)
                {
                    closestBlock.IsActive = false;
                    
                    closestBlock = _waterFlowBlocks[i];
                    closestBlock.IsActive = true;
                    closestDistance = distance;
                }
                else
                {
                    _waterFlowBlocks[i].IsActive = false;
                }
            }
        }
    }
}
