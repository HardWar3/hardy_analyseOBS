using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace hardy_analyseOBS.classes
{
    internal class Gpu
    {
        private ManagementObjectSearcher information = new ManagementObjectSearcher("select * from Win32_VideoController");
        private ManagementObjectSearcher usage = new ManagementObjectSearcher("select * from " +
            "Win32_PerfFormattedData_GPUPerformanceCounters_GPUEngine where Name Like '%engtype_3D'");

        private List<double> usage_list = new List<double>();

        private int count_on_start = 0;
        private int count_on_stop = 0;
        public int get_usage()
        {
            int counter = 0;

            System.Threading.Thread.Sleep(100);

            foreach(ManagementObject item in usage.Get()) 
            {
                counter += Convert.ToInt32(item["UtilizationPercentage"]);
            }

            return counter;
        }

        public string get_name()
        {
            string name = null;

            foreach(ManagementObject item in usage.Get())
            {
                name = item["Name"].ToString();
            }
            
            return name;
        }

        public void add_peak(int peak = -1)
        {
            if (peak < 0)
            {
                int _peak = get_usage();

                usage_list.Add(_peak);
            } else
            {
                usage_list.Add(peak);
            }
        }

        public List<double> get_usage_list()
        {
            return usage_list;
        }

        public int get_last_peak()
        {
            if (usage_list.Count == 0)
            {
                return 0;
            }
            double last_peak = usage_list.Last();

            return Convert.ToInt32(last_peak);
        }

        public int get_usage_average()
        {
            int count = count_on_stop - count_on_start;

            if (count <= 0)
            {
                return 0;
            } else if (count > usage_list.Count || count_on_stop > usage_list.Count)
            {
                count = usage_list.Count - count_on_start;
            }
            List<double> usage_range = usage_list.GetRange(count_on_start, count);
            return Convert.ToInt32(usage_range.Average());
        }

        public void set_count_on_start()
        {
            count_on_start = usage_list.Count();
        }

        public void set_count_on_stop()
        {
            count_on_stop = usage_list.Count();
        }
    }
}
