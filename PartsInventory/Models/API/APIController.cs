using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Extensions.Options;

using PartsInventory.Models.API.Interfaces;
using PartsInventory.Models.API.Models;
using PartsInventory.Models.API.RequestModels;
using PartsInventory.Models.Enums;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.Resources.Settings;
using Newtonsoft.Json;
using RestSharp;
using System.Windows.Shapes;

namespace PartsInventory.Models.API
{
   /// <summary>
   /// Main intermediary between the application and the Rest client.
   /// </summary>
   public class APIController : IAPIController
   {
      #region Local Props
      /// <summary>
      /// Settings for the API, including the base path and the endpoints.
      /// </summary>
      private readonly IOptions<APISettings> _apiSettings;
      /// <summary>
      /// Rest client that wraps the HttpClient (Devil class). Making requests alot simpler.
      /// </summary>
      private static RestClient Client { get; set; } = null!;
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
      #region Helpers
      /// <summary>
      /// Creates a new Rest Client and initializes it.
      /// </summary>
      /// <param name="apiSettings">API Endpoints</param>
      private static void InitializeClient(IOptions<APISettings> apiSettings)
      {
         Client = new RestClient(BuildBaseUrl(apiSettings));
      }

      /// <summary>
      /// Replaces the port placeholder with the actual port.
      /// <para>
      /// This is only a problem because the API keeps changing its port.
      /// Probably something to do with https.
      /// </para>
      /// <para>Not used in Production.</para>
      /// </summary>
      /// <param name="apiSettings">API Endpoints</param>
      /// <returns>Base Url string.</returns>
      private static string BuildBaseUrl(IOptions<APISettings> apiSettings)
      {
         if (apiSettings.Value.BaseUrl.Contains("{Port}"))
         {
            return apiSettings.Value.BaseUrl.Replace("{Port}", apiSettings.Value.Port.ToString());
         }
         return apiSettings.Value.BaseUrl;
      }
      #endregion

