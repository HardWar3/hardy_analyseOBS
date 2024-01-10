using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace hardy_analyseOBS.classes
{
    internal abstract class AnalyseElement
    {
        private ManagementObjectSearcher information = new ManagementObjectSearcher();

        private List<int> usage_list = new List<int>();

        private int count_on_start = 0;
        private int count_on_stop = 0;

        public abstract int get_usage();

        public virtual string get_name()
        {
            string name = null;
            foreach (ManagementObject item in information.Get())
            {
                name = item["name"].ToString();
            }
            return name;
        }

        public virtual void add_peak(int peak = -1)
        {
            if(peak < 0)
            {
                int _peak = get_usage();
                usage_list.Add(_peak);
            } else
            {
                usage_list.Add(peak);
            }
        }

        public virtual List<int> get_usage_list()
        {
            return usage_list;
        }

        public virtual int get_last_peak()
        {
            if (usage_list.Count == 0)
            {
                return 0;
            }
            int last_peak = usage_list.Last();
            return Convert.ToInt32(last_peak);
        }

        public virtual int get_usage_average()
        {
            int count = count_on_stop - count_on_start;
            if (count == 0)
            {
                return 0;
            } else if (count > usage_list.Count || count_on_stop > usage_list.Count)
            {
                count = usage_list.Count - count_on_start;
            }
            List<int> usage_range = usage_list.GetRange(count_on_start, count);
            return Convert.ToInt32(usage_range.Average());
        }

        public virtual void set_count_on_start()
        {
            count_on_start = usage_list.Count();
        }

        public virtual void set_count_on_stop()
        {
            count_on_stop = usage_list.Count();
        }

    }
}
