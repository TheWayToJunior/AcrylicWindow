using LiveCharts;
using LiveCharts.Wpf;

namespace AcrylicWindow.ViewModels.Tabs
{
    public class HomeViewModel : ViewModelBase
    {
        public SeriesCollection _seriesCollection = new SeriesCollection
        {
            new LineSeries
            {
                Values = new ChartValues<double> { 1113, 2225, 1227, 1224, 1225, 3200, 2122, 5200 }
            },
            //new LineSeries
            //{
            //    Values = new ChartValues<decimal> { 2122, 1225, 3136, 1222, 4417, 1225, 1113, 2122 }
            //},

            //new LineSeries
            //{
            //    Values = new ChartValues<decimal> { 1225, 6331, 2122, 7112, 1113, 2225, 1227, 1224 }
            //}
        };

        public SeriesCollection SeriesCollection { get => _seriesCollection; }

        public HomeViewModel()
        {
        }
    }
}
