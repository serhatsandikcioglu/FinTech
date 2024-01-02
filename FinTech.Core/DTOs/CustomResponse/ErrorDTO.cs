using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinTech.Core.DTOs.CustomResponse
{
    public class ErrorDTO
    {
        [JsonPropertyName("errorcode")] public int ErrorCode { get; set; }

        [JsonPropertyName("title")] public string Title { get; set; }

        [JsonPropertyName("details")] public List<string> Details { get; set; }

        [JsonPropertyName("stacktrace")] public string? StackTrace { get; set; }
    }
}
