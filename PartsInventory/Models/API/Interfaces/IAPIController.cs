using System.Collections.Generic;
using System.Threading.Tasks;

using PartsInventory.Models.API.Models;
using PartsInventory.Models.Inventory;
using PartsInventory.Models.Inventory.Main;

using MongoDB.Bson;

namespace PartsInventory.Models.API
{
   public interface IAPIController
   {
      #region Methods
      #region User
      /// <summary>
      /// Gets <see cref="UserModel"/> data stored on the server. Called after login.
      /// <para>
      /// GET /api/User/{id}
      /// </para>
      /// </summary>
      /// <param name="user">Incomplete <see cref="UserModel"/> data after login finishes.</param>
      /// <returns>Combined user data from server.</returns>
      Task<UserModel?> GetUser(UserModel user);
      /// <summary>
      /// Gets the <see cref="UserData"/> after login. Should be called ONCE per login.
      /// <para>
      /// POST /api/User/data
      /// </para>
      /// <para>
      /// Body : <see cref="UserModel"/> <paramref name="user"/>
      /// </para>
      /// </summary>
      /// <param name="user">Newly logged in user.</param>
      /// <returns>All <see cref="UserData"/>.</returns>
      Task<UserData?> GetUserData(UserModel user);
      #endregion
      #region Parts
      /// <summary>
      /// Gets a <see cref="PartModel"/> by <see cref="ObjectId"/>.
      /// <para>
      /// GET /api/Parts/{id}
      /// </para>
      /// </summary>
      /// <param name="id"><see cref="ObjectId"/> string</param>
      /// <returns>Found <see cref="PartModel"/>.</returns>
      Task<PartModel?> GetPart(string id);
      /// <summary>
      /// Gets a <see cref="List{T}"/> of <see cref="PartModel"/>s by <see cref="ObjectId"/>.
      /// <para>
      /// POST /api/Parts/many
      /// </para>
      /// <para>
      /// Body : <see cref="List{T}"/> <see cref="PartModel"/> <paramref name="ids"/>
      /// </para>
      /// </summary>
      /// <param name="ids"><see cref="List{T}"/> of <see cref="ObjectId"/>s.</param>
      /// <returns><see cref="List{T}"/> of <see cref="PartModel"/>s.</returns>
      Task<IEnumerable<PartModel>?> GetParts(string[] ids);
      /// <summary>
      /// Creates a new <see cref="PartModel"/>.
      /// <para>
      /// POST /api/Parts/
      /// </para>
      /// <para>
      /// Body : <see cref="PartModel"/>
      /// </para>
      /// </summary>
      /// <param name="part">Newly created <see cref="PartModel"/></param>
      /// <returns>True if successful.</returns>
      Task<bool> CreatePart(PartModel part);
      /// <summary>
      /// Creates multiple <see cref="PartModel"/>
      /// <para>
      /// POST /api/Parts/many
      /// </para>
      /// <para>
      /// Body : <see cref="List{T}"/> <paramref name="parts"/>
      /// </para>
      /// </summary>
      /// <param name="parts">Array of new <see cref="PartModel"/>.</param>
      /// <returns>Array of <seealso cref="bool"/> with each index matching the <paramref name="parts"/> array.</returns>
      Task<IEnumerable<PartModel>?> CreateParts(IEnumerable<PartModel> parts);
      /// <summary>
      /// Updates a changed <paramref name="part"/>
      /// <para>
      /// PUT /api/Parts/{id}
      /// </para>
      /// <para>
      /// Body : <see cref="PartModel"/> <paramref name="part"/>
      /// </para>
      /// </summary>
      /// <param name="part">Changed <see cref="PartModel"/></param>
      /// <returns>True if successful.</returns>
      Task<bool> UpdatePart(PartModel part);
      /// <summary>
      /// Deletes a <see cref="PartModel"/>.
      /// <para>
      /// DELETE /api/Parts/{id}
      /// </para>
      /// </summary>
      /// <param name="id"><see cref="ObjectId"/> string</param>
      /// <returns>True if successful.</returns>
      Task<bool> DeletePart(string id);
      /// <summary>
      /// Deletes multiple <see cref="PartModel"/>s.
      /// <para>
      /// DELETE /api/Parts
      /// </para>
      /// <para>
      /// Body : <see cref="List{T}"/> <seealso cref="string"/> <paramref name="ids"/>
      /// </para>
      /// </summary>
      /// <param name="ids"><see cref="List{T}"/> of <see cref="ObjectId"/> strings.</param>
      /// <returns>Number of <see cref="PartModel"/>s deleted.</returns>
      Task<int> DeleteParts(string[] ids);
      #endregion
      #region Invoices
      /// <summary>
      /// Gets an <see cref="InvoiceModel"/> by ObjectId.
      /// <para>
      /// GET /api/Invoices/{id}
      /// </para>
      /// </summary>
      /// <param name="id"><see cref="ObjectId"/> string</param>
      /// <returns><see cref="InvoiceModel"/>, only contains part ID <seealso cref="string"/>s.</returns>
      Task<InvoiceModel?> GetInvoice(string id);
      /// <summary>
      /// Gets an array of Invoices by ObjectId.
      /// <para>
      /// POST /api/invoices/many
      /// </para>
      /// <para>
      /// Body : <see cref="List{T}"/> <paramref name="ids"/>
      /// </para>
      /// </summary>
      /// <param name="ids">List of <see cref="ObjectId"/>s</param>
      /// <returns>Array of <see cref="InvoiceModel"/>s, only contains part IDs.</returns>
      Task<IEnumerable<InvoiceModel>?> GetInvoices(string[] ids);
      /// <summary>
      /// Creates a new <see cref="InvoiceModel"/>.
      /// <para>
      /// POST /api/Invoices
      /// </para>
      /// <para>
      /// Body : <see cref="InvoiceModel"/> <paramref name="invoice"/>
      /// </para>
      /// </summary>
      /// <param name="invoice">Newly created <see cref="InvoiceModel"/></param>
      /// <returns>True if successful.</returns>
      Task<bool> CreateInvoice(InvoiceModel invoice);
      /// <summary>
      /// Updates an <see cref="InvoiceModel"/>.
      /// <para>
      /// PUT /api/Invoices/{id}
      /// </para>
      /// <para>
      /// Body : <see cref="InvoiceModel"/> <paramref name="invoice"/>
      /// </para>
      /// </summary>
      /// <param name="invoice"></param>
      /// <returns>True if successful.</returns>
      Task<bool> UpdateInvoice(InvoiceModel invoice);
      /// <summary>
      /// Creates new <see cref="InvoiceModel"/>s.
      /// <para>
      /// POST /api/Invoices
      /// </para>
      /// <para>
      /// Body : <see cref="List{T}"/> <see cref="InvoiceModel"/> <paramref name="invoices"/>
      /// </para>
      /// </summary>
      /// <param name="invoices"><see cref="List{T}"/> of newly created <see cref="InvoiceModel"/>s.</param>
      /// <returns>Array of <seealso cref="bool"/> with each index matching the <paramref name="invoices"/> <see cref="List{T}"/>.</returns>
      Task<IEnumerable<InvoiceModel>?> CreateInvoices(IEnumerable<InvoiceModel> invoices);
      /// <summary>
      /// Deletes an <see cref="InvoiceModel"/>
      /// <para>
      /// DELETE /api/Invoices/{id}
      /// </para>
      /// </summary>
      /// <param name="id"><see cref="ObjectId"/> string</param>
      /// <returns>True if successful.</returns>
      Task<bool> DeleteInvoice(string id);
      /// <summary>
      /// Delete multiple <see cref="InvoiceModel"/>s
      /// <para>
      /// DELETE /api/Invoices
      /// </para>
      /// <para>
      /// Body : <see cref="List{T}"/> <see cref="ObjectId"/> <paramref name="ids"/>
      /// </para>
      /// </summary>
      /// <param name="ids"><see cref="List{T}"/> of <see cref="ObjectId"/> strings.</param>
      /// <returns>Number of <see cref="InvoiceModel"/>s deleted.</returns>
      Task<int> DeleteInvoices(string[] ids);
      #endregion
      #region Bins
      /// <summary>
      /// Gets a <see cref="BinModel"/> by <see cref="ObjectId"/>.
      /// <para>
      /// GET /api/Bins/{id}
      /// </para>
      /// </summary>
      /// <param name="id"><see cref="ObjectId"/> string</param>
      /// <returns>Found <see cref="BinModel"/>.</returns>
      Task<BinModel?> GetBin(string id);
      /// <summary>
      /// Gets a <see cref="List{T}"/> of <see cref="BinModel"/>s by <see cref="ObjectId"/>.
      /// <para>
      /// POST /api/Bins/many
      /// </para>
      /// <para>
      /// Body : <see cref="List{T}"/> <see cref="BinModel"/> <paramref name="ids"/>
      /// </para>
      /// </summary>
      /// <param name="ids"><see cref="List{T}"/> of <see cref="ObjectId"/>s.</param>
      /// <returns><see cref="List{T}"/> of <see cref="BinModel"/>s.</returns>
      Task<IEnumerable<BinModel>?> GetBins(string[] ids);
      /// <summary>
      /// Creates a new <see cref="BinModel"/>.
      /// <para>
      /// POST /api/Bins/
      /// </para>
      /// <para>
      /// Body : <see cref="BinModel"/>
      /// </para>
      /// </summary>
      /// <param name="bin">Newly created <see cref="BinModel"/></param>
      /// <returns>True if successful.</returns>
      Task<bool> CreateBin(BinModel bin);
      /// <summary>
      /// Creates multiple <see cref="BinModel"/>
      /// <para>
      /// POST /api/Bins/many
      /// </para>
      /// <para>
      /// Body : <see cref="List{T}"/> <paramref name="bins"/>
      /// </para>
      /// </summary>
      /// <param name="bins"><see cref="List{T}"/> of new <see cref="BinModel"/>.</param>
      /// <returns><see cref="List{T}"/> of <seealso cref="bool"/> with each index matching the <paramref name="bins"/>.</returns>
      Task<IEnumerable<BinModel>?> CreateBins(IEnumerable<BinModel> bins);
      /// <summary>
      /// Updates a changed <paramref name="bin"/>
      /// <para>
      /// PUT /api/Bins/{id}
      /// </para>
      /// <para>
      /// Body : <see cref="BinModel"/> <paramref name="bin"/>
      /// </para>
      /// </summary>
      /// <param name="bin">Changed <see cref="BinModel"/></param>
      /// <returns>True if successful.</returns>
      Task<bool> UpdateBin(BinModel bin);
      /// <summary>
      /// Deletes a <see cref="BinModel"/>.
      /// <para>
      /// DELETE /api/Bins/{id}
      /// </para>
      /// </summary>
      /// <param name="id"><see cref="ObjectId"/> string</param>
      /// <returns>True if successful.</returns>
      Task<bool> DeleteBin(string id);
      /// <summary>
      /// Deletes multiple <see cref="BinModel"/>s.
      /// <para>
      /// DELETE /api/Bins
      /// </para>
      /// <para>
      /// Body : <see cref="List{T}"/> <seealso cref="string"/> <paramref name="ids"/>
      /// </para>
      /// </summary>
      /// <param name="ids"><see cref="List{T}"/> of <see cref="ObjectId"/> strings.</param>
      /// <returns>Number of <see cref="BinModel"/>s deleted.</returns>
      Task<int> DeleteBins(string[] ids);
      #endregion
      #endregion
   }
}