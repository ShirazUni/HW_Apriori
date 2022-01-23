using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apriori
{
    public static class PermutaionsOfStringList{
        

        public static List<List<string>> permute(List<string> itemset){
            var list = new List<List<string>>();
            return doPermute(itemset,0,itemset.Count-1,list);
        }

        private static List<List<string>> doPermute(List<string> itemset,int start,int end, List<List<string>> list){
            if (start == end)
                list.Add(itemset);
            else
            {
                for (var i = start; i <= end; i++){
                    itemset=swap(itemset,start,i);
                    doPermute(itemset, start + 1, end, list);
                    itemset=swap(itemset,start,i);
                }

            }

            return list;
        }

        private static List<T> swap<T>(this List<T> list,int idxA, int idxB){
            var temp = list[idxA];
            list[idxA] = list[idxB];
            list[idxB] = temp;
            return list;

        }

      
    }
}
