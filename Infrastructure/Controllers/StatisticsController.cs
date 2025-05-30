using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using cybersoft_final_project.Services;

namespace cybersoft_final_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly StatisticsService _statisticsService;

        public StatisticsController(StatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("summary-chart")]
        public async Task<IActionResult> GetSummaryChart()
        {
            var chartData = await _statisticsService.GetSummaryChartDataAsync();
            return Ok(new
            {
                status = 200,
                message = "Dữ liệu chart thống kê tổng quan",
                data = chartData
            });
        }

    }
}