using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Trello.Model;

namespace Trello.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private CardsManager _cardsManager;

        public ObservableCollection<Card> TodoItems { get; private set; }
        public ObservableCollection<Card> DoingItems { get; private set; }
        public ObservableCollection<Card> CompletedItems { get; private set; }

        public ICommand DeleteCommand { get; private set; }

        public HomeViewModel(CardsManager cardsManager)
        {
            _cardsManager = cardsManager;

            DeleteCommand = new RelayCommand<Card>(OnDeleteCommand);

            _cardsManager.Load();

            TodoItems = _cardsManager.TodoItems;
            DoingItems = _cardsManager.DoingItems;
            CompletedItems = _cardsManager.CompletedItems;
        }

        private void OnDeleteCommand(Card card)
        {
            // TODO Delete item and save
            _cardsManager.Save();
        }
    }
}
