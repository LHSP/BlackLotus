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

namespace WindowsFormsApplication1
{
    public partial class Gatherer : Form,
        CardReadViewee,
        ThreadedQueueViewee<int>
    {
        private delegate void InsertLogDelegate(string text);
        private delegate void SetProgressDelegate(int value);
        CardGather cardGather;
        List<int> _cardsToGather = new List<int>();

        public Gatherer()
        {
            for (int i = 0; i < 1000; i++)
            {
                _cardsToGather.Add(240034 + i);
            }
            cardGather = new CardGather(this);
            InitializeComponent();
            progressBar.Maximum = _cardsToGather.Count;
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            ThreadedQueue<int> queue = new ThreadedQueue<int>(this, _cardsToGather);
            queue.Start(16);
        }

        private void Log(string value)
        {
            textBox1.Text = value + "\r\n" + textBox1.Text;
        }

        private void InsertLog(string contentString)
        {
            if (this.textBox1.InvokeRequired)
            {
                this.textBox1.Invoke(new InsertLogDelegate(InsertLog), contentString);
            }
            else
                Log(contentString);
        }

        private void SetProgress(int value)
        {
            if (this.progressBar.InvokeRequired)
            {
                this.progressBar.Invoke(new SetProgressDelegate(SetProgress), value);
            }
            else
                this.progressBar.Value = value;
        }

        void EixoX.Viewee.OnException(Exception ex)
        {
            throw new NotImplementedException();
        }

        void CardReadViewee.OnCardRead(Card card)
        {
            InsertLog(card.ToString());
            SetProgress(progressBar.Value + 1);
            Application.DoEvents();
        }

        void ThreadedQueueViewee<int>.OnWork(ThreadedQueue<int> caller, int item)
        {
            cardGather.ReadCard(item);
        }

        void ThreadedQueueViewee<int>.OnWorkFinished(ThreadedQueue<int> caller, DateTime startTime, DateTime endTime)
        {
            //  Do nothing
        }

        void ThreadedQueueViewee<int>.OnWorkDone(ThreadedQueue<int> caller, DateTime startTime, DateTime endTime)
        {
            Log("Finished!");
        }
    }
}
