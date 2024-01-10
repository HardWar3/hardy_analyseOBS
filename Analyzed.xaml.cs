using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace hardy_analyseOBS
{
    /// <summary>
    /// Interaktionslogik für Analyzed.xaml
    /// </summary>
    public partial class Analyzed : Window
    {
        public Analyzed()
        {
            InitializeComponent();
        }

        public void show_analyze(int cpu_avg, int ram_avg, int gpu_avg)
        {
            cpu_label.Content = String.Format("CPU: \n{0:00} %", cpu_avg);
            ram_label.Content = String.Format("RAM: \n{0:00} %", ram_avg);
            gpu_label.Content = String.Format("GPU: \n{0:00} %", gpu_avg);

            Show();
        }

        public int cpu_gpu_analyse_result(int cpu_avg, int gpu_avg)
        {
            // 10

            return 0;
        }
    }
}
