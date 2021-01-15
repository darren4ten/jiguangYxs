using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Logic.Util.Extension
{
    public static class ObservableCollectionExtension
    {
        /// <summary>
        /// 扩展AddRange
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="items"></param>
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            if (items != null)
            {
                foreach (var item in items)
                {
                    collection.Add(item);
                }
            }
        }

        public static void RemoveAll<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {

        }
        
    }
}
