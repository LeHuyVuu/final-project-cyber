using System.Collections.Generic;

namespace cybersoft_final_project.DTOs
{
    public class ChartDataDto
    {
        public string[] Labels { get; set; }
        public List<ChartDataset> Datasets { get; set; }
    }

    public class ChartDataset
    {
        public string Label { get; set; }
        public List<decimal> Data { get; set; }
        public List<string>? BackgroundColor { get; set; }
    }
}