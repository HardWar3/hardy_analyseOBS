using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics;
using System.Security.Permissions;

namespace hardy_analyseOBS.classes
{
    internal class Ram
    {
        private ManagementObjectSearcher information = new ManagementObjectSearcher("select * from Win32_PhysicalMemory");
        private PerformanceCounter usage = new PerformanceCounter("Memory", "Available Mbytes");

        private List<int> available_percent_list = new List<int>();

        private int count_on_start = 0;
        private int count_on_stop = 0;

        public int get_usage()
        {
            double total = get_total();
            double available = get_available();

            int result = Convert.ToInt32(Math.Round(total - available, 2));

            return result;
        }

        public int get_usage_percent()
        {
            int usage_percent = 100 - get_available_percent();

            return usage_percent;
        }

        public int get_available()
        {
            float first_ram_usage_value = usage.NextValue();

            double ram_available = Convert.ToUInt32(usage.NextValue()) / 1000.0; // 1000.0 führt zur korrekten kommerstelle

            ram_available = Math.Round(ram_available / 1.074, 2); // Gigabyte zu Gibibyte Umrechnung

            return Convert.ToInt32(ram_available);
        }

        public int get_available_percent()
        {
            double total = get_total();
            double available = get_available();

            int result = Convert.ToInt32(Math.Round(available / (total / 100),2));

            return result;
        }

        public double get_total()
        {
            double total = 0;

            foreach (ManagementObject item in information.Get())
            {
                total += Convert.ToDouble(item["Capacity"]) / 1000000000; // 1000000000 führt zur korrekten kommerstelle
            }

            total = Convert.ToInt32(Math.Round(total / 1.074,2));

            return total;
        }

        public string get_name()
        {
            string name = null;

            foreach (ManagementObject item in information.Get())
            {
                name = item["ManuFacturer"].ToString();
            }

            return name;
        }

        public void add_peak(int peak = -1)
        {
            if (peak < 0)
            {
                int _peak = get_available_percent();

                available_percent_list.Add(_peak);
            } else
            {
                available_percent_list.Add(peak);
            }
        }

        public List<int> get_available_percent_list()
        {
            return available_percent_list;
        }

        public int get_last_usage_peak()
        {
            if (available_percent_list.Count == 0)
            {
                return 0;
            }
            int last_peak = 100 - available_percent_list.Last();

            return last_peak;
        }

        public int get_last_available_peak()
        {
            if (available_percent_list.Count == 0)            
            {
                return 0;
            }
            int last_available_peak = available_percent_list.Last();

            return last_available_peak;
        }

        public int get_usage_average()
        {
            int count = count_on_stop - count_on_start;

            if (count == 0)
            {
                return 0;
            } else if (count > available_percent_list.Count || count_on_stop > available_percent_list.Count)
            {
                count = available_percent_list.Count - count_on_start;
            }

            List<int> usage_rang = available_percent_list.GetRange(count_on_start, count);
            int usage = 100 - Convert.ToInt32(usage_rang.Average());
            return usage;
        }

        public void set_count_on_start()
        {
            count_on_start = available_percent_list.Count();
        }

        public void set_count_on_stop()
        {
            count_on_stop = available_percent_list.Count();
        }
    }
}
