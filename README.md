# HW_Apriori

This code is based on simple APRIORI algorithm, and is make using visual studio.
It connects to a SQL server database and gets data that it needs and passess it to the Apriori class.

After that the codes runs based on the amount of min support and min confidence.

For the sake of performance and clearer code a class with the name `KeyList` has been added becase we need to keep track of trnasctions frequency as a dictionary which has `KeyList<string>` as the key of the dictionary and an `int` as the value.
This `KeyList` class recomputes hashcode so that it does not need to compute it later.

This `KeyList` class is thanks to [honey the codewitch](https://www.codeproject.com/Tips/5260474/Efficiently-Using-Lists-as-Dictionary-Keys-in-Csha).

You can run this code by simply using the `.sln` file.

The backup file of the data base is also added for those who want to test this code completely.

