using System;
using System.IO;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Timers;
namespace DataMiningNetCore
{
    class Program
    {

        static void Main(string[] args)
        {
            //ResortData(@"C:\MediaSlot\CloudDocs\Docs\课程\Data Mining\DataSets\BlackFriday\BlackFriday.csv", @"C:\MediaSlot\CloudDocs\Docs\课程\Data Mining\DataSets\BlackFriday\BlackFriday-Resorted.csv");
            //InsertToDataBase(@"C:\MediaSlot\CloudDocs\Docs\课程\Data Mining\DataSets\BlackFriday\BlackFriday-Resorted.csv");
            //List<List<int>> c2 = new List<List<int>>();

            //c2.Add(new List<int>() { 2, 4 });
            //c2.Add(new List<int>() { 1, 3 });
            //c2.Add(new List<int>() { 3, 2 });
            //c2.Add(new List<int>() { 4, 1 });
            //BubbleSort(c2);

            //GenerateCateDataSet(@"C:\MediaSlot\CloudDocs\Docs\课程\Data Mining\DataSets\BlackFriday\BlackFriday-Resorted.csv", @"C:\MediaSlot\CloudDocs\Docs\课程\Data Mining\DataSets\BlackFriday\BlackFriday-Resorted-CateSetGenerated.csv");

            //List<int> c1 = new List<int> { 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 1, 1, 1, 1, 1, 4, 4, 2, 2, 6, 6, 3, 3, 10, 10, 11, 11, 7, 13, 14, 16, 18 };

            //List<int> c2 = new List<int> { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
            //Console.WriteLine(CalJacSimilarity(c1, c2, JaccardType.DuplicationValid));
            //Console.ReadKey();


            //StreamReader streamReader1 = new StreamReader(@"C:\MediaSlot\CloudDocs\Docs\课程\Data Mining\DataSets\BlackFriday\BlackFriday-Resorted-CateSetGenerated.csv");


            //List<string> cluster = new List<string>();
            //while (!streamReader1.EndOfStream)
            //{
            //    cluster.Add(streamReader1.ReadLine());
            //}
            //streamReader1.Close();

            //int times = 3;
            //string[] records = new string[times];
            //StreamWriter streamWriter = new StreamWriter(@"C:\MediaSlot\CloudDocs\Docs\课程\Data Mining\Records\Iteration" + times + @"\clusterItemCount.txt");
            //for (int j = 0; j < 20; j++)
            //{
            //    List<List<string>> clusteredData = KMeans(cluster, times);

            //    int[] nums = new int[times];
            //    for (int i = 0; i < times; i++)
            //    {
            //        nums[i] = clusteredData[i].Count;
            //    }

            //    BubbleSort(nums);
            //    for (int i = 0; i < times; i++)
            //    {
            //        records[i] += nums[i] + " ";
            //    }

            //}
            //for (int i = 0; i < times; i++)
            //{
            //    streamWriter.WriteLine(records[i]);
            //}
            //streamWriter.Close();



            //Random rr = new Random();
            //do
            //{
            //    Console.WriteLine(rr.Next() % 1);
            //    Console.ReadKey();

            //} while (true);


            //Console.WriteLine(CalJacSimilarity(c1, c2, JaccardType.DuplicationValid).ToString());

            StreamReader streamReader1 = new StreamReader(@"C:\MediaSlot\CloudDocs\Docs\课程\Data Mining\DataSets\BlackFriday\BlackFriday-Resorted-CateSetGenerated.csv");


            List<string> cluster = new List<string>();
            while (!streamReader1.EndOfStream)
            {
                cluster.Add(streamReader1.ReadLine());
            }
            streamReader1.Close();

            int times = 6;



            List<List<string>> clusteredData = KMeans(cluster, times, JaccardType.DuplicationInvalid);




            CombineDataWithClusterInfo(@"C:\MediaSlot\CloudDocs\Docs\课程\Data Mining\DataSets\BlackFriday\BlackFriday-Resorted.csv", @"C:\MediaSlot\CloudDocs\Docs\课程\Data Mining\DataSets\BlackFriday\BlackFriday-Unregulated.csv", clusteredData);
            DataRegulation(@"C:\MediaSlot\CloudDocs\Docs\课程\Data Mining\DataSets\BlackFriday\BlackFriday-Unregulated.csv", @"C:\MediaSlot\CloudDocs\Docs\课程\Data Mining\DataSets\BlackFriday\TrainingData\T06BlackFriday-Regulated.csv");

            //Console.WriteLine("All Done!");
            //Console.ReadLine();
        }

