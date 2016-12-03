using System.Collections.Generic;

namespace KohonenMVC.Kohonen
{
    public class Data
    {
        public List<int> data;

        public Data(int[] data)
        {
            this.data = new List<int>();
            for (int i = 0; i < data.Length; i++)
                this.data.Add(data[i]);
        }

        public override string ToString()
        {
            string s = "";
            foreach (double d in data)
            {
                s = string.Concat(s, d + " ");
            }
            return s;
        }
    }
}
