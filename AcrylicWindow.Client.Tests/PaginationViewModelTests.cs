using AcrylicWindow.ViewModels;
using System.Linq;
using Xunit;

namespace AcrylicWindow.Client.Tests
{
    public class PaginationViewModelTests
    {
        [Fact]
        public void SetCount()
        {
            var testData = new object[] { new { Id = 1 }, new { Id = 2 }, new { Id = 3 } };

            PaginationViewModel viewModel = new((p, s) => { }, pageSize: 2);

            viewModel.SetCount(testData.Length);

            Assert.Equal(2, viewModel.PageCount);
        }
    }
}
