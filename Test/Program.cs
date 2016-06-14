using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//Вы собираетесь совершить долгое путешествие через множество населенных пунктов.Чтобы не запутаться, вы сделали карточки вашего путешествия.Каждая карточка содержит в себе пункт отправления и пункт назначения.
//Гарантируется, что если упорядочить эти карточки так, чтобы для каждой карточки в упорядоченном списке пункт назначения на ней совпадал с пунктом отправления в следующей карточке в списке, получится один список карточек без циклов и пропусков. 
//Например, у нас есть карточки
//Мельбурн > Кельн
//Москва > Париж
//Кельн > Москва
//Если упорядочить их в соответствии с требованиями выше, то получится следующий список: 
//Мельбурн > Кельн, Кельн > Москва, Москва > Париж
//Требуется: 
//Написать функцию, которая принимает набор неупорядоченных карточек и возвращает набор упорядоченных карточек в соответствии с требованиями выше, то есть в возвращаемом из функции списке карточек для каждой карточки пункт назначения на ней должен совпадать с пунктом отправления на следующей карточке.
//Дать оценку сложности получившегося алгоритма сортировки
//Написать тесты

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var cards = GenerateTestList(100);
            foreach(var card in cards)
                Console.WriteLine(card);

            Console.WriteLine("------------------sorted----------------");

            var sortedCards = SortCards(cards);
            foreach (var card in sortedCards)
                Console.WriteLine(card);

            Console.ReadKey();
        }

        //сложность алгоритма 2 * O(N) + 2 * O(Log 2 N)
        static IEnumerable<Card> SortCards(List<Card> cards)
        {
            var fromDict = new Dictionary<string, ListNode>();
            var toDict = new Dictionary<string, ListNode>();
            foreach (var card in cards)
            {
                var newNode = new ListNode(card);
                fromDict[card.From] = newNode;
                toDict[card.To] = newNode;

                ListNode nextNode;
                if (fromDict.TryGetValue(card.To, out nextNode))
                {
                    nextNode.Previous = newNode;
                    newNode.Next = nextNode;
                }

                ListNode prevNode;
                if (toDict.TryGetValue(card.From, out prevNode))
                {
                    prevNode.Next = newNode;
                    newNode.Previous = prevNode;
                }
            }

            var node = fromDict.Values.First(x => x.Previous == null);
            while (node != null)
            {
                yield return node.Card;
                node = node.Next;
            }
        }


        static List<Card> GenerateTestList(int count)
        {
            var result = new List<Card>();
            for (int i = 0; i < count; i++)
            {
                result.Add(new Card
                {
                    From = string.Format("City{0}", i),
                    To = string.Format("City{0}", i + 1)
                });
            }
            var rnd = new Random(DateTime.Now.Millisecond);
            return result.OrderBy(x => rnd.Next()).ToList();
        }

    }

    public class Card
    {
        public string From { get; set; }

        public string To { get; set; }

        public override string ToString()
        {
            return string.Format("{0} -> {1}", From, To);
        }
    }

    public class ListNode
    {
        public Card Card { get; set; }

        public ListNode Previous { get; set; }//нужно чтобы определить начало последовательности

        public ListNode Next { get; set; }

        public ListNode(Card card)
        {
            Card = card;
        }
    }
}
