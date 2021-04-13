using AcrylicWindow.Client.Core.Helpers;
using AcrylicWindow.Client.Core.IContract;
using LiveCharts;
using LiveCharts.Wpf;

namespace AcrylicWindow.ViewModels.Tabs
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly IAuthorizationProvider _authorizationProvider;

        private SeriesCollection _seriesCollection = new SeriesCollection
        {
            new LineSeries
            {
                Values = new ChartValues<double> { 1113, 2225, 1227, 1224, 1225, 3200, 2122, 5200, 1113, 2225, 1227, 1224, 1225, 3200, 2122, 5200 }
            }
        };

        public SeriesCollection SeriesCollection { get => _seriesCollection; }

        private decimal _earnings;

        public decimal Earnings
        {
            get => _earnings;
            set => Set(ref _earnings, value);
        }

        private decimal _earningsPercentage;

        public decimal EarningsPercentage
        {
            get => _earningsPercentage;
            set => Set(ref _earningsPercentage, value);
        }

        private string _name;

        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }


        public HomeViewModel(IAuthorizationProvider authorizationProvider)
        {
            _authorizationProvider = Has.NotNull(authorizationProvider);

            Earnings = 13000;
            EarningsPercentage = 12.5m;

            Name = authorizationProvider.AuthenticationState.GetClaim("name");
        }
    }
}
