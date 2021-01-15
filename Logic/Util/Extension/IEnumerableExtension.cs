using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Logic.Util.Extension
{
    public static class IEnumerableExtension
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerableList)
        {
            return new ObservableCollection<T>(enumerableList);
        }
    }
}
