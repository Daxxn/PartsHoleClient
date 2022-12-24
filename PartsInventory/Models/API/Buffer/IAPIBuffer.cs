using System.Threading.Tasks;

using PartsInventory.Models.Inventory;

namespace PartsInventory.Models.API.Buffer;

public interface IAPIBuffer
{
   /// <summary>
   /// Adds the provided model to the update buffer. If the model already exists, replaces the old model.
   /// </summary>
   /// <param name="model">Model to add</param>
   void UpdateModel(IModel model);
   /// <summary>
   /// Force the buffer to send all data to the api. Waits for the API to finish all updates before returning.
   /// <para/>
   /// This is meant to be used when <see cref="App.OnExit(System.Windows.ExitEventArgs)"/> triggers to make sure all updates are saved.
   /// </summary>
   public void ForceUpdateAll();
   public Task UpdateAll();
}
