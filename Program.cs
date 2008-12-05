using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZipCSharp
{
    public static class E
    {
        public static IEnumerable<TResult> Zip<T1, T2, TResult>(this IEnumerable<T1> s1, IEnumerable<T2> s2,
            Func<T1, T2, TResult> func)
        {
            if (s1.Count() > s2.Count()) 
                s1 = s1.Take(s2.Count());

            return s1.SelectMany((__, idxS1) => s2.Skip(idxS1).Take(1),
                    (itemS1, itemS2) => func(itemS1, itemS2));
        }

        /// <summary>
        /// Originally from http://community.bartdesmet.net/blogs/bart/archive/2008/11/03/c-4-0-feature-focus-part-3-intermezzo-linq-s-new-zip-operator.aspx
        /// </summary>
        public static IEnumerable<TResult> ZipB<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> func)
        {
            return first.Select((x, i) => new { X = x, I = i }).Join(second.Select((x, i) => new { X = x, I = i }), o => o.I, i => i.I, (o, i) => func(o.X, i.X));
        }

        public static string Print<A>(this IEnumerable<A> list)
        {
            Func<string, string, string> x = (Func<string, string, string>)delegate(string str, string ele) {
                return str + ", " + ele;
            };

            var result = list.Select((elem) => elem.ToString()).Aggregate(x);
            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<int> first = (new int[] { 1, 2, 3 }).ToList();
            List<int> second = (new int[] { 10, 11, 12, 13 }).ToList();

            Console.WriteLine("Add corresponding elements: " + first.Zip(second, (a, b) => a + b).Print());
            Console.WriteLine("Print as pairs: " + first.Zip(second, (a, b) => "(" + a + ", " + b + ")").Print());
            Console.WriteLine("Reversed pairs: " + first.Zip(second, (a, b) => "(" + b + ", " + a + ")").Print());
            Console.WriteLine("Two-element lists: " + 
                first.Zip(second, (a, b) => (new int[] {a, b}).ToList()).Select((list) => "[" + list.Print() + "]").Print());

            Console.WriteLine("(ZipB) Add corresponding elements: " + first.ZipB(second, (a, b) => a + b).Print());
            Console.WriteLine("(ZipB) Print as pairs: " + first.ZipB(second, (a, b) => "(" + a + ", " + b + ")").Print());
            Console.WriteLine("(ZipB) Reversed pairs: " + first.ZipB(second, (a, b) => "(" + b + ", " + a + ")").Print());
            Console.WriteLine("(ZipB) Two-element lists: " +
                first.ZipB(second, (a, b) => (new int[] { a, b }).ToList()).Select((list) => "[" + list.Print() + "]").Print());
        }
    }
}