        #region File Operations

        /// <summary>
        /// Regulate the data value to the range (0,20) (some of them)
        /// </summary>
        /// <param name="fileName"></param>
        private static void DataRegulation(string fileName, string objectiveFileName)
        {
            StreamReader streamReader = new StreamReader(fileName);
            StreamWriter streamWriter = new StreamWriter(objectiveFileName);
            streamWriter.WriteLine(streamReader.ReadLine());
            List<int> productCount = new List<int>();
            List<int> purchase= new List<int>();
            List<string[]> content = new List<string[]>();

            while (!streamReader.EndOfStream)
            {
                string[] data = streamReader.ReadLine().Split(",");
                content.Add(data);
                productCount.Add(Convert.ToInt32(data[6]));
                purchase.Add(Convert.ToInt32(data[7]));      
            }
            streamReader.Close();

            productCount.Sort();
            purchase.Sort();

            int max1 = productCount[productCount.Count - 1];
            int min1 = productCount[0];
            int max2 = purchase[purchase.Count - 1];
            int min2 = purchase[0];

            foreach (var item in content)
            {
                item[6] = ((Convert.ToDouble((item[6])) - (double)min1) / ((double)max1 - (double)min1) * 20.0).ToString();
                item[7] = ((Convert.ToDouble((item[7])) - (double)min2) / ((double)max2 - (double)min2) * 20.0).ToString();
                streamWriter.WriteLine(item[0]+","+ item[1] + "," + item[2] + "," + item[3] + "," + item[4] + "," + item[5] + "," + item[6] + "," + item[7] + "," + item[8]);
            }
            streamWriter.Close();
        

        }
        /// <summary>
        /// Resort the data by User_ID ASC
        /// </summary>
        /// <param name="fileName"></param>
        private static void ResortData(string fileName, string objectiveFileName)
        {
            StreamReader streamReader = new StreamReader(fileName);
            string title = streamReader.ReadLine();
            List<List<string>> userList = new List<List<string>>();
            List<int> userID = new List<int>();


            while (!streamReader.EndOfStream)
            {
                string lineText = streamReader.ReadLine();
                string[] data = lineText.Split(",");
                int userIDNum = Convert.ToInt32(data[0]) - 1000000;

                if (userID.Contains(userIDNum))
                {
                    userList[userID.IndexOf(userIDNum)].Add(lineText);
                }
                else
                {
                    userID.Add(userIDNum);
                    userList.Add(new List<string>());
                    userList[userID.IndexOf(userIDNum)].Add(lineText);
                    Console.WriteLine("User: " + userIDNum + " added!");
                }
            }
            streamReader.Close();

            StreamWriter streamWriter = new StreamWriter(objectiveFileName);
            streamWriter.WriteLine(title);
            foreach (List<string> user in userList)
            {
                foreach (string item in user)
                {
                    streamWriter.WriteLine(item);
                }
                Console.WriteLine("User: " + (Convert.ToInt32(user[0].Split(",")[0]) - 1000000).ToString() + " inserted!");
            }
            streamWriter.Close();

        }

        /// <summary>
        /// Generate the csv file of User_ID and it's product category for Jaccard-K-Means++ Clustering
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="objectiveFileName"></param>
        private static void GenerateCateDataSet(string fileName, string objectiveFileName)
        {
            StreamReader streamReader = new StreamReader(fileName);
            StreamWriter streamWriter = new StreamWriter(objectiveFileName);
            streamWriter.WriteLine(streamReader.ReadLine());

            string cateStr = "";
            string lastUser = "";

            while (!streamReader.EndOfStream)
            {
                string lineText = streamReader.ReadLine();
                string[] data = lineText.Split(",");

                if (lastUser != "" && lastUser != data[0])
                {
                    Console.WriteLine("Current User ID is: " + data[0]);

                    streamWriter.WriteLine(lastUser + cateStr);


                    cateStr = "";


                }

                cateStr += "," + data[8];

                lastUser = data[0];
            }
            streamReader.Close();
            streamWriter.Close();
        }

