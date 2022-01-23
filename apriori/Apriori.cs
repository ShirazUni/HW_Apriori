using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apriori
{
    class Apriori{
        private List<List<string>> data;
        private List<List<string>> firstElementItemSet;
        private Dictionary<List<string>,int> freqset = new Dictionary<List<string>,int>();
        private Dictionary<List<List<string>>, int> largset = new Dictionary<List<List<string>>, int>();
        private float minSup = 0.15f;
        private float minConf = 0.15f;
        public Apriori(List<List<string>> data){
            //first get the data 
            this.data = data;
            //initialize dictionary
            this.initializeFreqSet();
            this.firstElementItemSet = generateFirstElementItemSet(data);
            var currentSet = returnItemsWithMinSupport(this.firstElementItemSet, this.data, minSup);
            int k = 2;
            while (currentSet.Count>0) {
                this.largset.Add(currentSet, k - 1);
                currentSet = this.generateNthItemSet(currentSet, k);
                currentSet = this.returnItemsWithMinSupport(currentSet, this.data, minSup);
                k += 1;
            }
            foreach (var item in freqset) {
                Console.WriteLine(item.Value.ToString());
                foreach (var i in item.Key) {
                    Console.Write(i+",");
                }
                Console.WriteLine();
            }
            this.calculateSupportOfAll();
            //this.calculateConfidenceOfAll(this.minConf);
            Console.WriteLine("Done");
            
            
        }


        private bool issubset(List<string> item,List<string> transction) {
            List<List<string>> subsetsList = this.subSets(transction);
            foreach (var sub in subsetsList){
                if (Enumerable.SequenceEqual(item, sub))
                    return true;
            }
            return false;
        }

        private List<List<List<string>>> getAllSubsetsOfData(){
            var transactionList = new List<List<List<string>>>();
            
            foreach (var itemList in this.data){
                transactionList.Add(this.subSets(itemList));
            }


            /*foreach (var setList in transactionList.ToArray())
            {
                foreach (var set in setList.ToArray())
                {
                    Console.WriteLine("set "+set.Count().ToString());
                    foreach (var s in set.ToArray()) {
                        Console.Write(s + ",");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();

            }*/
            return transactionList;
        }


        private List<List<string>> subSets(List<string> source)
        {
            List<List<string>> allSubsets = new List<List<string>>();
            List<string> list = source;
            int length = list.Count;
            int max = (int)Math.Pow(2, list.Count);

            for (int count = 0; count < max; count++)
            {
                List<string> subset = new List<string>();
                uint rs = 0;
                while (rs < length)
                {
                    if ((count & (1u << (int)rs)) > 0)
                    {
                        subset.Add(list[(int)rs]);
                    }
                    rs++;
                }
                if(subset.Count>0)
                    allSubsets.Add(subset);
            }
            return allSubsets;
        }


        private List<List<string>> returnItemsWithMinSupport(List<List<string>> itemSet,List<List<string>> trans,float minSupport)
        {
            var _itemset = new List<List<string>>();
            var localset = new Dictionary<List<string>,int>();

            foreach (var itemset in itemSet) {
                var allCombinations = PermutaionsOfStringList.permute(itemset);
                localset.Add(itemset, 0);
                foreach (var t in trans){
                    if (this.issubset(itemset,t)) {
                        localset[itemset] += 1;
                        foreach (var elem in freqset.Keys.ToList()) {
                            if (elem.SequenceEqual(itemset)){
                                    freqset[elem] += 1;
                            }
                        }
                    }
                }

            }
            foreach (var item in localset) {
                float support = (float)item.Value / (float)trans.Count;
                if (support >= minSupport) {
                    _itemset.Add(item.Key);
                }
            }

            return _itemset;
        }

        private float getSupport(List<string> item){
            var val = 0f;
            foreach (var elem in freqset.Keys.ToList())
                if (elem.SequenceEqual(item))
                    val =(float)this.freqset[elem];
            return val/(float)this.data.Count;
            //var val = (float)this.freqset[item];
            //return val / (float)this.data.Count();
        }

        private void calculateSupportOfAll(){
            foreach (KeyValuePair<List<List<string>>,int> elem in this.largset) {
                foreach (var itemset in elem.Key){
                    foreach (var item in itemset){
                        Console.Write(item+",");
                    }
                    
                    Console.Write(" | Support = "+getSupport(itemset).ToString());
                    Console.WriteLine();
                }
            }
        }

        private void calculateConfidenceOfAll(float minCONF){
            foreach (var elem in this.largset){
                foreach (var itemset in elem.Key){
                    var subSet = this.subSets(itemset);
                    foreach (var set in subSet){
                        Console.WriteLine(set);
                        var remainSet = itemset.Except(set);
                        if (remainSet.Count()>0){
                            var confidence = (float)this.getSupport(itemset)/(float)this.getSupport(set.ToList());
                            if (confidence >= minCONF){
                                foreach (var s in set){
                                    Console.Write(s + ",");
                                }
                                Console.Write(" => ");
                                foreach (var s in remainSet){
                                    Console.Write(s+",");
                                }
                                Console.Write("confidence = "+confidence.ToString()+"\n");
                            }
                        }
                    }
                }
            }
        }

        private void calculateLiftOfAll(){

        }

        private List<List<string>> generateNthItemSet(List<List<string>> source,int length){
            List<List<string>> nthElementItemSet = new List<List<string>>();
            foreach (var itemset in source) {
                IEnumerable<string> tmp;
                foreach (var _itemset in source) {
                    tmp= itemset.Union(_itemset);
                    if (tmp.Count() == length)
                    {
                        nthElementItemSet.Add(tmp.ToList());
                    }
                    if (tmp.Count() > length) break;
                }
            }
            return nthElementItemSet;
        }

        private List<List<string>> generateFirstElementItemSet(List<List<string>> source) {
            List<List<string>> firstItemSet = new List<List<string>>();
            bool flag = true;
            foreach (var itemSet in source){
                foreach (var item in itemSet) {
                    List<string> ls = new List<string> { item};
                    foreach(var set in firstItemSet){
                        if (Enumerable.SequenceEqual(set, ls)) {
                            flag = false;
                            break;
                        }
                    }
                    if (flag) { firstItemSet.Add(ls); }
                }
            }
            return firstItemSet;
        }
        private void initializeFreqSet(){
            var allSubsets = this.getAllSubsetsOfData();
            foreach (var itemSetList in allSubsets)
                foreach (var itemSet in itemSetList){
                    var combItemSet = PermutaionsOfStringList.permute(itemSet);
                    foreach (var comb in combItemSet)
                        if (!freqset.ContainsKey(comb))
                            freqset.Add(comb,0);
                }      
        }
    }
}
