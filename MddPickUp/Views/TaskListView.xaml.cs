using MddPickUp.Models;
using MddPickUp.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MddPickUp.Views
{
    /// <summary>
    /// TaskView.xaml 的交互逻辑
    /// </summary>
    public partial class TaskListView : UserControl//, INotifyPropertyChanged
    {
        //public BindingList<TaskModel> Tasks
        //{
        //    get { return Task.tasks; }
        //    set { Task.tasks = value; }
        //}

        public TaskListView()
        {
            InitializeComponent();
        }

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

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            foreach(TaskModel t in Task.tasks)
            {
                if (t.State== TaskState.Started)
                {
                    MessageBox.Show("有任务正在运行，无法刷新");
                    return;
                }
            }
            Task.tasks.Clear();

            var res = PickUp.GroupInfoQuery(BasicInfo.groupNo);
            if (!res.success)
            {
                MessageBox.Show(res.message);
                return;
            }

            foreach(FormModel f in res.forms)
            {
                var t = new TaskModel(f);
                var res1 = PickUp.FormInfoQuery(f.formNo);
                if (!res1.success)
                {
                    MessageBox.Show(res1.message);
                    return;
                }
                t.Foods = res1.foods;
                Task.tasks.Add(t);
            }
        }
    }
}
