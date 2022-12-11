using System;
using System.Collections.Generic;
using System.Linq;

namespace AssetTrackingEntityFrameworkProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AssetTrackingEntities1 context = new AssetTrackingEntities1();

            #region sample data
            // deletes all previous data in the table "Assets"
            context.Database.ExecuteSqlCommand("TRUNCATE TABLE [Assets]");

            Assets testData1 = new Assets
            {
                Type = "Laptop",
                Brand = "Apple",
                Model = "Macbook",
                Office = "Spain",
                Purchase_Date = Convert.ToDateTime("2021-02-25"),
                Price_USD = 2999
            };
            context.Assets.Add(testData1);

            Assets testData2 = new Assets
            {
                Type = "Laptop",
                Brand = "Lenovo",
                Model = "Thinkpad L380",
                Office = "Sweden",
                Purchase_Date = Convert.ToDateTime("2020-11-05"),
                Price_USD = 999
            };
            context.Assets.Add(testData2);

            Assets testData3 = new Assets
            {
                Type = "Laptop",
                Brand = "Asus",
                Model = "Zephyrus",
                Office = "USA",
                Purchase_Date = Convert.ToDateTime("2019-08-14"),
                Price_USD = 1999
            };
            context.Assets.Add(testData3);

            Assets testData4 = new Assets
            {
                Type = "Phone",
                Brand = "Apple",
                Model = "iPhone X",
                Office = "Sweden",
                Purchase_Date = Convert.ToDateTime("2021-12-24"),
                Price_USD = 1099
            };
            context.Assets.Add(testData4);

            Assets testData5 = new Assets
            {
                Type = "Phone",
                Brand = "Nokia",
                Model = "Brickphone",
                Office = "Spain",
                Purchase_Date = Convert.ToDateTime("2010-12-31"),
                Price_USD = 99
            };
            context.Assets.Add(testData5);

            Assets testData6 = new Assets
            {
                Type = "Phone",
                Brand = "Samsung",
                Model = "Galaxy Flip Z",
                Office = "USA",
                Purchase_Date = Convert.ToDateTime("2022-05-12"),
                Price_USD = 1299
            };
            context.Assets.Add(testData6);

            context.SaveChanges();
            #endregion

            do
            {
                bool quitProgram = false;
                string input = "";
                do
                {
                    string type = "";
                    string brand = "";
                    string model = "";
                    string office = "";
                    int priceInUSD = 0;
                    DateTime purchaseDate = DateTime.Now;
                    Console.SetWindowSize(175, 40);

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(
                        "Type \"S\" to show the asset list \n" +
                        "Type \"Search\" to search for an specific assets ID\n" +
                        "Type \"U\" to update an asset \n" +
                        "Type \"D\" to delete an asset \n" +
                        "Type \"Q\" to quit the input screen \n" +
                        "Would you like to continue with adding new assets? Write the type of the asset(Only Laptops/Mobiles allowed) \n");

                    input = Console.ReadLine();

                    if (input.ToLower().Equals("s") || input.ToLower().Equals("search") || input.ToLower().Equals("u") || input.ToLower().Equals("d"))
                    {
                        Console.Clear();
                        break;
                    }
                    else if (input.ToLower().Equals("q"))
                    {
                        quitProgram = true;
                        Console.Clear();
                        break;
                    }
                    else
                    {
                        type = input;
                    }

                    do
                    {
                        Console.Write("Write the brand name: ");
                        brand = Console.ReadLine();
                        Console.WriteLine();
                    } while (!checkIfValidInput(brand));

                    do
                    {
                        Console.Write("Write the model name: ");
                        model = Console.ReadLine();
                        Console.WriteLine();
                    } while (!checkIfValidInput(model));

                    do
                    {
                        Console.Write("Write where the office is located(Sweden/Spain/USA): ");
                        office = Console.ReadLine();
                        Console.WriteLine();
                        if (office.ToLower().Equals("sweden") || office.ToLower().Equals("usa") || office.ToLower().Equals("spain")) { break; }
                    } while (true);

                    do
                    {
                        Console.Write("Write the cost of the asset in USD: ");
                        priceInUSD = Int32.Parse(Console.ReadLine());
                        Console.WriteLine();
                        if (priceInUSD > 0) { break; }
                    } while (true);


                    do
                    {
                        Console.Write("Write the purchase date in the following format yyyy-MM-dd: ");
                        string line = Console.ReadLine();

                        // Checks if user's input of purchase date is correct. If not, prompts user to retry again.
                        DateTime dt;
                        if (DateTime.TryParseExact(line, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dt))
                        {
                            purchaseDate = dt;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid date, please retry");
                        }
                    } while (true);

                    Assets asset1 = new Assets
                    {
                        Type = type,
                        Brand = brand,
                        Model = model,
                        Office = office,
                        Purchase_Date = Convert.ToDateTime(purchaseDate),
                        Price_USD = priceInUSD
                    };

                    context.Assets.Add(asset1);
                    context.SaveChanges();


                    Console.WriteLine("Asset successfully added!");
                    Console.WriteLine();
                } while (true);

                // Iterates through the database and adds each row to the list.
                int i = 1;
                List<Assets> assetsListFromDB = new List<Assets>();
                int totalNumberOfAssets = context.Assets.Count();
                int foundNumberOfAssets = 0;

                do
                {
                    // Attempts to find the asset by their id, starting from 1.
                    Assets tempAsset = context.Assets.FirstOrDefault(x => x.Id == i);
                    // If no asset could be found and the program have iterated equal times to the total number of assets,
                    // the logic will break.
                    if (tempAsset == null && foundNumberOfAssets == totalNumberOfAssets)
                    {
                        break;
                    }
                    else if (tempAsset == null)
                    {
                        i++;
                    }
                    else
                    {
                        assetsListFromDB.Add(tempAsset);
                        i++;
                        foundNumberOfAssets++;
                    }
                } while (true);

                if (input.ToLower().Equals("search"))
                {
                    searchForAsset();
                }
                else if (input.ToLower().Equals("u"))
                {
                    updateAsset();
                }
                else if (input.ToLower().Equals("d"))
                {
                    int id;
                    do
                    {
                        do
                        {
                            Console.WriteLine("Input the asset ID you would like to delete");
                            try
                            {
                                id = Int32.Parse(Console.ReadLine());
                                break;
                            }
                            catch (FormatException e) { Console.WriteLine(e.Message); Console.Write("Try again."); }
                            catch (Exception e) { Console.WriteLine(e.Message); Console.Write("Try again."); }

                        } while (true);

                        Console.Clear();
                        var foundAsset = context.Assets.FirstOrDefault(x => x.Id == id);

                        if (foundAsset == null)
                        {
                            Console.WriteLine($"ID {id} could not be found in the database. Try again.");
                        }
                        else
                        {
                            writeCategoriesInConsole();
                            outputAssetList(foundAsset);
                            consoleLineBreaks();

                            do
                            {
                                Console.WriteLine("Are you sure you wish to delete the asset shown above? Type \"Yes\" or \"No\"");
                                string choice = Console.ReadLine();

                                if (choice.ToLower().Equals("yes"))
                                {
                                    context.Assets.Remove(foundAsset);
                                    context.SaveChanges();
                                    Console.WriteLine($"Asset with the ID {id} has been deleted.");
                                    Console.WriteLine("");
                                    break;
                                }
                                else if (choice.ToLower().Equals("no"))
                                {
                                    Console.WriteLine("Cancelling the current operation. Asset will NOT be deleted.");
                                    Console.WriteLine("");
                                    break;
                                }
                            } while (true);
                            break;
                        }
                    } while (true);
                }
                else if (input.ToLower().Equals("s"))
                {
                    // Sorts list by Office and then by sorts it by Purchase date
                    List<Assets> sortedByType = assetsListFromDB.OrderBy(o => o.Office).ThenBy(o => o.Purchase_Date).ToList();
                    int totalCostOfAssets = 0;

                    writeCategoriesInConsole();

                    foreach (Assets assets in sortedByType)
                    {
                        outputAssetList(assets);
                        totalCostOfAssets += assets.Price_USD;
                    }
                    outputReportsAboutAssets(totalCostOfAssets);
                    consoleLineBreaks();
                }

                if (quitProgram)
                {
                    context.Dispose();
                    break;
                }
            } while (true);

            void writeCategoriesInConsole()
            {
                Console.WriteLine("ID".PadRight(5) + "Type".PadRight(20) + "Brand".PadRight(20) + "Model".PadRight(20) + "Office".PadRight(20) + "Purchase Date".PadRight(20) + "Price in USD".PadRight(20) + "Currency".PadRight(20) + "Local Price");
                Console.WriteLine("---".PadRight(5) + "---".PadRight(20) + "---".PadRight(20) + "---".PadRight(20) + "---".PadRight(20) + "------".PadRight(20) + "----".PadRight(20) + "----".PadRight(20) + "----");
            }

            bool checkIfValidInput(string input)
            {
                if (input == null) { return false; }
                else if (input.Length <= 2) { return false; }
                else { return true; }
            }

            double calculateLocalPrice(string input, int value)
            {
                if (input.ToLower().Equals("sweden"))
                {
                    return Math.Round(value * 10.37, 2);
                }
                else if (input.ToLower().Equals("spain"))
                {
                    return Math.Round(value * 0.96, 2);
                }
                else
                {
                    return value;
                }
            }

            string checkLocalCurrency(string input)
            {
                if (input.ToLower().Equals("sweden"))
                {
                    return "SEK";
                }
                else if (input.ToLower().Equals("spain"))
                {
                    return "EUR";
                }
                else
                {
                    return "USD";
                }
            }

            void outputAssetList(Assets asset)
            {
                // 3 years = 1095 days
                // items *RED* if purchase date is less than 3 months away from 3 years
                // Items *Yellow* if date less than 6 months away from 3 year
                // 1095 days - 90(3 months) = 1005 days
                // 1095 days - 180(6 months = 915 days

                DateTime today = DateTime.Today;
                DateTime assetPurchaseDate = asset.Purchase_Date;

                // Gets how many days between the purchase date of the asset and todays date
                TimeSpan isAssetExpiring = today - assetPurchaseDate;
                int totalDays = (int)isAssetExpiring.TotalDays;

                double localPrice = calculateLocalPrice(asset.Office, asset.Price_USD);
                string localCurrency = checkLocalCurrency(asset.Office);

                // If totaldays exceeds 1005 days, that means the asset is less than 3 months away from reaching 3 years or already has exceeded the 3 years mark.
                if (totalDays > 1005)
                {
                    Console.Write(asset.Id.ToString().PadRight(5) + asset.Type.PadRight(20) + asset.Brand.PadRight(20) + asset.Model.PadRight(20) + asset.Office.PadRight(20));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(asset.Purchase_Date.ToShortDateString().PadRight(20));
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(asset.Price_USD.ToString().PadRight(20) + localCurrency.PadRight(20) + localPrice);
                }
                // If totaldays exceeds 915 days but is less than 1005 days, that means the asset is between 6 months and 3 months from reaching 3 years old.
                else if (totalDays > 915 && totalDays < 1005)
                {
                    Console.Write(asset.Id.ToString().PadRight(5) + asset.Type.PadRight(20) + asset.Brand.PadRight(20) + asset.Model.PadRight(20) + asset.Office.PadRight(20));
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(asset.Purchase_Date.ToShortDateString().PadRight(20));
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(asset.Price_USD.ToString().PadRight(20) + localCurrency.PadRight(20) + localPrice);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(asset.Id.ToString().PadRight(5) + asset.Type.PadRight(20) + asset.Brand.PadRight(20) + asset.Model.PadRight(20) + asset.Office.PadRight(20) + asset.Purchase_Date.ToShortDateString().PadRight(20) + asset.Price_USD.ToString().PadRight(20) + localCurrency.PadRight(20) + localPrice);
                }
                Console.WriteLine("");
            }

            void outputReportsAboutAssets(int totalCostOfAssets)
            {
                int numberOfLaptops = context.Assets.Count(x => x.Type == "Laptop");
                int numberOfPhones = context.Assets.Count(x => x.Type == "Phone");
                int numberOfAssets = context.Assets.Count();
                var assetMaxPrice = context.Assets.Max(x => x.Price_USD);
                Assets assetMax = context.Assets.FirstOrDefault(x => x.Price_USD == assetMaxPrice);
                //Assets mostExpensiveAsset = (Assets)context.Assets.Where(x => x.Price_USD == context.Assets.Max(y => y.Price_USD));

                Console.WriteLine("----------------------------------");
                Console.WriteLine($"Number of laptops: {numberOfLaptops} || Number of phones: {numberOfPhones}");
                Console.WriteLine($"Number of assets: {numberOfAssets} || Total cost of all Assets: ${totalCostOfAssets}");
                Console.WriteLine($"Most expensive asset is the {assetMax.Type} {assetMax.Brand} {assetMax.Model} costing ${assetMax.Price_USD}");
                Console.WriteLine("----------------------------------");

            }

            void updateAsset()
            {
                string newType = "";
                string newBrand = "";
                string newModel = "";
                string newOffice = "";
                int newPriceInUSD = 0;
                DateTime newPurchaseDate = DateTime.Now;
                int id;

                do
                {
                    do
                    {
                        Console.WriteLine("Input the asset ID you would like to change.");
                        try
                        {
                            id = Int32.Parse(Console.ReadLine());
                            break;
                        }
                        catch (FormatException e) { Console.WriteLine(e.Message); Console.Write("Try again."); }
                        catch (Exception e) { Console.WriteLine(e.Message); Console.Write("Try again."); }

                    } while (true);

                    Console.Clear();
                    Assets foundAsset = context.Assets.FirstOrDefault(x => x.Id == id);

                    if (foundAsset == null)
                    {
                        Console.WriteLine($"ID {id} could not be found in the database. Try again.");
                    }
                    else
                    {
                        writeCategoriesInConsole();
                        outputAssetList(foundAsset);
                        consoleLineBreaks();

                        Console.WriteLine($"Type in a new type for the asset or click enter to keep current type \"{foundAsset.Type}\"");
                        do
                        {
                            newType = Console.ReadLine();
                            Console.WriteLine();
                            if (newType.Equals("")) { newType = foundAsset.Type; break; }
                        } while (!checkIfValidInput(newType));

                        Console.WriteLine($"Type in a new brand for the asset or click enter to keep current brand \"{foundAsset.Brand}\"");
                        do
                        {
                            newBrand = Console.ReadLine();
                            Console.WriteLine();
                            if (newBrand.Equals("")) { newBrand = foundAsset.Brand; break; }
                        } while (!checkIfValidInput(newBrand));


                        Console.WriteLine($"Type in a new Model for the asset or click enter to keep current Model \"{foundAsset.Model}\"");
                        do
                        {
                            newModel = Console.ReadLine();
                            Console.WriteLine();
                            if (newBrand.Equals("")) { newModel = foundAsset.Model; break; }
                        } while (!checkIfValidInput(newModel));

                        Console.WriteLine($"Type in a new Office for the asset or click enter to keep current Office \"{foundAsset.Office}\"");
                        do
                        {
                            newOffice = Console.ReadLine();
                            Console.WriteLine();
                            if (newOffice.Equals("")) { newOffice = foundAsset.Office; break; }
                            else if (newOffice.ToLower().Equals("Sweden") || newOffice.ToLower().Equals("Spain") || newOffice.ToLower().Equals("USA")) { break; }
                        } while (true);

                        Console.WriteLine($"Type in a new price (USD) for the asset or click enter to keep current price of \"{foundAsset.Price_USD}\"");
                        do
                        {
                            try
                            {
                                newPriceInUSD = Int32.Parse(Console.ReadLine());
                                Console.WriteLine();
                                if (newPriceInUSD > 0) { break; }
                            }
                            catch (FormatException e) { newPriceInUSD = foundAsset.Price_USD; break; }
                        } while (true);

                        Console.WriteLine($"Type in a new purchase date for the asset or click enter to keep current purchase date \"{foundAsset.Purchase_Date}\"");
                        do
                        {
                            string line = Console.ReadLine();

                            // Checks if user's input of purchase date is correct. If not, prompts user to retry again.
                            DateTime dt;
                            if (line.Equals("")) { newPurchaseDate = foundAsset.Purchase_Date; }
                            else if (DateTime.TryParseExact(line, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dt))
                            {
                                newPurchaseDate = dt;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Invalid date, please retry");
                            }
                        } while (true);

                        foundAsset.Type = newType;
                        foundAsset.Brand = newBrand;
                        foundAsset.Model = newModel;
                        foundAsset.Office = newOffice;
                        foundAsset.Price_USD = newPriceInUSD;
                        foundAsset.Purchase_Date = newPurchaseDate;

                        context.SaveChanges();

                        consoleLineBreaks();
                        break;
                    }
                } while (true);
            }

            void searchForAsset()
            {
                int id;
                do
                {
                    do
                    {
                        Console.WriteLine("Input the asset ID you would like to get more information about.");
                        try
                        {
                            id = Int32.Parse(Console.ReadLine());
                            break;
                        }
                        catch (FormatException e) { Console.WriteLine(e.Message); Console.Write("Try again."); }
                        catch (Exception e) { Console.WriteLine(e.Message); Console.Write("Try again."); }

                    } while (true);

                    Console.Clear();
                    Assets foundAsset = context.Assets.FirstOrDefault(x => x.Id == id);

                    if (foundAsset == null)
                    {
                        Console.WriteLine($"ID {id} could not be found in the database. Try again.");
                    }
                    else
                    {
                        writeCategoriesInConsole();
                        outputAssetList(foundAsset);
                        consoleLineBreaks();
                        break;
                    }
                } while (true);
            }
        }

        private static void consoleLineBreaks()
        {
            // Merely to make console viewing easier.
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("");
        }
    }
}
