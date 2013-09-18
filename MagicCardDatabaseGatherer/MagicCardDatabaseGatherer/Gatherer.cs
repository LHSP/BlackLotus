using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlackLotus.Cards;
using System.Diagnostics;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Gatherer : Form,
        CardReadViewee,
        ThreadedQueueViewee<int?>
    {
        private delegate void InsertLogDelegate(string text);
        private delegate void SetProgressDelegate(int value);
        private delegate void AddProgressDelegate(int value);
        private delegate void ShowElapsedTimeDelegate();
        CardGather cardGather;
        List<int?> _cardsToGather = new List<int?>();
        ThreadedQueue<int?> queue;

        private Stopwatch _stopwatch;

        public Gatherer()
        {
            for (int i = 0; i < 100; i++)
            {
                _cardsToGather.Add(240034 + i);
            }
            cardGather = new CardGather(this);
            InitializeComponent();
            progressBar.Maximum = _cardsToGather.Count;
            queue = new ThreadedQueue<int?>(this, _cardsToGather);
            _stopwatch = new Stopwatch();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            _stopwatch.Start();
            queue.Start(16);
        }

        private void Log(string value)
        {
            textBox1.Text = value + "\r\n" + textBox1.Text;
        }

        private void ShowElapsedTime()
        {
            if (this.lblTimer.InvokeRequired)
            {
                this.lblTimer.Invoke(new ShowElapsedTimeDelegate(ShowElapsedTime));
            }
            else
            {
                lblTimer.Text = _stopwatch.Elapsed.ToString();
            }
        }

        private void InsertLog(string contentString)
        {
            if (this.textBox1.InvokeRequired)
            {
                this.textBox1.Invoke(new InsertLogDelegate(InsertLog), contentString);
            }
            else
            {
                Log(contentString);
            }
        }

        private void SetProgress(int value)
        {
            if (this.progressBar.InvokeRequired)
            {
                this.progressBar.Invoke(new SetProgressDelegate(SetProgress), value);
            }
            else
            {
                this.progressBar.Value = value;
            }
        }

        private void AddProgress(int value)
        {
            if (this.progressBar.InvokeRequired)
            {
                this.progressBar.Invoke(new AddProgressDelegate(AddProgress), value);
            }
            else
                this.progressBar.Value += value;
        }

        void EixoX.Viewee.OnException(Exception ex)
        {
            InsertLog("Deu merda");
        }

        void CardReadViewee.OnCardRead(Card card)
        {
            AddProgress(1);
            InsertLog(card.ToString());
            ShowElapsedTime();
            Application.DoEvents();
        }

        void ThreadedQueueViewee<int?>.OnWork(ThreadedQueue<int?> caller, int? item)
        {
                cardGather.ReadCard(item.Value);
        }

        void ThreadedQueueViewee<int?>.OnWorkFinished(ThreadedQueue<int?> caller, DateTime startTime, DateTime endTime)
        {
            //  Do nothing
        }

        void ThreadedQueueViewee<int?>.OnWorkDone(ThreadedQueue<int?> caller, DateTime startTime, DateTime endTime)
        {
            InsertLog("Finished!");
            if (_stopwatch.IsRunning)
            {
                _stopwatch.Stop();
            }
        }
    }
}
