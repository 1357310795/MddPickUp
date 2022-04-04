using MddPickUp.Helpers;
using MddPickUp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace MddPickUp.Converters
{
    class GoodStateToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is GoodStateEnum state)
            {
                switch (state)
                {
                    case GoodStateEnum.Available:
                        return new SolidColorBrush(ThemeHelper.StringToColor("#4CAF50"));//绿色
                    case GoodStateEnum.SoldOut:
                        return new SolidColorBrush(ThemeHelper.StringToColor("#F44336"));//红色
                    //case GoodStateEnum.NotFound:
                    //    return new SolidColorBrush(ThemeHelper.StringToColor("#FF5722"));//橙色
                    //case GoodStateEnum.Ready:
                    //    return new SolidColorBrush(ThemeHelper.StringToColor("#2196F3"));//蓝色
                    //case GoodStateEnum.Unknow:
                    //    return new SolidColorBrush(ThemeHelper.StringToColor("#607D8B"));//灰色
                    //case GoodStateEnum.Known:
                    //    return new SolidColorBrush(ThemeHelper.StringToColor("#673AB7"));//紫色
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public static class GoodStateToBrush
    {
        public static SolidColorBrush Convert(GoodStateEnum state)
        {
            switch (state)
            {
                case GoodStateEnum.Available:
                    return new SolidColorBrush(ThemeHelper.StringToColor("#4CAF50"));//绿色
                case GoodStateEnum.SoldOut:
                    return new SolidColorBrush(ThemeHelper.StringToColor("#F44336"));//红色
                //case GoodStateEnum.NotFound:
                //    return new SolidColorBrush(ThemeHelper.StringToColor("#FF5722"));//橙色
                //case GoodStateEnum.Ready:
                //    return new SolidColorBrush(ThemeHelper.StringToColor("#2196F3"));//蓝色
                //case GoodStateEnum.Unknow:
                //    return new SolidColorBrush(ThemeHelper.StringToColor("#607D8B"));//灰色
                //case GoodStateEnum.Known:
                //    return new SolidColorBrush(ThemeHelper.StringToColor("#673AB7"));//紫色
            }
            return new SolidColorBrush(Colors.Black);
        }
    }
}
