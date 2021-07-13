using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHR
{
    /// <summary>
    /// Convert the ApplicationPage to an actual view\page
    /// </summary>
    public class ApplicatoinUserControlValueConverter : BaseValueConverter<ApplicatoinUserControlValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ApplicationUserControl)value)
            {
                case ApplicationUserControl.StaffRecruitment:
                    return new StaffRecruitmentPanel();

                case ApplicationUserControl.Adaptation:
                    return new AdaptationPanel();

                default:
                    //Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
