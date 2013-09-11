using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WindowsFormsApplication1
{
    public class ThreadedQueue<T>
    {
        private readonly Queue<T> _Items;
        private readonly ThreadedQueueViewee<T> _Viewee;
        private bool _Started;
        private DateTime _StartTime;
        private Thread[] _Workers;
        private readonly Object syncronizationRoot = new Object();

        public ThreadedQueue(ThreadedQueueViewee<T> viewee, IEnumerable<T> source)
        {
            this._Viewee = viewee;
            this._Items = new Queue<T>(source);
        }

        public void Push(T item)
        {
            this._Items.Enqueue(item);
        }

        public int Count
        {
            get { return _Items.Count; }
        }


        [MTAThread]
        private static T checkExecute(ThreadedQueue<T> queue)
        {
            if (Monitor.TryEnter(queue.syncronizationRoot, 1000))
            {
                try
                {
                    if (queue._Started && queue._Items.Count > 0)
                    {
                        return queue._Items.Dequeue();
                    }
                    else
                    {
                        queue._Started = false;
                        queue._Viewee.OnWorkDone(queue, queue._StartTime, DateTime.Now);
                    }
                }
                finally
                {
                    Monitor.Exit(queue.syncronizationRoot);
                }
            }


            return default(T);
        }

        private void Work()
        {
            T item = checkExecute(this);

            if (item != null)
            {
                DateTime startTime = DateTime.Now;
                try
                {
                    _Viewee.OnWork(this, item);
                }
                catch (Exception e)
                {
                    _Viewee.OnException(e);
                }
                _Viewee.OnWorkFinished(this, startTime, DateTime.Now);

                Work();
            }
        }

        public int GetThreadCount()
        {

            return _Started ? this._Workers.Length : 0;
        }

        public ThreadState GetTreadState(int ordinal)
        {
            return this._Workers[ordinal].ThreadState;
        }

        public bool Start(int workers)
        {
            if (_Started || workers < 1)
                return false;

            _Started = true;
            _StartTime = DateTime.Now;

            _Workers = new Thread[workers];
            for (int i = 0; i < workers; i++)
            {
                _Workers[i] = new Thread(new ThreadStart(Work));
                _Workers[i].Start();
            }

            return true;

        }
    }
}
