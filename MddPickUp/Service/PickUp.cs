using MddPickUp.Models;
using MddPickUp.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using static MddPickUp.Service.Web;

namespace MddPickUp.Service
{
    public static class PickUp
    {
        public static FormInfoQueryResult FormInfoQuery(string formNo)
        {
            Dictionary<string, string> d = Web.PostCommonHeaders();
            d.Add("x-wx-id", Account.id);
            d.Add("x-wx-skey", Account.key);
            var res = Get(BasicInfo.BaseUri + "/fanTan/api/v2/getFormInfo?formNo=" + formNo + "&ver=3.1.5", d);
            if (!res.success) return new FormInfoQueryResult(false, "查询Form信息失败");

            FormInfoQueryDto json;
            try
            {
                json = Newtonsoft.Json.JsonConvert.DeserializeObject<FormInfoQueryDto>(res.result);
            }
            catch //(Exception ex)
            {
                return new FormInfoQueryResult(false, "反序列化失败");
            }
            
            if (json.code != 0)
            {
                return new FormInfoQueryResult(false, json.msg);
            }

            var foods = new List<FoodModel>();
            FoodModel c = null;
            foreach (var fooddto in json.data.waFanTanForm.goodsWidgets)
            {
                c = new FoodModel(fooddto);
                foods.Add(c);
            }
            return new FormInfoQueryResult(foods, "成功");
        }

        public static GroupInfoQueryResult GroupInfoQuery(string groupNo)
        {
            Dictionary<string, string> d = Web.PostCommonHeaders();
            d.Add("x-wx-id", Account.id);
            d.Add("x-wx-skey", Account.key);
            int page = 1;
            bool hasmore = true;
            var forms = new List<FormModel>();

            while (hasmore)
            {
                var res = Get(BasicInfo.BaseUri + "/fanTan/api/v2/group/getInfoFlow?groupNo=" + groupNo + "&page=" + page.ToString() + "&ver=3.1.5", d);
                if (!res.success) return new GroupInfoQueryResult(false, "查询Group信息失败");

                GroupInfoQueryDto json;
                try
                {
                    json = Newtonsoft.Json.JsonConvert.DeserializeObject<GroupInfoQueryDto>(res.result);
                }
                catch //(Exception ex)
                {
                    return new GroupInfoQueryResult(false, "反序列化失败");
                }

                if (json.code != 0)
                {
                    return new GroupInfoQueryResult(false, json.msg);
                }

                FormModel c = null;
                foreach (var recorddto in json.data.sendRecords)
                {
                    c = new FormModel(recorddto.waFanTanForm);
                    forms.Add(c);
                }

                page++;
                hasmore = json.data.hasMore;
            }
            return new GroupInfoQueryResult(forms, "成功");
        }
    }

}
