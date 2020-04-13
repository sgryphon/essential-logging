using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HelloWebApp22.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UptimeService _uptimeService;

        public IndexModel(ILogger<IndexModel> logger, UptimeService uptimeService)
        {
            _logger = logger;
            _uptimeService = uptimeService;
            
            Id = Guid.NewGuid();
        }
        
        public Guid Id { get; }
        public TimeSpan Uptime { get; set; }

        public void OnGet()
        {
            var activity = Activity.Current;
            var context = this.HttpContext;
            Log.GetIndex(_logger, Id, activity?.Id, activity?.ParentId, activity?.RootId, context?.TraceIdentifier, null);
            Uptime = _uptimeService.GetUptime();
        }
    }
}
