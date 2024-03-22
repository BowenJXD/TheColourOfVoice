using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;

namespace BehaviourSequence
{
    public enum IndexConditionType
    {
        True,
        AfterFirst,
        AfterLast,
        False,
    }
    
    /// <summary>
    /// 行为序列，用于执行一系列行为节点。每当满足时间条件和索引条件时，会执行一次序列。
    /// 包括一个黑板，用于存储行为节点之间的数据交互。
    /// 基于QFramework的ActionKit实现。
    /// </summary>
    public class BehaviourSequence : MonoBehaviour
    {
        /// <summary>
        ///  循环间隔条件。0: 无时间条件。
        /// </summary>
        [Tooltip("The time condition to start a loop of the sequence. \n" +
                 "0: No time condition. ")]
        public float loopInterval = -1;
        float loopIntervalTimer = 0;
        bool intervalCondMet = false;
        
        /// <summary>
        ///  循环索引条件。0: 无索引条件。-1: 只在最后一个序列结束后循环。n: 在第n个序列结束后循环（假设起始状态为1）。
        /// </summary>
        [Tooltip("The index condition to start a new sequence. \n" +
                "True: No index condition. \n" +
                "AfterFirst: Start new sequence after the first node. \n" +
                "AfterLast: Start new sequence after the last node. \n" +
                "False: Will not loop automatically.")]
        public IndexConditionType indexCondition = IndexConditionType.True;
        protected bool indexCondMet = false;
        
        LinkedList<BehaviourNode> nodes;
        List<BehaviourNode> currentNodes = new();
        public Action onFinish;

        [SerializeField]
        public Dictionary<string, object> blackboard = new();

        private void OnEnable()
        {
            nodes = new();
            nodes.AddRange(this.GetComponentsInChildren<BehaviourNode>());
            foreach (var node in nodes)
            {
                node.sequence = this;
                node.Init();
            }
            intervalCondMet = loopInterval == 0;
            indexCondMet = true;
            
            TryStartSequence();
        }

        public void Update()
        {
            if (loopIntervalTimer < loopInterval)
            {
                loopIntervalTimer += Time.deltaTime;
            }
            if (loopIntervalTimer >= loopInterval)
            {
                intervalCondMet = true;
            }
            TryStartSequence();
        }

        public void TryStartSequence()
        {
            if (intervalCondMet && indexCondMet)
            {
                Continue();
                if (loopInterval != 0)
                {
                    intervalCondMet = false;
                    loopIntervalTimer = 0;
                }
                if (indexCondition != IndexConditionType.True)
                {
                    indexCondMet = false;
                }
            }
        }

        public void Continue(BehaviourNode prev = null)
        {
            if (prev)
            {
                StopCoroutine(prev.Execute());
                currentNodes.Remove(prev);
            }
            
            BehaviourNode next = null;
            
            if (!prev)
            {
                next = nodes.First.Value;
            }
            else
            {
                next = nodes.Find(prev)?.Next?.Value;
            }
            
            if (prev == nodes.First.Value)
            {
                if (indexCondition == IndexConditionType.AfterFirst) indexCondMet = true;
            }
            if (prev == nodes.Last.Value)
            {
                if (indexCondition == IndexConditionType.AfterLast) indexCondMet = true;
                onFinish?.Invoke();
            }

            if (next)
            {
                StartCoroutine(next.Execute());
                currentNodes.Add(next);
            }
        }
        
        void OnDisable()
        {
            StopAllCoroutines();
            onFinish = null;
        }
        
        public void Set(string key, object value)
        {
            blackboard[key] = value;
        }
        
        public T Get<T>(string key)
        {
            if (!blackboard.ContainsKey(key))
            {
                return default;
            }
            return (T) blackboard[key];
        }
        
        public bool TryGet<T>(string key, out T value)
        {
            if (!blackboard.ContainsKey(key))
            {
                value = default;
                return false;
            }
            
            if (blackboard[key] is T)
            {
                value = (T) blackboard[key];
                return true;
            }
            
            try
            {
                value = (T)Convert.ChangeType(blackboard[key], typeof(T));
                return true;
            }
            catch (InvalidCastException)
            {
                value = default;
            }
            catch (FormatException)
            {
                value = default;
            }
            return false;
        }
        
        public void SetIndexConditionMet(bool value)
        {
            indexCondMet = value;
        }
    }
}