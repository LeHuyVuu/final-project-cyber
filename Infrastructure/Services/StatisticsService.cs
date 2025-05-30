using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cybersoft_final_project.DTOs;
using cybersoft_final_project.Infrastructure.UnitOfWork;

namespace cybersoft_final_project.Services
{
    public class StatisticsService
    {
        private readonly UnitOfWork _unit;

        public StatisticsService(UnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<ChartDataDto> GetSummaryChartDataAsync()
        {
            var totalUsers = await _unit.UserRepository.GetTotalUsers();
            var totalProducts = await _unit.ProductRepository.GetAllWithIncludes()
                .CountAsync(p => p.status == true);
            var totalOrders = (await _unit.OrderRepository.GetAllAsync()).Count();

            return new ChartDataDto
            {
                Labels = new[] { "Người dùng", "Sản phẩm", "Đơn hàng" },
                Datasets = new List<ChartDataset>
                {
                    new ChartDataset
                    {
                        Label = "Tổng số lượng",
                        Data = new List<decimal> { totalUsers, totalProducts, totalOrders },
                        BackgroundColor = new List<string> { "#42A5F5", "#66BB6A", "#FFA726" }
                    }
                }
            };
        }

    }
}
