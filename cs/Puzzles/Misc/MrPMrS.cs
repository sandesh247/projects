
using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

namespace Misc
{
	[TestFixture]
    public class MrPAndMrS
    {
        [Test]
        public void Solve()
        {
            /*
            We pick two numbers a and b, so that a>=b and both numbers are within the range [2,99]. 
            We give Mr.P the product a*b and give Mr.S the sum a+b.
            The following dialog takes place:

            Mr.P: I don’t know the numbers.
            Mr.S: I knew you didn’t know. I don’t know either.
            Mr.P: Now I know the numbers.
            Mr.S: Now I know them too.

            Can we find the numbers a and b?
            */

            # region Declaration

            List<int> numbers = new List<int>(Xrange(2, 100, i => ++i));

            var sums = new Dictionary<SamePair<int>, int>();
            var products = new Dictionary<SamePair<int>, int>();

            var pairsOfSum = new Dictionary<int, List<SamePair<int>>>();
            var pairsOfProduct = new Dictionary<int, List<SamePair<int>>>();

            #endregion

            # region Initialization

            foreach (var i in numbers)
            {
                foreach (var j in (from all_j in numbers where all_j <= i select all_j))
                {
                    var currentPair = new SamePair<int>(i, j);

                    sums[currentPair] = i + j;
                    products[currentPair] = i * j;

                    if (!pairsOfSum.ContainsKey(sums[currentPair]))
                    {
                        pairsOfSum.Add(sums[currentPair], new List<SamePair<int>>());
                    }

                    pairsOfSum[sums[currentPair]].Add(new SamePair<int>(i, j));

                    if (!pairsOfProduct.ContainsKey(products[currentPair]))
                    {
                        pairsOfProduct.Add(products[currentPair], new List<SamePair<int>>());
                    }

                    pairsOfProduct[products[currentPair]].Add(new SamePair<int>(i, j));
                }
            }

            #endregion

            var PDoesntKnow = (from pair in pairsOfProduct
                               where pair.Value.Count > 1
                               select pair.Key).ToArray();

            var SDoesntKnow = (from pair in pairsOfSum
                               where pair.Value.All(productPair => PDoesntKnow.Contains(productPair.First * productPair.Second))
                               where pair.Value.Count > 1
                               select pair.Key).ToArray();

            var PNowKnows = (from product in PDoesntKnow
                             where (from pair in pairsOfProduct[product]
                                    where SDoesntKnow.Contains(pair.First + pair.Second)
                                    select pair).Count() == 1
                             select product).ToArray();

            var SNowKnows = (from sum in SDoesntKnow
                             where (from pair in pairsOfSum[sum]
                                    where PNowKnows.Contains(pair.First * pair.Second)
                                    select pair).Count() == 1
                             select sum).ToArray();

            var solutionProduct = (from product in PNowKnows
                                   where (from pair in pairsOfProduct[product]
                                          where SNowKnows.Contains(pair.First + pair.Second)
                                          select pair).Count() == 1
                                   select product).ToArray();

            var solutionPairs = from product in solutionProduct
                                from productPair in pairsOfProduct[product]
                                where (from sum in SNowKnows
                                       from sumPair in pairsOfSum[sum]
                                       select sumPair).Contains(productPair)
                                select productPair;

            Console.WriteLine("Pairs :");
            foreach (var pair in solutionPairs)
            {
                Console.WriteLine("{0}, {1}", pair.First.ToString(), pair.Second.ToString());
            }


        }

        public static IEnumerable<T> Xrange<T>(T begin, T stop, Func<T, T> next) where T : IComparable<T>
        {
            for (T i = begin; i.CompareTo(stop) < 0; i = next(i))
            {
                yield return i;
            }
        }

        public class Pair<T1, T2> : IEquatable<Pair<T1, T2>>
        {
            public T1 First { get; set; }
            public T2 Second { get; set; }

            public Pair()
            {
            }

            public Pair(T1 first, T2 second)
            {
                this.First = first;
                this.Second = second;
            }

            #region IEquatable<Pair<T1,T2>> Members

            public bool Equals(Pair<T1, T2> other)
            {
                return this.First.Equals(other.First) && this.Second.Equals(other.Second);
            }

            #endregion
        }

        public class SamePair<T> : Pair<T, T>, IEquatable<SamePair<T>>
        {
            public SamePair()
            {
            }

            public SamePair(T first, T second)
            {
                this.First = first;
                this.Second = second;
            }

            #region IEquatable<SamePair<T>> Members

            public bool Equals(SamePair<T> other)
            {
                return 
                    (this.First.Equals(other.First) && this.Second.Equals(other.Second))
                    || 
                    (this.First.Equals(other.Second) && this.Second.Equals(other.First));
            }

            #endregion
        }
    }
}
