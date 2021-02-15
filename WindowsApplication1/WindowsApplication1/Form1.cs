using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsApplication1
{
    public partial class Form1 : Form
    {
        Dictionary<CheckBox, Cell> field = new Dictionary<CheckBox, Cell>();
        Money money = new Money();
        int time_after_start = 0;
        int timer1_interval = 100;
        public Form1()
        {
            InitializeComponent();
            foreach (CheckBox cb in panel1.Controls)
                field.Add(cb, new Cell());
            moneyCounterLabel.Text = money.current_money.ToString();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (sender as CheckBox);
            if (cb.Checked) Plant(cb);
            else Harvest(cb);
            moneyCounterLabel.Text = money.current_money.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (CheckBox cb in panel1.Controls)
                NextStep(cb);
            time_after_start += timer1.Interval;
            if (time_after_start % 1000 == 0)
            {
                var timespan = TimeSpan.FromSeconds(time_after_start / 1000);
                timeCounterLabel.Text = timespan.ToString(@"mm\:ss");
            }

        }

        private void Plant(CheckBox cb)
        {
            field[cb].Plant(money);
            UpdateBox(cb);
        }

        private void Harvest(CheckBox cb)
        {
            field[cb].Harvest(money);
            UpdateBox(cb);
        }

        private void NextStep(CheckBox cb)
        {
            field[cb].NextStep();
            UpdateBox(cb);
        }

        private void UpdateBox(CheckBox cb)
        {
            Color c = Color.White;
            switch (field[cb].state)
            {
                case CellState.Planted: c = Color.Black;
                    break;
                case CellState.Green: c = Color.Green;
                    break;
                case CellState.Immature: c = Color.Yellow;
                    break;
                case CellState.Mature: c = Color.Red;
                    break;
                case CellState.Overgrow: c = Color.Brown;
                    break;
            }
            cb.BackColor = c;
            
        }

        private void speedNumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            decimal d = speedNumericUpDown1.Value;
            string DoubleValue = decimal.ToDouble(d).ToString();
           
            switch (DoubleValue)
            {
                case "1":
                    timer1_interval = 100;
                    break;
                case "2":
                    timer1_interval = 75;
                    break;
                case "3":
                    timer1_interval = 50;
                    break;
                case "4":
                    timer1_interval = 25;
                    break;
            }
            timer1.Interval = timer1_interval;
        }

        
    }


    enum CellState
    {
        Empty,   // -2 money after click
        Planted, // 0
        Green,   // 0
        Immature,// 3
        Mature,  // 5
        Overgrow // -1
    }

    class Money
    {
        private const int default_money = 100;
        public int current_money = default_money;
        public void ChangeCount(int sum)
        {
            current_money += sum;
        }
    }

    class Cell
    {
        public CellState state = CellState.Empty;
        public int progress = 0;

        private const int prPlanted = 20;
        private const int prGreen = 100;
        private const int prImmature = 120;
        private const int prMature = 140;

        private const int money_for_empty = -2;
        private const int money_for_planted = 0;
        private const int money_for_green = 0;
        private const int money_for_immature = 3;
        private const int money_for_mature = 5;
        private const int money_for_overgrow = -1;

        public int money_for_harvest = money_for_empty;

        public void Plant(Money money)
        {
            state = CellState.Planted;
            progress = 1;
            money.ChangeCount(money_for_harvest);
            money_for_harvest = money_for_planted;
        }

        public void Harvest(Money money)
        {
            money.ChangeCount(money_for_harvest);
            state = CellState.Empty;
            progress = 0;
            money_for_harvest = money_for_empty;       
        }

        public void NextStep()
        {
            if ((state != CellState.Empty) && (state != CellState.Overgrow))
            {
                progress++;
                if (progress < prPlanted)
                {
                    state = CellState.Planted;
                    money_for_harvest = money_for_planted;
                }
                else if (progress < prGreen)
                {
                    state = CellState.Green;
                    money_for_harvest = money_for_green;
                }
                else if (progress < prImmature)
                {
                    state = CellState.Immature;
                    money_for_harvest = money_for_immature;
                }
                else if (progress < prMature)
                {
                    state = CellState.Mature;
                    money_for_harvest = money_for_mature;
                }
                else
                {
                    state = CellState.Overgrow;
                    money_for_harvest = money_for_overgrow;
                }
            }
        }
    }
}