using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace apriori
{
    class Apriori{
        private string filePath = "resutlfile.txt";
        
        private List<List<string>> data;
        private List<List<string>> firstElementItemSet;
        private IDictionary<KeyList<string>,int> freqset = new Dictionary<KeyList<string>,int>();
        private IDictionary<KeyList<KeyList<string>>, int> largset = new Dictionary<KeyList<KeyList<string>>, int>();
        private float minSup = 0.00001f;
        private float minConf = 0.00001f;
        public Apriori(List<List<string>> data){ 
            //first get the data 
            this.data = data;
            //initialize dictionary
            this.initializeFreqSet();
            //this.getAllSubsetsOfData();
            this.firstElementItemSet = generateFirstElementItemSet(data);
            var currentSet = returnItemsWithMinSupport(this.firstElementItemSet, this.data, minSup);
            int k = 2;
            while (currentSet.Count>0) {
                this.largset.Add(this.convertTo2DKeyList(currentSet), k - 1);
                currentSet = this.generateNthItemSet(currentSet, k);
                currentSet = this.returnItemsWithMinSupport(currentSet, this.data, minSup);
                k += 1;
            }
            
            this.calculateSupportOfAll();
            this.calculateConfidenceOfAll(this.minConf);
            Console.WriteLine("Done");
            
            
        }

        private void initializeFreqSet(){
            var allSubsets = this.getAllSubsetsOfData();
            foreach (var itemSetList in allSubsets){
                foreach (var itemSet in itemSetList){
                    KeyList<string> tmpKey = new KeyList<string>();
                    foreach (var item in itemSet) tmpKey.Add(item);
                    if (!freqset.ContainsKey(tmpKey)) freqset.Add(tmpKey,0);
                }
            }
        }

        private bool issubset<T>(List<T> item,List<T> transction) {
            List<List<T>> subsetsList = this.subSets(transction);
            foreach (var sub in subsetsList){
                if (Enumerable.SequenceEqual(item, sub))
                    return true;
            }
            return false;
        }

        private List<List<List<string>>> getAllSubsetsOfData(){
            var allsubsets = new List<List<List<string>>>();
            
            foreach (var itemList in this.data){
                allsubsets.Add(this.subSets(itemList));
            }


            return allsubsets;
        }


        private List<List<T>> subSets<T>(List<T> source)
        {
            List<List<T>> allSubsets = new List<List<T>>();
            List<T> list = source;
            int length = list.Count;
            int max = (int)Math.Pow(2, list.Count);

            for (int count = 0; count < max; count++)
            {
                List<T> subset = new List<T>();
                uint rs = 0;
                while (rs < length)
                {
                    if ((count & (1u << (int)rs)) > 0)
                    {
                        subset.Add(list[(int)rs]);
                    }
                    rs++;
                }
                
                        
                if (subset.Count() > 0)
                    allSubsets.Add(subset);
                
            }
            return allSubsets;
        }


        private List<List<string>> returnItemsWithMinSupport(List<List<string>> itemSet,List<List<string>> trans,float minSupport)
        {
            var _itemset = new List<List<string>>();
            var localset = new Dictionary<KeyList<string>,int>();

            foreach (var itemset in this.convertTo2DKeyList(itemSet)) {
                
                if(!localset.ContainsKey(itemset))
                    localset.Add(itemset, 0);
                
                if (!freqset.ContainsKey(itemset))
                    freqset.Add(itemset,0);

                foreach (var t in trans){
                    if (this.issubset(itemset.ToList(),t)) {
                        localset[itemset] += 1;
                        freqset[itemset] += 1;
                    }
                }

            }
            foreach (var item in localset) {
                float support = (float)item.Value / (float)trans.Count;
                if (support >= minSupport) {
                    _itemset.Add(item.Key.ToList());
                }
            }

            return _itemset;
        }

        private float getSupport(KeyList<string> item){
            return (float)this.freqset[item] / (float)this.data.Count();
        }

        private void calculateSupportOfAll(){
            foreach (var elem in this.largset) {
                foreach (var itemset in elem.Key){
                    foreach (var item in itemset){
                        Console.Write(item+",");
                        this.printResultToFile(item+",");
                    }
                    
                    Console.Write(" | Support = "+getSupport(itemset).ToString());
                    this.printResultToFile(" | Support = " + getSupport(itemset).ToString()+"\n");
                    Console.WriteLine();
                }
            }
        }

        private void calculateConfidenceOfAll(float minCONF){
            foreach (var elem in this.largset){
                foreach (var itemset in elem.Key){
                    var subSet = this.convertTo2DKeyList(this.subSets(itemset.ToList()));
                    foreach (var set in subSet){

                        var remainSet = new KeyList<string>();
                        foreach (var item in itemset.Except(set))remainSet.Add(item);

                        if (remainSet.Count() > 0){

                            var confidence = (float)getSupport(itemset) / (float)getSupport(set);
                            var lift = (float)getSupport(itemset) / ((float)getSupport(set) * (float)getSupport(remainSet));

                            if (confidence >= minCONF && !float.IsInfinity(confidence)){
                                foreach (var s in set){
                                    this.printResultToFile(s + ",");     
                                    Console.Write(s + ",");
                                }
                                this.printResultToFile(" => ");
                                Console.Write(" => ");
                                foreach (var s in remainSet){
                                    this.printResultToFile(s + ",");
                                    Console.Write(s + ",");
                                }
                                this.printResultToFile("confidence = " + confidence.ToString() +"| lift = "+lift.ToString() +"\n");
                                Console.Write("confidence = " + confidence.ToString() + "| lift = " + lift.ToString() + "\n");
                            }
                        }
                    }
                }
            }
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

        private KeyList<KeyList<string>> convertTo2DKeyList(List<List<string>> list) {
            KeyList<KeyList<string>> retList = new KeyList<KeyList<string>>();
            foreach (var ls in list){
                KeyList<string> tmpls = new KeyList<string>();
                foreach (var l in ls)
                    tmpls.Add(l);
                retList.Add(tmpls);
            }
            return retList;   
        }

        private KeyList<string> convertToKeyList(List<string> list){
            KeyList<string> retList = new KeyList<string>();
            foreach (var item in list)
                retList.Add(item);
            return retList;
        }

        private bool printResultToFile(string res){
            try{
                if (!File.Exists(filePath)){
                    using (StreamWriter sr = File.CreateText(filePath)){
                        sr.Write(res);
                    }
                }
                using (StreamWriter sr = File.AppendText(filePath)){
                    sr.Write(res);
                }
                return true;
            }
            catch (Exception error){
                return false;
                throw error;
            }
        }
    }
}
