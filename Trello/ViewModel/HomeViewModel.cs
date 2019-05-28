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

        public ICommand MoveToDoCommand { get; private set; }

        public ICommand MoveToNextCommand { get; private set; }

        public ICommand MoveToPriorCommand { get; private set; }

        public HomeViewModel(CardsManager cardsManager)
        {
            _cardsManager = cardsManager;           

            DeleteCommand = new RelayCommand<Card>(OnDeleteCommand);
            MoveToDoCommand = new RelayCommand<Card>(OnMoveToDoCommand);

            MoveToNextCommand = new RelayCommand<Card>(OnMoveToNextCommand);
            MoveToPriorCommand = new RelayCommand<Card>(OnMoveToPriorCommand);

            TodoItems = _cardsManager.Load()[0];
            DoingItems = _cardsManager.Load()[1];
            CompletedItems = _cardsManager.Load()[2];

            // TODO Delete CreateTodoItems when Load is implemented
            //CreateTodoItems();
        }

        private void OnDeleteCommand(Card card)
        {
            if (TodoItems != null) { TodoItems.Remove(card); }
            if (DoingItems!= null) { DoingItems.Remove(card); }
            if (CompletedItems != null) { CompletedItems.Remove(card); }
            _cardsManager.Save(TodoItems,DoingItems,CompletedItems);
        }

        private void OnMoveToNextCommand(Card card)
        {
            if (!CompletedItems.Contains(card))
            {
                if (TodoItems.Contains(card))
                {
                    TodoItems.Remove(card);
                    DoingItems.Add(card);
                }

                else if (DoingItems.Contains(card))
                {
                    DoingItems.Remove(card);
                    CompletedItems.Add(card);
                }
            }
            _cardsManager.Save(TodoItems, DoingItems, CompletedItems);
        }

        private void OnMoveToPriorCommand(Card card)
        {
            if (!TodoItems.Contains(card))
            {
                if (DoingItems.Contains(card))
                {
                    DoingItems.Remove(card);
                    TodoItems.Add(card);
                }

                else if (CompletedItems.Contains(card))
                {
                    CompletedItems.Remove(card);
                    DoingItems.Add(card);
                }
            }
            _cardsManager.Save(TodoItems, DoingItems, CompletedItems);
        }

        private void OnMoveToDoCommand (Card card)
        {
            if (!TodoItems.Contains(card))
            {
                DoingItems.Remove(card);
                CompletedItems.Remove(card);
                TodoItems.Add(card);
            }
            _cardsManager.Save(TodoItems, DoingItems, CompletedItems);
        }

        private void OnMoveDoingCommand (Card card)
        {
            if (!DoingItems.Contains(card))
            {
                TodoItems.Remove(card);
                CompletedItems.Remove(card);
                DoingItems.Add(card);
            }
            _cardsManager.Save(TodoItems, DoingItems, CompletedItems);
        }

        private void OnMoveCompleteComamnd (Card card)
        {
            if (!CompletedItems.Contains(card))
            {
                TodoItems.Remove(card);
                DoingItems.Remove(card);
                CompletedItems.Add(card);
            }
            _cardsManager.Save(TodoItems, DoingItems, CompletedItems);
        }

        private void CreateTodoItems()
        {
            TodoItems = new ObservableCollection<Card>();

            var card = new Card("Buy tickets", "Caravan Palace in 'The Observatory'");
            card.DueState = DueState.Expired;
            TodoItems.Add(card);

            card = new Card("Rent", "$150, Due by 01/01/2020");
            TodoItems.Add(card);

            card = new Card("Groceries list", "Milk, bread, buttermilk, feta cheese, olives.");
            TodoItems.Add(card);

            DoingItems = new ObservableCollection<Card>();
            card = new Card("Read a book", "Neon Prey by John Sandford");
            DoingItems.Add(card);
        }
    }
}
