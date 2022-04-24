using Firebase.Firestore;
using Swibbl.Services;
using System;
using System.Collections.ObjectModel;

namespace Swibbl.Droid.Extensions
{
    public static class DocumentReferenceExtensions
    {
        public static T Convert<T>(this DocumentSnapshot doc) where T : IIdentifiable
        {
            try
            {
                var jsonStr = Newtonsoft.Json.JsonConvert.SerializeObject(doc.Data.ToDictionary());
                var item = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonStr);
                item.Id = doc.Id;
                return item;
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION THROWN");
            }
            return default;
        }

        public static ObservableCollection<T> Convert<T>(this QuerySnapshot docs) where T : IIdentifiable
        {
            var list = new ObservableCollection<T>();
            foreach (var doc in docs.Documents)
            {
                list.Add(doc.Convert<T>());
            }
            return list;
        }
    }
}
