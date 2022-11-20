using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Extensions.Options;

using PartsHoleRestLibrary.Requests;

using PartsInventory.Models.API.Models;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.Resources.Settings;

using RestSharp;

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
            return null;
         }
      }

      public async Task<IUserData?> GetUserData(IUserModel user)
      {
         var request = new RestRequest($"{_apiSettings.Value.UserEndpoint}/data", Method.Post)
            .AddJsonBody(UserApiModel.FromModel(user));
         var response = await Client.PostJsonAsync<UserApiModel, APIResponse<UserData>>($"{_apiSettings.Value.UserEndpoint}/data", UserApiModel.FromModel(user));
         //var response = await Client.PostAsync<APIResponse<UserData>>(request);
         if (response is null) return null;
         if (response.Body is null)
         {
            MessageBox.Show(response.Message);
            return null;
         }
         return response.Body;
      }

      public async Task<bool> AddPartToUser(string userId, string partId)
      {
         var data = new AppendRequestModel(){ UserId = userId, ModelId = partId};
         var request = new RestRequest($"{_apiSettings.Value.UserEndpoint}/add-part", Method.Post)
            .AddJsonBody(data);
         var response = await Client.PostAsync<APIResponse<bool>>(request);
         if (response == null) return false;
         if (response.Body == false)
         {
            MessageBox.Show(response.Message);
            return false;
         }
         return true;
      }

      public async Task<bool> AddInvoiceToUser(string userId, string invoiceId)
      {
         var data = new AppendRequestModel(){ UserId = userId, ModelId = invoiceId};
         var request = new RestRequest($"{_apiSettings.Value.UserEndpoint}/add-invoice", Method.Post)
            .AddJsonBody(data);
         var response = await Client.PostAsync<APIResponse<bool>>(request);
         if (response == null)
         {
            MessageBox.Show("Response was null.");
            return false;
         }
         if (response.Body == false)
         {
            MessageBox.Show(response.Message);
            return false;
         }
         return true;
      }

      public async Task<bool> UpdateUser(IUserModel user)
      {
         user.SyncIDs();
         var request = new RestRequest($"{_apiSettings.Value.UserEndpoint}", Method.Put)
            .AddJsonBody(UserApiModel.FromModel(user));
         var response = await Client.PutAsync<APIResponse<bool>>(request);
         if (response == null)
            return false;
         if (response.Body == false)
         {
            MessageBox.Show(response.Message);
            return false;
         }
         return true;
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

      public async Task<IEnumerable<PartModel>?> GetParts(IEnumerable<string> ids)
      {
         try
         {
            var idList = new IdListRequestModel(ids);
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}/many")
               .AddJsonBody(idList);
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
            var response = await Client.PostAsync<APIResponse<bool>>(request);
            if (response == null)
               return false;
            if (response.Body == false)
            {
               MessageBox.Show(response.Message);
               return false;
            }
            return true;
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
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}/many", Method.Post)
               .AddJsonBody(parts.Select(x => PartApiModel.FromModel(x)));
            var response = await Client.PostAsync<APIResponse<IEnumerable<bool>>>(request);
            if (response?.Body?.Any(x => x == false) == true)
            {
               var partData = parts.ToList();
               var failedParts = new List<PartModel>();
               var data = response?.Body.ToList();
               for (int i = 0; i < data?.Count; i++)
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
            var response = await Client.PutAsync<APIResponse<bool>>(request);
            if (response == null)
               return false;
            if (response.Body == false)
            {
               MessageBox.Show(response.Message);
               return false;
            }
            return true;
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "PUT Error");
            return false;
         }
      }

      public async Task<IEnumerable<bool>?> UpdateParts(IEnumerable<PartModel> parts)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}/many", Method.Put)
               .AddJsonBody(parts.Select(x => PartApiModel.FromModel(x)));
            var response = await Client.PutAsync<APIResponse<IEnumerable<bool>>>(request);
            if (response is null) return null;
            if (response.Body is null) return null;
            return response.Body;
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "PUT Error");
            return null;
         }
      }

      public async Task<bool> DeletePart(string id)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}/{id}", Method.Delete);
            var response = await Client.DeleteAsync<APIResponse<bool>>(request);
            if (response == null)
               return false;
            if (response.Body == false)
            {
               MessageBox.Show(response.Message);
               return false;
            }
            return true;
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "DELETE Error");
            return false;
         }
      }

      public async Task<int> DeleteParts(IEnumerable<string> ids)
      {
         try
         {
            var idList = new IdListRequestModel(ids);
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}", Method.Delete)
               .AddJsonBody(idList);
            var response = await Client.DeleteAsync<APIResponse<int>>(request);
            if (response == null)
               return 0;
            if (response.Body == 0)
            {
               MessageBox.Show(response.Message);
               return 0;
            }
            return response.Body;
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

      public async Task<IEnumerable<InvoiceModel>?> GetInvoices(IEnumerable<string> ids)
      {
         try
         {
            var idList = new IdListRequestModel(ids);
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}/many")
               .AddJsonBody(idList);
            var data = await Client.PostAsync<APIResponse<IEnumerable<InvoiceApiModel>>>(request);
            return data?.Body?.Select((d) => d.ToModel());
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
            return (await Client.PostAsync<APIResponse<bool>>(request))?.Body == true;
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
            MessageBox.Show(e.Message, "PUT Error");
            return null;
         }
      }

      public async Task<bool> UpdateInvoice(InvoiceModel invoice)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}/{invoice.Id}", Method.Put)
               .AddJsonBody(InvoiceApiModel.FromModel(invoice));
            return (await Client.PutAsync<APIResponse<bool>>(request))?.Body == true;
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
            return (await Client.DeleteAsync<APIResponse<bool>>(request))?.Body == true;
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "DELETE Error");
            return false;
         }
      }

      public async Task<int> DeleteInvoices(IEnumerable<string> ids)
      {
         try
         {
            var idList = new IdListRequestModel(ids);
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}", Method.Delete)
               .AddJsonBody(idList);
            return (await Client.DeleteAsync<APIResponse<int>>(request))?.Body ?? 0;
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "DELETE Error");
            return 0;
         }
      }

      public async Task<InvoiceModel> ParseInvoiceFile(string path)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}/files/single", Method.Post)
            .AddFile("file", path, "text/csv");
            var newInvoice = (await Client.PostAsync<InvoiceApiModel>(request))?.ToModel();
            return newInvoice ?? throw new Exception("Response from API did not contain an invoice.");
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "File Parse Error");
            throw;
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

      public async Task<IEnumerable<BinModel>?> GetBins(IEnumerable<string> ids)
      {
         try
         {
            var data = new IdListRequestModel(ids);
            var request = new RestRequest($"{_apiSettings.Value.BinsEndpoint}/many", Method.Post)
               .AddJsonBody(data);
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
            return (await Client.PostAsync<APIResponse<bool>>(request))?.Body == true;
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
            return (await Client.PutAsync<APIResponse<bool>>(request))?.Body == true;
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
            return (await Client.DeleteAsync<APIResponse<bool>>(request))?.Body == true;
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "DELETE Error");
            return false;
         }
      }

      public async Task<int> DeleteBins(IEnumerable<string> ids)
      {
         try
         {
            var idList = new IdListRequestModel(ids);
            var request = new RestRequest($"{_apiSettings.Value.BinsEndpoint}", Method.Delete)
               .AddJsonBody(idList);
            return (await Client.DeleteAsync<APIResponse<int>>(request))?.Body ?? 0;
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "DELETE Error");
            return 0;
         }
      }
      #endregion

      #region Testing
      public InvoiceModel? ParseFileTest(string path)
      {
         var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}/files/test", Method.Post)
            .AddFile("file", path, "text/csv");
         return Client.Post<InvoiceApiModel>(request)?.ToModel();
      }
      #endregion

      #endregion
   }
}