        /// <summary>
        /// Insert csv data to mysql database
        /// </summary>
        /// <param name="fileName"></param>
        private static void InsertToDataBase(string fileName)
        {
            MySqlConnection myCon = new MySqlConnection("Database=mydata;Data Source=127.0.0.1;User Id=root;Password=1234;pooling=false;CharSet=utf8;port=3306");

            try
            {
                myCon.Open();
                Console.WriteLine("Connected to database!");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return;

            }


            int count = 0;
            StreamReader streamReader = new StreamReader(fileName);
            streamReader.ReadLine();
            while (!streamReader.EndOfStream)
            {
                string[] preData = streamReader.ReadLine().Split(',');
                List<string> data = new List<string>();
                foreach (string item in preData)
                {
                    if (item.Replace(" ", "") != "" && item != null)
                    {
                        data.Add(item);
                    }
                    else
                    {
                        data.Add("0");
                    }
                }
                string sqlStr = "INSERT INTO `blackfriday` (`User_ID`, `Product_ID`, `Gender`, `Age`, `Occupation`, `City_Category`, `Stay_In_Current_City_Years`, `Marital_Status`, `Product_Category_1`, `Product_Category_2`, `Product_Category_3`, `Purchase`) " +
                                                "VALUES ('" + (Convert.ToInt32(data[0]) - 1000000) + "', '" + data[1] + "', '" + data[2] + "', '" + data[3] + "', '" + data[4] + "', '" + data[5] + "', '" + data[6] + "', '" + data[7] + "', '" + data[8] + "', '" + data[9] + "', '" + data[10] + "', '" + data[11] + "')";
                MySqlCommand insertCommand = new MySqlCommand(sqlStr, myCon);
                insertCommand.ExecuteNonQuery();
                count++;
                if (count % 100 == 0)
                {
                    Console.WriteLine("Current Item: " + count.ToString());
                }



            }
            streamReader.Close();


            Console.WriteLine("Done!");
            Console.ReadKey();
        }

 


        /// <summary>
        /// Calculate the total expenditure of a customer and update the dataset(input must be a sorted file!)
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static void CombineDataWithClusterInfo(string fileName, string objectiveFileName, List<List<string>> clusterData)
        {
            StreamReader streamReader = new StreamReader(fileName);
            StreamWriter streamWriter = new StreamWriter(objectiveFileName);
            streamReader.ReadLine();
            streamWriter.WriteLine("Gender,Occupation,City_Category,Stay_In_Current_City_Years,Marital_Status,Behevior_Category,Product_Count,Purchase,Age");
         
            int productCount = 0;
            int totalExpenditure = 0;
            string lastUser = "";
            int gender = -1;
            int age = 0;
            int cityCate = 0;
            int stayYears = -1;
            int maritalState = -1;

            List<List<string>> cateUserMap = new List<List<string>>();
            for (int i = 0; i < clusterData.Count; i++)
            {
                List<string> t1 = new List<string>();
                cateUserMap.Add(t1);
            }
            
            for (int i = 0; i < clusterData.Count; i++)
            {
                
                foreach (var item in clusterData[i])
                {
                    cateUserMap[i].Add(item.Split(",")[0]);
                }
                
            }
         

            while (!streamReader.EndOfStream)
            {
                string lineText = streamReader.ReadLine();
                string[] data = lineText.Split(",");
                
                if (lastUser != "" && lastUser.Split(",")[0] != data[0])
                {
                    string[] data1 = lastUser.Split(",");
                    Console.WriteLine("Current User ID is: " + data[0]);
                    int clusterNum = -1;
                    for (int i = 0; i < clusterData.Count; i++)
                    {
                        if (cateUserMap[i].Contains(data1[0]))
                        {
                            clusterNum = i;
                            break;
                        }
                        
                    }
                    if (data1[2].Replace(" ", "") == "M")
                    {
                        gender = 5;

                    }
                    else if (data1[2].Replace(" ", "") == "F")
                    {
                        gender = 15;
                    }
                    switch (data1[3].Replace(" ", ""))
                    {
                        case "0-17": age = 1; break;
                        case "18-25": age = 2; break;
                        case "26-35": age = 3; break;
                        case "36-45": age = 4; break;
                        case "46-50": age = 5; break;
                        case "51-55": age = 6; break;
                        case "55+": age = 7; break;
                        default:
                            break;
                    }
                    switch (data1[5].Replace(" ", ""))
                    {
                        case "A": cityCate = 5; break;
                        case "B": cityCate = 10; break;
                        case "C": cityCate = 15; break;
                   
                        default:
                            break;
                    }

                    switch (data1[6].Replace(" ", ""))
                    {
                        case "0": stayYears = 3; break;
                        case "1": stayYears = 6; break;
                        case "2": stayYears = 9; break;
                        case "3": stayYears = 12; break;
                        case "4+": stayYears = 15; break;

                        default:
                            break;
                    }

                    if (data1[4].Replace(" ", "") == "0")
                    {
                        maritalState = 5;

                    }
                    else if (data1[4].Replace(" ", "") == "1")
                    {
                        maritalState = 15;
                    }
                    streamWriter.WriteLine(gender + "," + data1[4] + "," + cityCate + "," + stayYears + "," + maritalState+ "," + (clusterNum + 1) * 2 + "," + productCount + "," + totalExpenditure + "," + age);
                 
                  
                    totalExpenditure = 0;
                    productCount = 0;

                }
                productCount++;
                totalExpenditure += Convert.ToInt32(data[11]);
                
                lastUser = lineText;
                
            }
            streamReader.Close();
            streamWriter.Close();
        }
        #endregion