      #region User
      public async Task<IUserModel?> GetUser(IUserModel user)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.UserEndpoint}/{user.Id}", Method.Get);
            return (await Client.GetAsync<UserApiModel>(request))?.ToModel();
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "GET Error");
            throw;
         }
      }

      public async Task<IUserData?> GetUserData(IUserModel user)
      {
         var request = new RestRequest($"{_apiSettings.Value.UserEndpoint}/data", Method.Post)
            .AddJsonBody(UserApiModel.FromModel(user));
         return await Client.PostAsync<UserData>(request);
      }
      #endregion

      #region Parts
      public async Task<PartModel?> GetPart(string id)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}/{id}");
            return await Client.GetAsync<PartModel>(request);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "GET Error");
            return null;
         }
      }
      
      public async Task<IEnumerable<PartModel>?> GetParts(string[] ids)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}/many")
               .AddJsonBody(ids);
            var data = await Client.PostAsync<IEnumerable<PartApiModel>>(request);
            return data?.Select(x => x.ToModel());
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "GET Error");
            return null;
         }
      }
      
      public async Task<bool> CreatePart(PartModel part)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}", Method.Post)
               .AddJsonBody(PartApiModel.FromModel(part));
            return await Client.PostAsync<bool>(request);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "GET Error");
            return false;
         }
      }
      
      public async Task<IEnumerable<PartModel>?> CreateParts(IEnumerable<PartModel> parts)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}", Method.Post)
               .AddJsonBody(parts.Select(x => PartApiModel.FromModel(x)));
            var response = await Client.PostAsync<IEnumerable<bool>>(request);
            if (response?.Any(x => x == false) == true)
            {
               var partData = parts.ToList();
               var failedParts = new List<PartModel>();
               var data = response.ToList();
               for (int i = 0; i < data.Count; i++)
               {
                  if (data[i])
                     failedParts.Add(partData[i]);
               }
               return failedParts;
            }
            return null;
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "POST Error");
            return null;
         }
      }
      
      public async Task<bool> UpdatePart(PartModel part)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}", Method.Put)
               .AddJsonBody(PartApiModel.FromModel(part));
            return await Client.PutAsync<bool>(request);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "PUT Error");
            return false;
         }
      }
      
      public async Task<bool> DeletePart(string id)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}/{id}", Method.Delete);
            return await Client.DeleteAsync<bool>(request);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "DELETE Error");
            return false;
         }
      }
      
      public async Task<int> DeleteParts(string[] ids)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}", Method.Delete)
               .AddJsonBody(ids);
            return await Client.DeleteAsync<int>(request);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "DELETE Error");
            return 0;
         }
      }
      #endregion

      #region Invoices
      public async Task<InvoiceModel?> GetInvoice(string id)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}/{id}");
            return (await Client.GetAsync<InvoiceApiModel>(request))?.ToModel();
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "POST Error");
            return null;
         }
      }

      public async Task<IEnumerable<InvoiceModel>?> GetInvoices(string[] ids)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}/many").AddJsonBody(ids);
            var data = await Client.PostAsync<IEnumerable<InvoiceApiModel>>(request);
            return data?.Select((d) => d.ToModel());
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "POST Error");
            return null;
         }
      }

      public async Task<bool> CreateInvoice(InvoiceModel invoice)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}", Method.Post)
               .AddJsonBody(invoice);
            return await Client.PostAsync<bool>(request);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "POST Error");
            return false;
         }
      }

      public async Task<IEnumerable<InvoiceModel>?> CreateInvoices(IEnumerable<InvoiceModel> invoices)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}", Method.Post)
               .AddJsonBody(invoices.Select(x => InvoiceApiModel.FromModel(x)));
            var response = await Client.PostAsync<IEnumerable<bool>>(request);
            if (response?.Any(x => x == false) == true)
            {
               var partData = invoices.ToList();
               var failedParts = new List<InvoiceModel>();
               var data = response.ToList();
               for (int i = 0; i < data.Count; i++)
               {
                  if (data[i])
                     failedParts.Add(partData[i]);
               }
               return failedParts;
            }
            return null;
         }
         catch (Exception e)
         {

            return null;
         }
      }

      public async Task<bool> UpdateInvoice(InvoiceModel invoice)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}", Method.Put)
               .AddJsonBody(InvoiceApiModel.FromModel(invoice));
            return await Client.PutAsync<bool>(request);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "PUT Error");
            return false;
         }
      }

      public async Task<bool> DeleteInvoice(string id)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}/{id}", Method.Delete);
            return await Client.DeleteAsync<bool>(request);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "DELETE Error");
            return false;
         }
      }

      public async Task<int> DeleteInvoices(string[] ids)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}", Method.Delete)
               .AddJsonBody(ids);
            return await Client.DeleteAsync<int>(request);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "DELETE Error");
            return 0;
         }
      }
      #endregion

      #region Bins
      public async Task<BinModel?> GetBin(string id)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.BinsEndpoint}/{id}");
            return await Client.GetAsync<BinModel>(request);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "GET Error");
            return null;
         }
      }

      public async Task<IEnumerable<BinModel>?> GetBins(string[] ids)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.BinsEndpoint}/many")
               .AddJsonBody(ids);
            return await Client.PostAsync<IEnumerable<BinModel>>(request);
            //return data?.Select(x => x.ToModel());
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "GET Error");
            return null;
         }
      }

      public async Task<bool> CreateBin(BinModel bin)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.BinsEndpoint}", Method.Post)
               .AddJsonBody(bin);
            return await Client.PostAsync<bool>(request);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "GET Error");
            return false;
         }
      }

      public async Task<IEnumerable<BinModel>?> CreateBins(IEnumerable<BinModel> bins)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.BinsEndpoint}", Method.Post)
               .AddJsonBody(bins);
            var response = await Client.PostAsync<IEnumerable<bool>>(request);
            if (response?.Any(x => x == false) == true)
            {
               var partData = bins.ToList();
               var failedParts = new List<BinModel>();
               var data = response.ToList();
               for (int i = 0; i < data.Count; i++)
               {
                  if (data[i])
                     failedParts.Add(partData[i]);
               }
               return failedParts;
            }
            return null;
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "POST Error");
            return null;
         }
      }

      public async Task<bool> UpdateBin(BinModel bin)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.BinsEndpoint}", Method.Put)
               .AddJsonBody(bin);
            return await Client.PutAsync<bool>(request);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "PUT Error");
            return false;
         }
      }

      public async Task<bool> DeleteBin(string id)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.BinsEndpoint}/{id}", Method.Delete);
            return await Client.DeleteAsync<bool>(request);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "DELETE Error");
            return false;
         }
      }

      public async Task<int> DeleteBins(string[] ids)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.BinsEndpoint}", Method.Delete)
               .AddJsonBody(ids);
            return await Client.DeleteAsync<int>(request);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "DELETE Error");
            return 0;
         }
      }
      #endregion

      #endregion
   }
}
