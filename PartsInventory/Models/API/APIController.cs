using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Extensions.Options;

using PartsHoleRestLibrary.Requests;

using PartsInventory.Models.API.Models;
using PartsInventory.Models.Enums;
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

      private T? ParseApiResponse<T>(APIResponse<T>? response)
      {
         if (response is null)
         {
            MessageBox.Show("No response from API.");
            return default(T?);
         }
         if (response.Body is null)
         {
            MessageBox.Show($"API {response.Method} Issue: {response.Message}");
            return default(T?);
         }
         return response.Body;
      }

      private IEnumerable<T>? ParseSuccessList<T>(APIResponse<IEnumerable<bool>>? response, IEnumerable<T> models)
      {
         var body = ParseApiResponse(response);
         if (body?.Any(x => x == false) == true)
         {
            var modelList = models.ToList();
            var failedModels = new List<T>();
            var data = body.ToList();
            for (int i = 0; i < data?.Count; i++)
            {
               if (data[i])
                  failedModels.Add(modelList[i]);
            }
            return failedModels;
         }
         return null;
      }
      #endregion

      #region User
      public async Task<IUserModel?> GetUser(IUserModel user)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.UserEndpoint}/{user.Id}", Method.Get);
            var response = await Client.GetAsync<APIResponse<UserApiModel>>(request);
            return ParseApiResponse(response)?.ToModel();
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "GET Error");
            return null;
         }
      }

      public async Task<IUserData?> GetUserData(IUserModel user)
      {
         try
         {
            var modelData = UserApiModel.FromModel(user);
            var request = new RestRequest($"{_apiSettings.Value.UserEndpoint}/data", Method.Post)
            .AddJsonBody(modelData);
            //var response = await Client.PostJsonAsync<UserApiModel, APIResponse<UserData>>($"{_apiSettings.Value.UserEndpoint}/data", UserApiModel.FromModel(user));
            var response = await Client.PostAsync<APIResponse<UserData>>(request);
            return ParseApiResponse(response);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "POST Error");
            return null;
         }
      }

      public async Task<bool> AddModelToUser(string userId, string modelId, ModelIDSelector selector)
      {
         var data = new RequestUpdateListModel{ UserId = userId, ModelId = modelId, PropId = (int)selector};
         var request = new RestRequest($"{_apiSettings.Value.UserEndpoint}/add-model", Method.Post)
            .AddJsonBody(data);
         var response = await Client.PostAsync<APIResponse<bool>>(request);
         return ParseApiResponse(response);
      }

      public async Task<bool> RemoveModelFromUser(string userId, string modelId, ModelIDSelector selector)
      {
         var data = new RequestUpdateListModel{ UserId = userId, ModelId = modelId, PropId = (int)selector};
         var request = new RestRequest($"{_apiSettings.Value.UserEndpoint}/remove-model", Method.Delete)
            .AddJsonBody(data);
         var response = await Client.DeleteAsync<APIResponse<bool>>(request);
         return ParseApiResponse(response);
      }

      public async Task<bool> UpdateUser(IUserModel user)
      {
         user.SyncIDs();
         var request = new RestRequest($"{_apiSettings.Value.UserEndpoint}", Method.Put)
            .AddJsonBody(UserApiModel.FromModel(user));
         var response = await Client.PutAsync<APIResponse<bool>>(request);
         return ParseApiResponse(response);
      }
      #endregion

      #region Parts
      public async Task<PartModel?> GetPart(string id)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}/{id}");
            var response = await Client.GetAsync<APIResponse<PartModel>>(request);
            return ParseApiResponse(response);
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
            var response = await Client.PostAsync<APIResponse<IEnumerable<PartApiModel>>>(request);
            return ParseApiResponse(response)?.Select(x => x.ToModel());
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
            return ParseApiResponse(response);
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
            return ParseSuccessList(response, parts);
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
            return ParseApiResponse(response);
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
            return ParseApiResponse(response);
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
            return ParseApiResponse(response);
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
            return ParseApiResponse(response);
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
            var response = await Client.GetAsync<APIResponse<InvoiceApiModel>>(request);
            return ParseApiResponse(response)?.ToModel();
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
            var response = await Client.PostAsync<APIResponse<IEnumerable<InvoiceApiModel>>>(request);
            return ParseApiResponse(response)?.Select(x => x.ToModel());
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
            var response = await Client.PostAsync<APIResponse<bool>>(request);
            return ParseApiResponse(response);
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
            var response = await Client.PostAsync<APIResponse<IEnumerable<bool>>>(request);
            return ParseSuccessList(response, invoices);
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
            var response = await Client.PutAsync<APIResponse<bool>>(request);
            return ParseApiResponse(response);
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
            var response = await Client.DeleteAsync<APIResponse<bool>>(request);
            return ParseApiResponse(response);
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
            var response = await Client.DeleteAsync<APIResponse<int>>(request);
            return ParseApiResponse(response);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "DELETE Error");
            return 0;
         }
      }

      public async Task<InvoiceModel?> ParseInvoiceFile(string path)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}/files/single", Method.Post)
            .AddFile("file", path, "text/csv");
            var response = await Client.PostAsync<APIResponse<InvoiceApiModel>>(request);
            return ParseApiResponse(response)?.ToModel();
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "File Parse Error");
            throw;
         }
      }

      // BROKEN
      public async Task<IEnumerable<InvoiceModel>?> ParseInvoiceFiles(string[] paths)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}/files/many");
            request.AlwaysMultipartFormData= true;
            foreach (var path in paths)
            {
               request = request.AddFile($"files[{Path.GetFileName(path)}]", path);
               request.AddParameter("key", "value", ParameterType.GetOrPost);
            }
            var response = await Client.PostAsync<APIResponse<IEnumerable<InvoiceApiModel>>>(request);
            return ParseApiResponse(response)?.Select(inv =>  inv.ToModel());
         }
         catch (Exception e)
         {
            MessageBox.Show($"Unknown file parse error.\n{e.Message}", "Error");
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
            var response = await Client.GetAsync<APIResponse<BinModel>>(request);
            return ParseApiResponse(response);
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
            var response = await Client.PostAsync<APIResponse<IEnumerable<BinModel>>>(request);
            return ParseApiResponse(response);
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
            var response = await Client.PostAsync<APIResponse<bool>>(request);
            return ParseApiResponse(response);
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
            if (!bins.Any())
               return null;
            var request = new RestRequest($"{_apiSettings.Value.BinsEndpoint}", Method.Post)
               .AddJsonBody(bins);
            var response = await Client.PostAsync<APIResponse<IEnumerable<bool>>>(request);
            return ParseSuccessList(response, bins);
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
            var response = await Client.PutAsync<APIResponse<bool>>(request);
            return ParseApiResponse(response);
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
            var response = await Client.DeleteAsync<APIResponse<bool>>(request);
            return ParseApiResponse(response);
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
            var response = await Client.DeleteAsync<APIResponse<int>>(request);
            return ParseApiResponse(response);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "DELETE Error");
            return 0;
         }
      }
      #endregion

      #region PartNumbers
      public async Task<PartNumber?> NewPartNumber(string userId, uint fullCategory)
      {
         try
         {
            var pnRequest = new PartNumberRequestModel
            {
               UserId = userId,
               FullCategory = fullCategory,
            };
            var request = new RestRequest($"{_apiSettings.Value.PartNumberEndpoint}/gen")
               .AddJsonBody(pnRequest);
            var response = await Client.PostAsync<APIResponse<PartNumber>>(request);
            return ParseApiResponse(response);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "POST api/partnum/gen Error");
            return null;
         }
      }

      public async Task<PartNumber?> GetPartNumber(string id)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.PartNumberEndpoint}/{id}");
            var response = await Client.GetAsync<APIResponse<PartNumber>>(request);
            return ParseApiResponse(response);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "GET api/partnum/{id} Error");
            return null;
         }
      }

      public async Task<bool> UpdatePartNumber(PartNumber updatedPartNumber)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.PartNumberEndpoint}/{updatedPartNumber.Id}")
               .AddJsonBody(updatedPartNumber);
            var response = await Client.PutAsync<APIResponse<bool>>(request);
            return ParseApiResponse(response);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "PUT api/partnum/{id} Error");
            return false;
         }
      }

      public async Task<bool> DeletePartNumber(string id)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.PartNumberEndpoint}/{id}");
            var response = await Client.DeleteAsync<APIResponse<bool>>(request);
            return ParseApiResponse(response);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "DELETE api/partnum/{id} Error");
            return false;
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
