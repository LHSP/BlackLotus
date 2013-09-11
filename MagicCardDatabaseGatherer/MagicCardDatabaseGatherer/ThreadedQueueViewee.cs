using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EixoX;

namespace WindowsFormsApplication1
{
    public interface ThreadedQueueViewee<T> : Viewee
    {
        void OnWork(ThreadedQueue<T> caller, T item);
        void OnWorkFinished(ThreadedQueue<T> caller, DateTime startTime, DateTime endTime);
        void OnWorkDone(ThreadedQueue<T> caller, DateTime startTime, DateTime endTime);
    }
}
