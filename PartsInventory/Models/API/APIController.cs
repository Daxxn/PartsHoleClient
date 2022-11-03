using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;

using PartsInventory.Models.API.Models;
using PartsInventory.Models.Enums;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.Resources.Settings;

namespace PartsInventory.Models.API
{
   public class APIController : IAPIController
   {
      #region Local Props
      private readonly IOptions<APISettings> _apiSettings;
      private static HttpClient Client { get; set; } = null!;
      #endregion

      #region Constructors
      public APIController(IOptions<APISettings> apiSettings)
      {
         _apiSettings = apiSettings;
         if (Client == null)
         {
            InitializeClient(apiSettings);
         }
      }
      #endregion

      #region Methods
      private static void InitializeClient(IOptions<APISettings> apiSettings)
      {
         Client = new HttpClient();
         Client.BaseAddress = new Uri(BuildBaseUrl(apiSettings));
         Client.DefaultRequestHeaders.Accept.Clear();
         Client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
      }

      private static string BuildBaseUrl(IOptions<APISettings> apiSettings)
      {
         if (apiSettings.Value.BaseUrl.Contains("{Port}"))
         {
            return apiSettings.Value.BaseUrl.Replace("{Port}", apiSettings.Value.Port.ToString());
         }
         return apiSettings.Value.BaseUrl;
      }

      public async Task<UserModel?> GetPartsCollection()
      {
         try
         {
            var response = await Client.GetAsync($"{_apiSettings.Value.PartsEndpoint}/collection");
            if (response == null)
               return null;
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<UserModel>() : null;
         }
         catch (Exception)
         {
            throw;
         }
      }

      public async Task<PartModel?> GetPart(string id)
      {
         try
         {
            var response = await Client.GetAsync($"{_apiSettings.Value.PartsEndpoint}/{id}");
            if (response == null)
               return null;
            return response.IsSuccessStatusCode ? (await response.Content.ReadFromJsonAsync<PartApiModel>())?.Convert() : null;
         }
         catch (Exception)
         {
            throw;
         }
      }

      public async Task<IEnumerable<InvoiceModel>?> GetInvoices(string userID)
      {
         var response = await Client.GetAsync($"{_apiSettings.Value.InvoicesEndpoint}/{userID}");
         if (response == null)
            return null;
         return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<IEnumerable<InvoiceModel>>() : null;
      }

      public async Task<InvoiceModel?> GetInvoice(string id)
      {
         var response = await Client.GetAsync($"{_apiSettings.Value.PartsEndpoint}/{id}");
         if (response == null)
            return null;
         return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<InvoiceModel>() : null;
      }
      #endregion

      #region Full Props

      #endregion
   }
}
