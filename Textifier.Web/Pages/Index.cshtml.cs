using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Textifier.Web.Pages
{

    public class TextifierForm
    {
        public string Url { get; set; }

        public string Text { get; set; }
    }

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public TextifierForm TextifierForm { get; set; }

        public string TextifierResponse { get; set; } 

        public void OnGet(string url)
        {
            if (!string.IsNullOrWhiteSpace(url)) {
                TextifierForm = new TextifierForm();
                TextifierForm.Url = url;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            string url = TextifierForm.Url;

            string myJson = "{\"text\": \"" + TextifierForm.Text + "\"}";
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(
                    url,
                    new StringContent(myJson, Encoding.UTF8, "application/json"));

                string jsonstring = await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject<dynamic>(jsonstring);
                TextifierResponse = json.html;
            }

            return Page();
        }
    }
}
