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

using Microsoft.Extensions.Options;

using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

using PartsInventory.Models.API.Interfaces;
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

      public async Task<IEnumerable<PartModel>?> GetParts(string[] ids)
      {
         try
         {
            var response = await Client.PostAsync($"{_apiSettings.Value.PartsEndpoint}", JsonContent.Create(ids));
            if (response == null)
               return null;
            return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<IEnumerable<PartModel>>() : null;
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

      public async Task<IEnumerable<InvoiceModel>?> GetInvoices(string[] ids)
      {
         var data = await Post<IEnumerable<InvoiceApiModel>, string[]>($"{_apiSettings.Value.InvoicesEndpoint}", ids);
         return data?.Select((d) => d.Convert());
      }

      public async Task<InvoiceModel?> GetInvoice(string id)
      {
         return (await Get<InvoiceApiModel>($"{_apiSettings.Value.UserEndpoint}/{id}"))?.Convert();
      }

      public async Task<UserModel?> GetUser(UserModel user)
      {
         return (await Get<UserApiModel>($"{ _apiSettings.Value.UserEndpoint}/{ user.Id}"))?.Convert();
      }

      public async Task<UserData?> GetUserData(UserModel user)
      {

         return await Post<UserData, UserModel>($"{_apiSettings.Value.UserEndpoint}/data", user);
      }

      private static async Task<TData?> Get<TData>(string url) where TData : class
      {
         var response = await Client.GetAsync(url);
         return response?.IsSuccessStatusCode == true ? await response.Content.ReadFromJsonAsync<TData>() : null;
      }

      private static async Task<TData?> Post<TData, TInput>(string url, TInput input) where TData : class where TInput : class
      {
         // Well this didnt work...
         // The doc writer needs some args?? IDK...
         // Probably just going to implement another converter method in UserApiModel.
         var writer = new BsonDocumentWriter(new());
         BsonSerializer.Serialize(writer, input);
         var str = writer.ToJson();
         var response = await Client.PostAsJsonAsync(url, str);
         return response?.IsSuccessStatusCode == true ? await response.Content.ReadFromJsonAsync<TData>() : null;
      }

      private static async Task<TData?> Put<TData, TInput>(string url, TInput input) where TData : class
      {
         var response = await Client.PutAsJsonAsync(url, input);
         return response?.IsSuccessStatusCode == true ? await response.Content.ReadFromJsonAsync<TData>() : null;
      }

      private static async Task<bool> Delete(string url)
      {
         var response = await Client.DeleteAsync(url);
         return response?.IsSuccessStatusCode == true;
      }
      #endregion

      #region Full Props

      #endregion
   }
}
