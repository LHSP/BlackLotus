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
using System.IO;
using Helpers;

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
        private delegate void UpdateCardCounterDelegate();
        CardGather cardGather;
        List<int?> _cardsToGather = new List<int?>();
        ThreadedQueue<int?> queue;

        private Stopwatch _stopwatch;

        public Gatherer()
        {
            cardGather = new CardGather(this);
            InitializeComponent();
            _stopwatch = new Stopwatch();
        }

        private void ResetUI()
        {
            this.progressBar.Value = 0;
            this.progressBar.Maximum = 0;
            this.textBox1.Text = "";
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            ResetUI();
            StreamReader reader;
            //  Check if the cardIds text we need exists
            if (!File.Exists("OrderedCardIds.txt"))
            {
                MessageBox.Show("Please first order the card list.");
            }
            else
            {
                string fileContent;
                //  Reads the ids we want to get from the Magic Gatherer
                using (reader = new StreamReader(File.OpenRead("OrderedCardIds.txt")))
                {
                    fileContent = reader.ReadToEnd();
                }
                List<string> cardIds = fileContent.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();

                _cardsToGather = cardIds.Select(GetNullableInt).Take(20).ToList();
                progressBar.Maximum = _cardsToGather.Count;
                queue = new ThreadedQueue<int?>(this, _cardsToGather);

                _stopwatch.Start();
                queue.Start(16);
            }
        }

        private int? GetNullableInt(string value)
        {
            return value.GetValueOrNull<int>();
        }

        private void SetCardCounter()
        {
            lblProgress.Text = progressBar.Value.ToString() + '/' + progressBar.Maximum.ToString();
        }

        private void Log(string value)
        {
            textBox1.Text = value + "\r\n" + textBox1.Text;
        }

        private void UpdateCardCounter()
        {
            if (this.lblProgress.InvokeRequired)
                this.lblProgress.Invoke(new UpdateCardCounterDelegate(SetCardCounter));
            else
                SetCardCounter();
        }

        private void ShowElapsedTime()
        {
            if (this.lblTimer.InvokeRequired)
            {
                this.lblTimer.Invoke(new ShowElapsedTimeDelegate(ShowElapsedTime));
            }
            else
            {
                lblTimer.Text = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
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
            InsertLog("Deu merda" + ex.Source);
        }

        void CardReadViewee.OnCardInformationRead(CardInformation card)
        {
            AddProgress(1);
            InsertLog(card.ToString());
            ShowElapsedTime();
            UpdateCardCounter();
            BlackLotusDb<CardInformation>.Instance.Insert(card);
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

        private void orderListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(OrderCardIdList().ToString());
        }

        public bool OrderCardIdList()
        {
            try
            {
                //  Open the file with the cards ids to read and sort.
                StreamReader reader = new StreamReader(File.OpenRead("CardIds.txt"));
                //  Create/Overwrite the file to where the cards ids qill be saved.
                StreamWriter writer = new StreamWriter(File.Create("OrderedCardIds.txt"));
                //  Read all the cards ids.
                string fileContent = reader.ReadToEnd();
                //  Split the filecontent by line break and ignore empty entries to be sure.
                //  Each card id must be on its own line.
                List<string> cardIds = fileContent.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                //  Sort the distinct elements of the list and save them to a list of integers.
                List<int> convertedCardIds = cardIds.Select(Int32.Parse).Distinct().OrderBy(x => x).ToList();
                //  Write to the ordered file.
                //  One Card Id by line.
                foreach (int cardId in convertedCardIds)
                    writer.WriteLine(cardId);
                //  Guarantees the ids have all been written.
                writer.Flush();
                //  Disposes of the reader
                reader.Close();
                //  Disposes of the writer
                writer.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }
    }
}
