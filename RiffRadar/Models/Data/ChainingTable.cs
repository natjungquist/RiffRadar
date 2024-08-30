using RiffRadar.Models.Data.Responses;

namespace RiffRadar.Models.Data
{
    public class ChainingTable
    {
        private Dictionary<object, List<string>> ht;

        public ChainingTable()
        {
            ht = new();
        }

        public int Length()
        {
            return ht.Count;
        }
        public bool isEmpty()
        {
            return ht.Count == 0;
        }
        public int size()
        {
            return ht.Count;
        }
        public void Add(object key, object val)
        {
            if (key is Track && val is string)
            { 
                Track track = (Track)key;
                string genre = (string)val;

                if (!ht.ContainsKey(track))
                {
                    ht.Add(track, new List<string>());
                } 
                ht[track].Add(genre);
            }

        }
        public void Add(object key, List<string> val)
        {
            if (key is Track && val is List<string>)
            {
                Track track = (Track)key;
                List<string> genresList = val;

                if (!ht.ContainsKey(track))
                {
                    ht.Add(track, genresList);
                } else
                {
                    ht[track].AddRange(genresList);
                }
            }

        }
    
        public bool ContainsKey(object key)
        {
            return ht.ContainsKey(key);
        }
        public bool ContainsValue(object val)
        {
            foreach (var pair in ht)
            {
                if (pair.Value.Contains(val)) return true;
            }
            return false;
        }
        public object GetKey(object val)
        {
            foreach (var pair in ht)
            {
                if (pair.Value.Contains(val))
                {
                    return pair.Key;
                }
            }
            return null;
        }
        public List<object> GetKeys()
        {
            List<object> allKeys = new();
            foreach (var pair in ht)
            {
                allKeys.Add(pair.Key);
            }
            return allKeys;
        }
        public List<string> GetValues(object key)
        {
            return ht[key];
        }
        public object RemoveValue(object val)
        {
            foreach (var pair in ht)
            {
                if (pair.Value.Contains(val))
                {
                    pair.Value.Remove((string)val);
                    return val;
                }
            }
            return null;
        }
    }
}
