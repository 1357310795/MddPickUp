using MddPickUp.Helpers;
using MddPickUp.Service;
using MddPickUp.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace MddPickUp.Models
{
    public class TaskModel : INotifyPropertyChanged
    {
        public List<FoodModel> foods;
        public List<FoodModel> Foods
        {
            get { return foods; }
            set
            {
                foods = value;
                this.RaisePropertyChanged("Foods");
            }
        }

        public void RaiseFoodsChanged()
        {
            this.RaisePropertyChanged("Foods");
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                this.RaisePropertyChanged("Title");
            }
        }
        private string startTime;

        public string StartTime
        {
            get { return startTime; }
            set
            {
                startTime = value;
                this.RaisePropertyChanged("StartTime");
            }
        }

        private int interval;
        public int Interval
        {
            get { return interval; }
            set
            {
                interval = value;
                this.RaisePropertyChanged("Interval");
            }
        }
        private int times;
        public int Times
        {
            get { return times; }
            set
            {
                times = value;
                this.RaisePropertyChanged("Times");
            }
        }
        private TaskState state;
        public TaskState State
        {
            get { return state; }
            set
            {
                state = value;
                this.RaisePropertyChanged("State");
            }
        }

        private string message = "等待开始";
        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                this.RaisePropertyChanged("Message");
            }
        }

        public Logger logger;
        public UserControl view;

        public FormModel form;

        public string id;

        //private Timer ti;
        private BackgroundWorker bgw;

        public delegate void OnGoAnimationHandler();

        public event OnGoAnimationHandler OnGoAnimation;

        #region Task Work
        private bool Go()
        {
            Times++;
            //----Check Condition----//
            if (form == null)
            {
                Message = "课程不能为空";
                return false;
            }

            var res = PickUp.FormInfoQuery(form.formNo);
            if (!res.success)
            {
                Message = res.message;
                Common.logger.log("Query失败！ " + form.formNo + " " + res.message);
                return false;
            }
            //----Update Foods----//
            bool foodchanged = false;
            foreach(var foodres in res.foods)
            {
                bool flag = false;
                for (int i = 0; i < Foods.Count; i++)
                {
                    if (Foods[i].goodsId == foodres.goodsId)
                    {
                        Foods[i].inventory = foodres.inventory;
                        Foods[i].sellNum = foodres.sellNum;
                        Foods[i].Refresh();
                        flag = true;
                        foodchanged = true;
                        break;
                    }
                }
                if (!flag)
                {
                    Foods.Add(foodres);
                    foodchanged = true;
                    foodres.Refresh();
                }
            }
            if (foodchanged)
                RaiseFoodsChanged();

            //----Check----//
            List<FoodModel> ava = new List<FoodModel>();
            for (int i = 0; i < Foods.Count; i++)
            {
                var food = Foods[i];
                if (food.isCheck == false) continue;

                if (food.State == GoodStateEnum.Available)
                {
                    ava.Add(food);
                }
            }

            if (ava.Count != 0)
            {
                Message = "有余量！";
                string con = "";
                foreach (var food in ava)
                {
                    con += food.itemName + "\n";
                }
                Common.Toast(Message, con);
                return false;
            }

            //----时间未到----//
            if (DateTime.Parse(form.startTime) > DateTime.Now)
                Message = "未开始";
            Message = "已抢完";
            return true;
        }

        public void StartRun()
        {
            if (State == TaskState.Started) return;
            if (!CanRun())
            {
                //
                return;
            }

            State = TaskState.Started;
            initbgw();
            bgw.RunWorkerAsync();
            OnGoAnimation.Invoke();
        }

        public void StopRun()
        {
            //bgw = null;
            State = TaskState.None;
            Message = "等待开始";
        }

        public bool CanRun()
        {
            return true;
        }

        public void ToggleRun()
        {
            if (State == TaskState.None)
            {
                StartRun();
            }
            else
            {
                StopRun();
            }
        }

        private void initbgw()
        {
            if (bgw == null)
            {
                bgw = new BackgroundWorker();
                bgw.DoWork += Bgw_DoWork;
                bgw.RunWorkerCompleted += Bgw_RunWorkerCompleted;
            }
        }

        private void Bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((bool)e.Result)
            {
                bgw.RunWorkerAsync();
            }
        }

        private void Bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            var res = Go();//true继续
            e.Result = res;
            if (!res)
            {
                State = TaskState.None;
                return;
            }
            if (State != TaskState.Started)
            {
                StopRun();
                e.Result = false;
                return;
            }
            OnGoAnimation.Invoke();
            System.Threading.Thread.Sleep(Interval);
            if (State != TaskState.Started)
            {
                StopRun();
                e.Result = false;
                return;
            }
        }
        #endregion

        #region Constructors
        public TaskModel()
        {
            this.logger = new Logger();
        }

        public TaskModel(FormModel form)
        {
            this.logger = new Logger();
            this.form = form;
            this.Title = form.title;
            this.StartTime = form.startTime;
            this.Interval = 500;
            this.State = TaskState.None;
        }

        #endregion

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion
    }

    public enum TaskState
    {
        None, Started
    }
}
