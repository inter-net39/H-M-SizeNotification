using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Media;
using System.Threading.Tasks;

namespace H_M_SizeN
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            var mainUrl = string.Empty;
            var liczby = string.Empty;
            long productId = 0;
            List<long> numery = new List<long>();

            var ur = "https://www2.hm.com/pl_pl/getAvailability?variants=";
            try
            {
                do
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Podaj adres URL do produktu.");
                    Console.Write(">> ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    mainUrl = Console.ReadLine();
                    var list = mainUrl.Split('.');
                    try
                    {
                        productId = long.Parse(list[list.Length - 2]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                } while (string.IsNullOrWhiteSpace(mainUrl) || mainUrl.Contains("https://") == false || mainUrl.Length < 12 || productId == 0);

                do
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Podaj numery oddzielone przecinkiem.");
                    Console.Write(">> ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    liczby = Console.ReadLine();
                    var list = liczby?.Split(',');
                    try
                    {
                        foreach (var s in list)
                        {
                            numery.Add(long.Parse(s));
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                } while (string.IsNullOrWhiteSpace(liczby) || numery.Count == 0);


#pragma warning disable 4014
                Request(ur, productId, numery);
#pragma warning restore 4014












            }
            catch (Exception ex)
            {
                Console.WriteLine("[Main(string[] args)] - " + ex.Message);
            }


            Console.ReadKey();
            Console.ReadKey();
        }

        public static async Task Request(string ur, long productId, List<long> numbersList)
        {

            do
            {
                try
                {


                    var client = new RestClient($"{ur}0{productId}");
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("Postman-Token", "33752acb-4d64-429d-bf96-2dc431e5ecfb");
                    request.AddHeader("cache-control", "no-cache");
                    IRestResponse response = client.Execute(request);
                    Console.WriteLine(response.ResponseStatus);
                    Console.WriteLine(response.Content.ToString());

                    Details det = JsonConvert.DeserializeObject<Details>(response.Content);

                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Dostępne rozmiary:");
                    foreach (var awa in det.availability)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;

                        Console.WriteLine(((awa % 100) + 31).ToString().PadLeft(6));
                        if (numbersList.Contains((awa % 100) + 31))
                        {
                            PlayNotyficationSound();
                            Console.WriteLine("Znaleziono twój rozmiar !!!");
                            Console.WriteLine("Znaleziono twój rozmiar !!!");
                            Console.WriteLine("Znaleziono twój rozmiar !!!");
                            return;
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Zostało tylko kilka sztuk:");
                    foreach (var awa in det.fewPieceLeft)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;

                        Console.WriteLine(((awa % 100) + 31).ToString().PadLeft(6));
                    }
                    Console.ForegroundColor = ConsoleColor.White;

                    await Task.Delay(10000);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            } while (true);

        }

        public static void PlayNotyficationSound()
        {
            try
            {
                SoundPlayer typewriter = new SoundPlayer();
                typewriter.SoundLocation = Environment.CurrentDirectory + "/Chicken_Impossible-rMLD1JmEoQ0.wav";
                typewriter.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[PlayNotyficationSound()] - " + ex.Message);

            }
        }
    }

    internal class Details
    {
        public List<long> availability { get; set; }
        public List<long> fewPieceLeft { get; set; }
    }
}
