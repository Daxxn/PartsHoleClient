using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Extensions.Options;

using NPOI.SS.Formula.Functions;

using PartsHoleRestLibrary.Requests;

using PartsInventory.Models.API.Models;
using PartsInventory.Models.Enums;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;
using PartsInventory.Resources.Settings;
using PartsInventory.Utils.Messager;

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
      private readonly IMessageService _messageService;
      /// <summary>
      /// Rest client that wraps the HttpClient (Devil class). Making requests alot simpler.
      /// </summary>
      private static RestClient Client { get; set; } = null!;
      #endregion

      #region Constructors
      public APIController(IOptions<APISettings> apiSettings, IMessageService messageService)
      {
         _apiSettings = apiSettings;
         _messageService = messageService;
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
         var options = new RestClientOptions
         {
            BaseUrl = new(BuildBaseUrl(apiSettings)),
            //ThrowOnDeserializationError = true,
            //ThrowOnAnyError = false,
         };
         Client = new RestClient(options);
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

      private T? ParseApiResponse<T>(RestResponse<APIResponse<T>?> response)
      {
         if (!response.IsSuccessful)
         {
            var errorResponse = Client.Deserialize<APIException>(response).Data;
         }
         if (response.Data is null)
         {
            _messageService.AddMessage("No response from API.", Severity.Warning);
            return default(T?);
         }
         var body = response.Data!.Body;
         if (body is null)
         {
            _messageService.AddMessage($"API {response.Data.Method} Issue: {response.Data.Message}", Severity.Error);
            return default(T?);
         }
         return body;
      }

      private IEnumerable<T>? ParseSuccessList<T>(RestResponse<APIResponse<IEnumerable<bool>>?> response, IEnumerable<T> models)
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

      private async Task<T?> SendRequest<T>(RestRequest request)
      {
         var response = await Client.ExecuteAsync<APIResponse<T>?>(request);
         if (response.IsSuccessful)
         {
            return ParseApiResponse(response);
         }
         var data = Client.Deserialize<APIException>(response).Data;
         _messageService.AddMessage($"API request error : {data?.Title}", Severity.Error);
         return default;
      }

      private async Task<IEnumerable<T>?> SendListRequest<T>(RestRequest request, IEnumerable<T> models)
      {
         var response = await Client.ExecuteAsync<APIResponse<IEnumerable<bool>>?>(request);
         if (response.IsSuccessful)
         {
            return ParseSuccessList(response, models);
         }
         var data = Client.Deserialize<APIException>(response).Data;
         _messageService.AddMessage($"API request error : {data?.Title}");
         return default;
      }
      #endregion

      #region User
      public async Task<IUserModel?> GetUser(IUserModel user)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.UserEndpoint}/{user.Id}", Method.Get);
            return (await SendRequest<UserApiModel>(request))?.ToModel();
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API GET Error: {e.Message}", Severity.Error);
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
            return await SendRequest<UserData>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API POST Error: {e.Message}", Severity.Error);
            return null;
         }
      }

      public async Task<bool> AddModelToUser(string userId, string modelId, ModelIDSelector selector)
      {
         var data = new RequestUpdateListModel{ UserId = userId, ModelId = modelId, PropId = (int)selector};
         var request = new RestRequest($"{_apiSettings.Value.UserEndpoint}/add-model", Method.Post)
            .AddJsonBody(data);
         return await SendRequest<bool>(request);
      }

      public async Task<bool> RemoveModelFromUser(string userId, string modelId, ModelIDSelector selector)
      {
         var data = new RequestUpdateListModel{ UserId = userId, ModelId = modelId, PropId = (int)selector};
         var request = new RestRequest($"{_apiSettings.Value.UserEndpoint}/remove-model", Method.Delete)
            .AddJsonBody(data);
         return await SendRequest<bool>(request);
      }

      public async Task<bool> UpdateUser(IUserModel user)
      {
         user.SyncIDs();
         var request = new RestRequest($"{_apiSettings.Value.UserEndpoint}", Method.Put)
            .AddJsonBody(UserApiModel.FromModel(user));
         return await SendRequest<bool>(request);
      }
      #endregion

      #region Parts
      public async Task<PartModel?> GetPart(string id)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}/{id}", Method.Get);
            return await SendRequest<PartModel>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API GET Error: {e.Message}", Severity.Error);
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
            return (await SendRequest<IEnumerable<PartApiModel>>(request))?.Select(x => x.ToModel());
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API POST Error: {e.Message}", Severity.Error);
            return null;
         }
      }

      public async Task<bool> CreatePart(PartModel part)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}", Method.Post)
               .AddJsonBody(PartApiModel.FromModel(part));
            return await SendRequest<bool>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API POST Error: {e.Message}", Severity.Error);
            return false;
         }
      }

      public async Task<IEnumerable<PartModel>?> CreateParts(IEnumerable<PartModel> parts)
      {
         try
         {
            var newParts = parts.Select(x => PartApiModel.FromModel(x));
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}/many", Method.Post)
               .AddJsonBody(parts);
            return await SendRequest<IEnumerable<PartModel>>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API POST Error: {e.Message}", Severity.Error);
            return null;
         }
      }

      public async Task<bool> UpdatePart(PartModel part)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}", Method.Put)
               .AddJsonBody(PartApiModel.FromModel(part));
            return await SendRequest<bool>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API PUT Error: {e.Message}", Severity.Error);
            return false;
         }
      }

      public async Task<IEnumerable<bool>?> UpdateParts(IEnumerable<PartModel> parts)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}/many", Method.Put)
               .AddJsonBody(parts.Select(x => PartApiModel.FromModel(x)));
            return await SendRequest<IEnumerable<bool>>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API PUT Error: {e.Message}", Severity.Error);
            return null;
         }
      }

      public async Task<bool> DeletePart(string id)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.PartsEndpoint}/{id}", Method.Delete);
            return await SendRequest<bool>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API DELETE Error: {e.Message}", Severity.Error);
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
            return await SendRequest<int>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API DELETE Error: {e.Message}", Severity.Error);
            return 0;
         }
      }
      #endregion

      #region Invoices
      public async Task<InvoiceModel?> GetInvoice(string id)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}/{id}", Method.Get);
            return await SendRequest<InvoiceModel?>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API GET Error: {e.Message}", Severity.Error);
            return null;
         }
      }

      public async Task<IEnumerable<InvoiceModel>?> GetInvoices(IEnumerable<string> ids)
      {
         try
         {
            var idList = new IdListRequestModel(ids);
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}/many", Method.Post)
               .AddJsonBody(idList);
            return await SendRequest<IEnumerable<InvoiceModel>>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API POST Error: {e.Message}", Severity.Error);
            return null;
         }
      }

      public async Task<bool> CreateInvoice(InvoiceModel invoice)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}", Method.Post)
               .AddJsonBody(invoice);
            return await SendRequest<bool>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API POST Error: {e.Message}", Severity.Error);
            return false;
         }
      }

      public async Task<IEnumerable<InvoiceModel>?> CreateInvoices(IEnumerable<InvoiceModel> invoices)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}", Method.Post)
               .AddJsonBody(invoices);
            return await SendRequest<IEnumerable<InvoiceModel>>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API POST Error: {e.Message}", Severity.Error);
            return null;
         }
      }

      public async Task<bool> UpdateInvoice(InvoiceModel invoice)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}/{invoice.Id}", Method.Put)
               .AddJsonBody(invoice);
            return await SendRequest<bool>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API PUT Error: {e.Message}", Severity.Error);
            return false;
         }
      }

      public async Task<bool> DeleteInvoice(string id)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}/{id}", Method.Delete);
            return await SendRequest<bool>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API DELETE Error: {e.Message}", Severity.Error);
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
            return await SendRequest<int>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API DELETE Error: {e.Message}", Severity.Error);
            return 0;
         }
      }

      public async Task<InvoiceModel?> ParseInvoiceFile(string path)
      {
         try
         {
            var ext = Path.GetExtension(path);
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}/files/single", Method.Post)
            .AddFile("file", path, ext == ".csv" ? "text/csv" : null);
            return await SendRequest<InvoiceModel?>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API POST Error: {e.Message}", Severity.Error);
            throw;
         }
      }

      // BROKEN
      public async Task<IEnumerable<InvoiceModel>?> ParseInvoiceFiles(string[] paths)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.InvoicesEndpoint}/files/many", Method.Post);
            request.AlwaysMultipartFormData = true;
            foreach (var path in paths)
            {
               request = request.AddFile($"files[{Path.GetFileName(path)}]", path);
               request.AddParameter("key", "value", ParameterType.GetOrPost);
            }
            return await SendRequest<IEnumerable<InvoiceModel>>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API POST Error: {e.Message}", Severity.Error);
            throw;
         }
      }
      #endregion

      #region Bins
      public async Task<BinModel?> GetBin(string id)
      {
         try
         {
            var request = new RestRequest($"{_apiSettings.Value.BinsEndpoint}/{id}", Method.Get);
            return await SendRequest<BinModel>(request);
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
            return await SendRequest<IEnumerable<BinModel>>(request);
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
            return await SendRequest<bool>(request);
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
            return await SendRequest<IEnumerable<BinModel>>(request);
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
            return await SendRequest<bool>(request);
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
            return await SendRequest<bool>(request);
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
            return await SendRequest<int>(request);
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
            var request = new RestRequest($"{_apiSettings.Value.PartNumberEndpoint}/gen", Method.Post)
               .AddJsonBody(pnRequest);
            return await SendRequest<PartNumber>(request);
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
            var request = new RestRequest($"{_apiSettings.Value.PartNumberEndpoint}/{id}", Method.Get);
            return await SendRequest<PartNumber>(request);
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
            var request = new RestRequest($"{_apiSettings.Value.PartNumberEndpoint}/{updatedPartNumber.Id}", Method.Put)
               .AddJsonBody(updatedPartNumber);
            return await SendRequest<bool>(request);
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
            var request = new RestRequest($"{_apiSettings.Value.PartNumberEndpoint}/{id}", Method.Delete);
            return await SendRequest<bool>(request);
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
         return Client.Post<InvoiceModel>(request);
      }

      public async Task<InvoiceModel?> PostInvoiceTest(InvoiceModel invoice)
      {
         try
         {
            var request = new RestRequest("testing/invoice-test", Method.Post)
               .AddJsonBody(invoice);
            return await SendRequest<InvoiceModel>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API TEST Error: {e.Message}", Severity.Error);
            throw;
         }
      }

      public async Task<InvoiceModel?> PostInvoiceTest2()
      {
         try
         {
            var testInvoice = new InvoiceModel()
            {
               OrderNumber = 9999,
               SupplierType = SupplierType.DigiKey,
               IsAddedToParts = false,
               PartModels = new()
               {
                  new()
                  {
                     PartNumber = "Test-Part-1",
                     Reference = "9999-0001",
                     Description = "Test part 1",
                     Quantity= 1,
                     SupplierPartNumber = "Test-Part-1 ND",
                     UnitPrice = 1
                  },
                  new()
                  {
                     PartNumber = "Test-Part-2",
                     Reference = "9999-0002",
                     Description = "Test part 2",
                     Quantity= 2,
                     SupplierPartNumber = "Test-Part-2 ND",
                     UnitPrice = 2
                  },
                  new()
                  {
                     PartNumber = "Test-Part-3",
                     Reference = "9999-0003",
                     Description = "Test part 3",
                     Quantity= 3,
                     SupplierPartNumber = "Test-Part-3 ND",
                     UnitPrice = 3
                  }
               }
            };
            var request = new RestRequest("testing/invoice-test", Method.Post)
               .AddJsonBody(testInvoice);
            return await SendRequest<InvoiceModel>(request);
         }
         catch (Exception e)
         {
            _messageService.AddMessage($"API TEST Error: {e.Message}", Severity.Error);
            throw;
         }
      }
      #endregion

      #endregion
   }
}
