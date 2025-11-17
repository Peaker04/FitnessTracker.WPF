using FitnessTracker.WPF.Services.Interfaces;
using FitnessTracker.WPF.ViewModels.Base;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessTracker.WPF.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
    {
        private readonly IDatabaseService _databaseService;

        // Properties cho biểu đồ (LiveCharts)
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public StatisticsViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            LoadChartData();
        }

        private async void LoadChartData()
        {
            // Ví dụ: Biểu đồ cột thể hiện thời gian tập trong tuần
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Phút tập luyện",
                    Values = new ChartValues<double> { 10, 50, 39, 50, 30, 60, 0 } // Data giả lập, thực tế lấy từ DB
                }
            };

            Labels = new[] { "T2", "T3", "T4", "T5", "T6", "T7", "CN" };
            Formatter = value => value.ToString("N");

            OnPropertyChanged(nameof(SeriesCollection));
            OnPropertyChanged(nameof(Labels));
        }
    }
}
