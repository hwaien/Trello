using System;
using System.Collections.ObjectModel;
using System.IO;
using Trello.Model;
using System.Xml.Linq;
using System.Text;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using System.Windows;

namespace Trello.ViewModel
{
    public class CardsManager
    {
        string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public enum CollectionEnum
        {
            TodoItems = 0,
            DoingItems,
            CompletedItems
        }

        public ObservableCollection<Card>[] Load()
        {
            ObservableCollection<Card>[] collectionList = new ObservableCollection<Card>[3];

            ObservableCollection<Card> TodoItems = new ObservableCollection<Card>();
            ObservableCollection<Card> DoingItems = new ObservableCollection<Card>();
            ObservableCollection<Card> CompletedItems = new ObservableCollection<Card>();
            
            var path = Path.Combine(docPath, "CardData.xml");

            if (!File.Exists(path))
            {
                File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\CardData.xml"), path);
            }

            XElement CardsXml = XElement.Load(path/*(Path.Combine(docPath, "CardData.xml"))*/);


            IEnumerable<Card> todoItemsList =                
                from el in CardsXml.Elements("Collection")
                .Where(element => element.Attribute("List").Value == "TodoItems").Descendants("Card")
                select new Card(
                    (string)el.Element("Title"),
                    (string)el.Element("Description"),
                    (DateTime)el.Element("CreatedDate"),
                    (DateTime)el.Element("CompletedDate"),
                    (DueState)Enum.Parse(typeof(DueState), (string)el.Element("DueState"))                    
                );

            foreach (Card card in todoItemsList)
            {
                TodoItems.Add(card);
            }

            IEnumerable<Card> DoingItemsList =
                from el in CardsXml.Elements("Collection")
                .Where(element => element.Attribute("List").Value == "DoingItems").Descendants("Card")
                select new Card(
                    (string)el.Element("Title"),
                    (string)el.Element("Description"),
                    (DateTime)el.Element("CreatedDate"),
                    (DateTime)el.Element("CompletedDate"),
                    (DueState)Enum.Parse(typeof(DueState), (string)el.Element("DueState"))                    
                );
            

            foreach (Card card in DoingItemsList)
            {
                DoingItems.Add(card);
            }

            IEnumerable<Card> CompletedItemsList =
                from el in CardsXml.Elements("Collection")
                .Where(element => element.Attribute("List").Value == "CompletedItems").Descendants("Card")
                select new Card(
                    (string)el.Element("Title"),
                    (string)el.Element("Description"),
                    (DateTime)el.Element("CreatedDate"),
                    (DateTime)el.Element("CompletedDate"),
                    (DueState)Enum.Parse(typeof(DueState), (string)el.Element("DueState"))                    
                );


            foreach (Card card in CompletedItemsList)
            {
                CompletedItems.Add(card);
            }

            collectionList[0] = TodoItems;
            collectionList[1] = DoingItems;
            collectionList[2] = CompletedItems;

            return collectionList;

        }

        public void Save(params ObservableCollection<Card>[] collectionList)
        {
            XDocument collectionXDoc = new XDocument();
            XElement rootElement = new XElement("Collections");

                for (int i = 0; i < collectionList.Length; i++)
                {
                XElement collectionElement = new XElement("Collection", 
                    new XAttribute("List", (CollectionEnum)i));

                    foreach (Card card in collectionList[i])
                    {
                        XElement cardElement = new XElement("Card",
                            new XElement("Title", card.Title),
                            new XElement("Description", card.Description),
                            new XElement("CreatedDate", card.CreatedDate),
                            new XElement("CompletedDate", card.CompletedDate),
                            new XElement("DueState", card.DueState)                            
                            );
                        collectionElement.Add(cardElement);
                    }
                    rootElement.Add(collectionElement);
                }
            collectionXDoc.Add(rootElement);
            collectionXDoc.Save(Path.Combine(docPath, "CardData.xml"));

        }
            
    }


}

