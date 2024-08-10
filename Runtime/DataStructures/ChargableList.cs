using System;
using System.Collections;
using System.Collections.Generic;
using Bdeshi.Helpers.Utility;
using UnityEngine;

namespace Bdeshi.Helpers.DataStructures
{
    [Serializable]
    public class ChargableList<TItem> : IEnumerable<TItem>
    {
        [SerializeField] private int chargeIndex;
        [SerializeField] private List<ChargableListSlot> items = new List<ChargableListSlot>();
        [SerializeField] private bool ChargeComplete => chargeIndex >= (items.Count) ||
                                                        ( chargeIndex == (items.Count - 1)
                                                          && items[chargeIndex].chargeTimer.isComplete);

        // [SerializeField] private bool hasMoreChargeLevels => (items.Count) > chargeIndex;
        public bool hasMoreChargeLevels => (items.Count -1 ) > chargeIndex;
        public int chargeLevelCount => items.Count;

        public bool tryGetNextItem(out TItem item)
        {
            item = default(TItem);
            if (hasMoreChargeLevels)
            {
                item = items[chargeIndex + 1].item;
                
                return true;
            }
            else
            {
                return false;
            }
        }
        public void increaseCharge(float amount,
            Func<TItem, TItem,bool > levelIncreasePermissionFunc = null,
            Action<TItem> levelIncreaseCallback = null)
        {
            if (ChargeComplete)
            {
                return;
            }
            
            while ( amount > 0 && 
                    (items.Count) > chargeIndex &&
                   (levelIncreasePermissionFunc == null
                    || levelIncreasePermissionFunc.Invoke(items[chargeIndex].item, items[chargeIndex + 1].item)))
            {
                if(items[chargeIndex].chargeTimer.tryCompleteTimer(amount, out float remainder))
                {
                    amount = remainder;
                    if(chargeIndex < (items.Count - 1))
                    {
                        chargeIndex++;
                        if (chargeIndex < items.Count)
                        {
                            items[chargeIndex].chargeTimer.reset();
                            levelIncreaseCallback?.Invoke(items[chargeIndex].item);
                        }
                    }
                    else
                    {
                        
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public bool tryDropLevel(Action<TItem> levelChangeCallback= null)
        {
            if (chargeIndex <= 0)
            {
                return false;
            }


            float oldRatio = items[chargeIndex].chargeTimer.Ratio;
            items[chargeIndex].chargeTimer.reset();

            chargeIndex--;
            items[chargeIndex].chargeTimer.resetByFractionOfMax(oldRatio);
            levelChangeCallback?.Invoke(items[chargeIndex].item);
            return true;
        }
        
        
        public void decreaseCharge(float amount, Action<TItem> levelChangeCallback = null)
        {
            bool changed = false;
            while ( amount > 0 && items[chargeIndex].chargeTimer.Timer >= 0)
            {
                if(items[chargeIndex].chargeTimer.tryEmptyTimer(amount, out float remainder))
                {
                    amount = remainder;
                    changed = true;
                    if (chargeIndex <= 0)
                    {
                        break;
                    }

                    chargeIndex--;
                }
                else
                {
                    break;
                }
            }
            if(changed)
                levelChangeCallback?.Invoke(items[chargeIndex].item);
        }

        public int getCurChargeLevel() => chargeIndex;


        public TItem getCurrentItem()
        {
            if (items.Count <= 0)
                return default(TItem);
            return items[chargeIndex].item;
        }

        public float getFirstChargeCost() => items[0].chargeTimer.MaxValue; 
        public TItem getItem(int index)
        {
            return items[index].item;
        }

        public bool isEmpty()
        {
            return chargeIndex == 0 && items[0].chargeTimer.Timer <= 0;
        }

        public float getCurrentLevelChargeRatio()
        {
            if (items.Count <= 0)
                return 0;
            return items[chargeIndex].chargeTimer.Ratio;
        }
        
        public float getCurrentLevelRemainingAmount()
        {
            if (items.Count <= 0)
                return 0;
            return items[chargeIndex].chargeTimer.remaingValue();
        }

        public void resetChargeLevel()
        {
            chargeIndex = 0;
            if (chargeIndex < items.Count)
            {
                items[chargeIndex].chargeTimer.reset();
            }
        }

        public TItem getItemAndReset()
        {
            var result =  getCurrentItem();
            resetChargeLevel();
            return result;
        }

        /// <summary>
        /// Total charge amount over fully charged levels,
        /// 2.5 levels charged=. get timer sum of 2 levels
        /// </summary>
        /// <returns></returns>
        public float getFullyChargedAmount()
        {
            float total = 0;
            for (int i = 0; i <= chargeIndex && i < items.Count; i++)
            {
                if (i == chargeIndex && items[i].chargeTimer.Ratio < 1)
                    break;
                total += items[i].chargeTimer.Timer;
            }

            return total;
        }
        
        public float getTotalChargedAmount()
        {
            float total = 0;
            for (int i = 0; i <= chargeIndex && i < items.Count; i++)
            {
                total += items[i].chargeTimer.Timer;
            }

            return total;
        }

        
        /// <summary>
        /// set max timer values for ALL charge levels
        /// </summary>
        /// <param name="to">max timer values for ALL charge levels</param>
        public void setAllChargeThresholdsTo(float to)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].chargeTimer.MaxValue = to;
            }
        }

        /// <summary>
        /// How many levels completely charged
        /// </summary>
        /// <returns></returns>
        public int getFullyChargedLevelCount()
        {
            return chargeIndex;
        }

        
        /// <summary>
        /// chargeIndex + items[chargeIndex].chargeTimer.Ratio
        /// </summary>
        /// <returns></returns>
        public float getNormalizedChargedLevelCount()
        {
            return chargeIndex + items[chargeIndex].chargeTimer.Ratio;
        }
        
        
        /// <summary>
        /// 2.5 => guaranteed drop of 2 levels + lose another half of style gauge
        /// from cur charge level as much as possible and the remaining from the charge level from initial drop
        /// EX: from S with 25% to C with 75%.
        /// </summary>
        /// <param name="normalizedChargeLevelDrop"></param>
        public void subtractNormalizedChargeLevel(float normalizedChargeLevelDrop)
        {
            float curNormalizedChargeLevel = getNormalizedChargedLevelCount();
            curNormalizedChargeLevel = Mathf.Max(curNormalizedChargeLevel - normalizedChargeLevelDrop, 0);
            chargeIndex =(int) curNormalizedChargeLevel;
            items[chargeIndex].chargeTimer.resetByFractionOfMax(curNormalizedChargeLevel - chargeIndex);
        }

        public float getChargeCostUpTo(float normalizedChargeLevelThreshold, out int resultingChargeLevel)
        {
            float chargeCostSum = 0;
            resultingChargeLevel = 0;
            foreach (var item in items)
            {
                float newSum = chargeCostSum + item.chargeTimer.MaxValue;
                if (newSum > normalizedChargeLevelThreshold || resultingChargeLevel >= (items.Count - 1))
                {
                    break;
                }

                chargeCostSum = newSum;
                resultingChargeLevel++;
            }

            return chargeCostSum;
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            foreach (var chargableListSlot in items)
            {
                yield return chargableListSlot.item;
            }
        }


        public IEnumerable<ChargableListSlot> getChargeLevels => items;
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        [Serializable]
        public class ChargableListSlot
        {
            [SerializeField] public TItem item;
            [SerializeField] public FiniteTimer chargeTimer = new FiniteTimer(0.5f);
        }


    }
}