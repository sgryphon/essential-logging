using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HelloWebApp31.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UptimeService _uptimeService;

        public IndexModel(ILogger<IndexModel> logger, UptimeService uptimeService)
        {
            _logger = logger;
            _uptimeService = uptimeService;
        }
        
        public TimeSpan Uptime { get; set; }

        public void OnGet()
        {
            var activity = Activity.Current;
            var context = this.HttpContext;
            Log.GetIndex(_logger, activity?.Id, activity?.TraceId.ToString(), activity?.SpanId.ToString(), activity?.ParentId, activity?.RootId, context?.TraceIdentifier, null);
            Uptime = _uptimeService.GetUptime();
        }
    }
}
