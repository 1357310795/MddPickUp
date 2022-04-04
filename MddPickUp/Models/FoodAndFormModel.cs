using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MddPickUp.Models
{
    public class FormInfoQueryResult
    {
        public List<FoodModel> foods;
        public bool success;
        public string message;

        public FormInfoQueryResult()
        {
        }

        public FormInfoQueryResult(bool success, string message)
        {
            this.success = success;
            this.message = message;
        }

        public FormInfoQueryResult(List<FoodModel> foods, string message)
        {
            this.foods = foods;
            this.message = message;
            this.success = true;
        }
    }//返回详细的foods
    public class FormInfoQueryDto
    {
        public int code;
        public string msg;
        public FormInfoQueryDataDto data;
    }
    public class FormInfoQueryDataDto
    {
        public FormInfoQuerywaFanTanFormDto waFanTanForm;
    }
    public class FormInfoQuerywaFanTanFormDto
    {
        public List<FormInfoQuerygoodDto> goodsWidgets;
    }
    public class FormInfoQuerygoodDto
    {
        public string categoryName;
        public List<string> images;
        public string intro;
        public string itemName;
        public double price;
        public int inventory;
        public int sellNum;
        public string goodsId;
    }

    public class GroupInfoQueryResult
    {
        public List<FormModel> forms;
        public bool success;
        public string message;

        public GroupInfoQueryResult()
        {
        }

        public GroupInfoQueryResult(bool success, string message)
        {
            this.success = success;
            this.message = message;
        }

        public GroupInfoQueryResult(List<FormModel> forms, string message)
        {
            this.forms = forms;
            this.message = message;
            this.success = true;
        }
    }//返回详细的forms
    public class GroupInfoQueryDto
    {
        public int code;
        public string msg;
        public GroupInfoQueryDataDto data;
    }
    public class GroupInfoQueryDataDto
    {
        public bool hasMore;
        public List<GroupInfoQueryRecordDto> sendRecords;
    }
    public class GroupInfoQueryRecordDto
    {
        public GroupInfoQuerywaFanTanFormDto waFanTanForm;
    }
    public class GroupInfoQuerywaFanTanFormDto
    {
        public string content;
        public string formNo;
        public int formStatus;
        public string startTime;
        public string title;
    }

    public class FormModel
    {
        //public List<FoodModel> foods;
        public string content;
        public string startTime;
        public string formNo;
        public int formStatus;
        public string title;
        public FormModel()
        {
        }
        public FormModel(GroupInfoQuerywaFanTanFormDto dto)
        {
            this.content = dto.content;
            this.startTime = dto.startTime;
            this.formNo = dto.formNo;
            this.formStatus = dto.formStatus;
            this.title = dto.title;
        }
    }
    public class FoodModel: INotifyPropertyChanged
    {
        public FormModel form;
        public string categoryName;
        public List<string> images;
        public string intro;
        public string itemName;
        public double price;
        public int inventory;
        public int sellNum;
        public string goodsId;
        public bool isCheck;

        public GoodStateEnum State
        {
            get { return (inventory > 0 ? GoodStateEnum.Available : GoodStateEnum.SoldOut); }
        }

        private string stateStr;
        public string StateStr
        {
            get { return stateStr; }
            set
            {
                stateStr = value;
                this.RaisePropertyChanged("StateStr");
            }
        }


        public string Price
        {
            get { return price.ToString("f2"); }
        }

        public string ItemName
        {
            get { return itemName; }
        }

        public string Inventory
        {
            get { return inventory.ToString(); }
        }

        public string SellNum
        {
            get { return sellNum.ToString(); }
        }
        public bool? IsChecked
        {
            get { return isCheck; }
            set { isCheck = value.Value; }
        }

        public FoodModel()
        {
            isCheck = true;
        }
        public FoodModel(FormInfoQuerygoodDto dto)
        {
            this.categoryName = dto.categoryName;
            this.images = dto.images;
            this.intro = dto.intro;
            this.itemName = dto.itemName;
            this.price = dto.price;
            this.inventory = dto.inventory;
            this.sellNum = dto.sellNum;
            this.goodsId = dto.goodsId;
            this.isCheck = true;
        }

        public void Refresh()
        {
            this.RaisePropertyChanged("Price");
            this.RaisePropertyChanged("State");
            this.RaisePropertyChanged("ItemName");
            this.RaisePropertyChanged("IsChecked");
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
    }

    public enum GoodStateEnum
    {
        SoldOut, Available
    }
}
