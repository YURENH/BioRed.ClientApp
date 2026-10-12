using System;
using System.Collections.Generic;
using System.Text;

namespace BioRed.Core.DTOs
{
    public class QuickActionDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Icon { get; set; } = "💊";
        public string ActionCode { get; set; } = string.Empty;
        public string BackgroundColor { get; set; } = "#F5F5F5";
    }
}