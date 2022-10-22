using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    
    public class Dispatcher: MonoBehaviour
    {
        private static Dispatcher _instance;
        public static Dispatcher Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("Dispatcher").AddComponent<Dispatcher>();
                }
                return _instance;
            }
        }
    
        private readonly Queue<Action> ExecuteOnMainThread = new Queue<Action>();
        
        void Awake()
        {
            
        }

        void Update()
        {
            while (ExecuteOnMainThread.Count > 0)
            {
                ExecuteOnMainThread.Dequeue().Invoke();
            }
        }
        
        public void RunInMainThread(Action action)
        {
            ExecuteOnMainThread.Enqueue(action);
        }
    }
}