        #region Bolck of K-Means++ algorithm
        /// <summary>
        /// K-Means Use Modified Jaccard similarity instead of Euclidean distance 
        /// The init cluster centers are Generated by Jaccard Similarity combined with some probability
        /// Detailed information are in my course project documentation
        /// </summary>
        /// <param name="dataSet">The unclustered dataset, each line with form like "id,item1,item2,....."</param>
        /// <param name="classAmount">The amount of clusters that you want it to be</param>
        /// <returns></returns>
        private static List<List<string>> KMeans(List<string> dataSet, int classAmount,JaccardType jaccardType)
        {
            StreamWriter streamWriter = new StreamWriter(@"C:\MediaSlot\CloudDocs\Docs\课程\Data Mining\Records\Iteration" + classAmount + @"\" + DateTime.Now.ToString("yyMMddHHmmss") + ".txt");
            string[] record = new string[classAmount];
            Console.WriteLine("K-Means Start");
            //create some room to store the clustered data
            List<List<string>> clusteredDataSet = new List<List<string>>();
            for (int i = 0; i < classAmount; i++)
            {
                List<string> cluster = new List<string>();
                clusteredDataSet.Add(cluster);
            }

            //a sign of whether "K-means" convergence
            bool isConvergence = false;
            Console.WriteLine("Calculate Init Centers");

            //calculate the init cluster center
            List<string> centers = SetInitCenters(dataSet, classAmount, jaccardType);
            
            //to store the map of a certain item's jaccard similarity with each cluster
            List<List<double>> JacSimMap = new List<List<double>>();
            for (int i = 0; i < classAmount; i++)
            {
                List<double> tm1 = new List<double>() { i, 0.0 };
                JacSimMap.Add(tm1);
            }


            int iterCount = 0;

            //do the k-means iteration
            while (!isConvergence)
            {
                Console.WriteLine("Iter "+ iterCount +" Start");

                //create room to store the data of last iteration, it will be used to the isconvergence judgement
                List<List<string>> lastClusterdDataSet = new List<List<string>>();
                foreach (List<string> item in clusteredDataSet)
                {
                    List<string> t1 = new List<string>();
                    foreach (string str in item)
                    {
                        t1.Add(str);
                    }
                    lastClusterdDataSet.Add(t1);
                }
                
               
                //as for 2nd and the following iteration, recalculate the cluster braycenter at the beginning
                if (iterCount > 0)
                {
                    for (int i = 0; i < classAmount; i++)
                    {
                        centers[i] = ReCalBrayCenter(clusteredDataSet[i]);
                        
                    }
                    Console.WriteLine("Bray Center Recalculated");
                }

                for (int i = 0; i < classAmount; i++)
                {
                    clusteredDataSet[i].Clear();
                }
                List<List<int>> centersSet = new List<List<int>>();


                foreach (var center in centers)
                {

                    List<int> centerSet = new List<int>();
                    string[] centerss = center.Split(",");
                    for (int j = 1; j < centerss.Length; j++)
                    {
                        centerSet.Add(Convert.ToInt32(centerss[j]));
                    }
                    centersSet.Add(centerSet);
                }

                Console.WriteLine("Calculate Jaccard Similarity");
                //calculate the jaccard similarity between every item and each cluster center,
                //then allocate a certain item to cluster which has the maximun jaccard similarity with it
                foreach (var item in dataSet)
                {
                    
                    List<int> itemSet = new List<int>();
                    string[] items = item.Split(",");
                    for (int j = 1; j < items.Length; j++)
                    {
                        itemSet.Add(Convert.ToInt32(items[j]));
                    }

                    for (int j = 0; j < centersSet.Count; j++)
                    {
                        JacSimMap[j][0] = (double)j;
                        JacSimMap[j][1] = CalJacSimilarity(itemSet, centersSet[j], jaccardType);
                    }
                    BubbleSort(JacSimMap);
                    clusteredDataSet[(int)JacSimMap[0][0]].Add(item);
                }
                
              
                for (int i = 0; i < classAmount; i++)
                {
                    record[i] += clusteredDataSet[i].Count + " ";
                    Console.WriteLine("Cluster " + i + " = " + clusteredDataSet[i].Count + ", " + lastClusterdDataSet[i].Count);
                }
                
                //isconvergence judgement
                for (int i = 0; i < classAmount; i++)
                {
                    isConvergence = true;
                    if (lastClusterdDataSet[i].Count != clusteredDataSet[i].Count)
                    {
                        Console.WriteLine("Amount not equal");
                        isConvergence = false;
                        break;
                    }

                    foreach (var item in lastClusterdDataSet[i])
                    {
                        if (!clusteredDataSet[i].Contains(item))
                        {
                            Console.WriteLine("Item not equal");
                            isConvergence = false;
                            break;
                        }
                    }

                }
                Console.WriteLine(" ");
                iterCount++;
                
            }
            for (int i = 0; i < classAmount; i++)
            {
                streamWriter.WriteLine(record[i]);
            }
            streamWriter.Close();

            return clusteredDataSet;
        }


        /// <summary>
        /// Jacarrd similarity Type
        /// </summary>
        private enum JaccardType
        {
            /// <summary>
            /// Allow repeated values like {1,2,2,3,4,5,5}
            /// </summary>
            DuplicationValid,

            /// <summary>
            /// Reduce the repeated values kile {1,2,2,3,4,5,5} -> {1,2,3,4,5}
            /// </summary>
            DuplicationInvalid

        }

        /// <summary>
        /// Calculate the jaccard similarity
        /// </summary>
        /// <param name="categorySet1">Set 1</param>
        /// <param name="categorySet2">Set 2</param>
        /// <param name="jaccardType">Jacarrd similarity Type</param>
        /// <returns></returns>
        private static double CalJacSimilarity(List<int> categorySet1, List<int> categorySet2, JaccardType jaccardType)
        {
            int orSet = 0;
            int andSet = 0;
            List<int> catereduced1 = new List<int>();
            List<int> catereduced2 = new List<int>();

            //if repeated values not allowed, reduce each set
            if (jaccardType == JaccardType.DuplicationInvalid)
            {

                foreach (int category in categorySet1)
                {
                    if (!catereduced1.Contains(category))
                    {
                        catereduced1.Add(category);
                    }
                }
                foreach (int category in categorySet2)
                {
                    if (!catereduced2.Contains(category))
                    {
                        catereduced2.Add(category);
                    }
                }
            }

            //if repeated values allowed, do nothing to the raw data, just make a copy of it
            if (jaccardType == JaccardType.DuplicationValid)
            {
                foreach (var item in categorySet1)
                {
                    catereduced1.Add(item);
                }

                foreach (var item in categorySet2)
                {
                    catereduced2.Add(item);
                }
              
            }

            //body
            foreach (int category in catereduced1)
            {
                for (int i = 0; i < catereduced2.Count; i++)
                {
                    if (category == catereduced2[i])
                    {
                        andSet++;
                        catereduced2.Remove(category);
                        break;
                    }
                }
                orSet++;
            }
            orSet += catereduced2.Count;
            return (double)andSet / (double)orSet;
        }

        /// <summary>
        /// Generate init cluster cneters
        /// </summary>
        /// <param name="dataSet">The unclustered dataset, each line with form like "id,item1,item2,....."</param>
        /// <param name="classAmount">The amount of clusters that you want it to be</param>
        /// <returns></returns>
        private static List<string> SetInitCenters(List<string> dataSet, int classAmount,JaccardType jaccardType)
        {
            List<string> centers = new List<string>();
            List<string> centersCopy = new List<string>();
            List<List<double>> jacSimValueSet = new List<List<double>>();
            Random rdm = new Random();
            centers.Add(dataSet[rdm.Next() % dataSet.Count]);
            centersCopy.Add(centers[0]);

        
            //don't want to write the documentation any more................
            while (centers.Count < classAmount || centers.Count == 1)
            {
                string brayCenter = ReCalBrayCenter(centersCopy);
              
                List<int> brayCenterSet = new List<int>();
                string[] brayCenters = brayCenter.Split(",");
                for (int j = 1; j < brayCenters.Length; j++)
                {
                    brayCenterSet.Add(Convert.ToInt32(brayCenters[j]));
                }
                string minJacSimItem = "";
                double minJacSimValue = 1.0;
                foreach (string item in dataSet)
                {
                    List<int> itemSet = new List<int>();
                    string[] items = item.Split(",");
                    for (int j = 1; j < items.Length; j++)
                    {
                        itemSet.Add(Convert.ToInt32(items[j]));
                    }
                    double currentJacSim = CalJacSimilarity(brayCenterSet, itemSet, jaccardType);

                    if (currentJacSim < minJacSimValue)
                    {
                        minJacSimValue = currentJacSim;
                        minJacSimItem = item;
                    }
                }
                
                centersCopy.Add(minJacSimItem);
                if (!centers.Contains(minJacSimItem))
                {
                    centers.Add(minJacSimItem);
                }
         
            }
            return centers;
        }

        /// <summary>
        /// Recalculate the braycenter
        /// </summary>
        /// <param name="cluster"></param>
        /// <returns></returns>
        private static string ReCalBrayCenter(List<string> cluster)
        {
            string brayCenter = "BrayCenter";
            List<int> orSet = new List<int>();
            List<List<int>> itemCount = new List<List<int>>();
            for (int i = 0; i < 18; i++)
            {
                List<int> cateCount = new List<int>();
                cateCount.Add(i + 1);
                cateCount.Add(0);
                itemCount.Add(cateCount);
            }
            List<int> braySet = new List<int>();
            foreach (string item in cluster)
            {
                string[] data = item.Split(",");
                for (int i = 1; i < data.Length; i++)
                {
                    orSet.Add(Convert.ToInt32(data[i]));
                    itemCount[Convert.ToInt32(data[i])-1][1]++;
                }
            }
            BubbleSort(itemCount);
            int brayItemCount = orSet.Count / cluster.Count;
            for (int i = 0; i < itemCount.Count; i++)
            {
                
                if (braySet.Count <= brayItemCount)
                {
                    
                    for (int j = 0; j < Math.Round((double)itemCount[i][1] / (double)orSet.Count * (double)brayItemCount); j++)
                    {
                        braySet.Add(itemCount[i][0]);
                    }
                }
           
            }
            for (int i = 0; i < braySet.Count; i++)
            {
                brayCenter += "," + braySet[i];
            }
            return brayCenter;
        }
        #endregion

        #region Globle Functions
        private static void BubbleSort(List<List<int>> unSorted)
        {
            for (int i = 0; i < unSorted.Count; i++)
            {
                for (int j = i; j < unSorted.Count; j++)
                {
                    if (unSorted[i][1] < unSorted[j][1])
                    {
                        List<int> temp = unSorted[i];
                        unSorted[i] = unSorted[j];
                        unSorted[j] = temp;
                    }
                }
            }
        }

        private static void BubbleSort(List<List<double>> unSorted)
        {
            for (int i = 0; i < unSorted.Count; i++)
            {
                for (int j = i; j < unSorted.Count; j++)
                {
                    if (unSorted[i][1] < unSorted[j][1])
                    {
                        List<double> temp = unSorted[i];
                        unSorted[i] = unSorted[j];
                        unSorted[j] = temp;
                    }
                }
            }
        }
        private static void BubbleSort(int[] unSorted)
        {
            for (int i = 0; i < unSorted.Length; i++)
            {
                for (int j = i; j < unSorted.Length; j++)
                {
                    if (unSorted[i] < unSorted[j])
                    {
                        int temp = unSorted[i];
                        unSorted[i] = unSorted[j];
                        unSorted[j] = temp;
                    }
                }
            }
        }
        #endregion



    }
}
