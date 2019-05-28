using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using Trello.Model;

namespace Trello.ViewModel
{
    public class CardsManager
    {
        public ObservableCollection<Card> TodoItems { get; private set; }
        public ObservableCollection<Card> DoingItems { get; private set; }
        public ObservableCollection<Card> CompletedItems { get; private set; }

        public void Load()
        {
            TodoItems = new ObservableCollection<Card>(Load("todo.json"));
            DoingItems = new ObservableCollection<Card>(Load("doing.json"));
            CompletedItems = new ObservableCollection<Card>(Load("done.json"));
        }

        public void Save()
        {
            Save(TodoItems, "todo.json");
            Save(DoingItems, "doing.json");
            Save(CompletedItems, "done.json");
        }

        public void Save(IEnumerable<Card> items, string path)
        {
            var contents = JsonConvert.SerializeObject(items);
            File.WriteAllText(path, contents);
        }

        public IEnumerable<Card> Load(string path)
        {
            var contents = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<IEnumerable<Card>>(contents);
        }
    }
}
