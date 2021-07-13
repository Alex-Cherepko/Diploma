using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace MyHR
{
    
    /// <summary>
    /// A base value converter that allows direct XAML usage
    /// Can use like static recource and binding
    /// </summary>
    /// <typeparam name="T"> The type of the value converter</typeparam>
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
        where T: class, new() 
    {
        #region Private Members

        /// <summary>
        /// A single static instanse(пример, образец) of this value converter 
        /// </summary>
        private static T mConverter = null;

        #endregion

        #region Markup Extension Methods

        /// <summary>
        /// Provide a static instance of the value converter
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return mConverter ?? (mConverter = new T());
        }

        #endregion

        #region Value converter methods

        /// <summary>
        /// A methods to convert one type to another
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// The method to convert value back to it's source type
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
        
        #endregion
    }
   
}
