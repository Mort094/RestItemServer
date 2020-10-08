using ModelLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace ConsumeRest
{
    internal class RestWorker
    {
        private const String URI = "http://localhost:50575/api/Items/";

        public async void Start()
        {


            PrintHeader("Henter alle Items");
            List<Item> allItems = await GetAllItemsAsync();

            foreach (Item item in allItems)
            {
                Console.WriteLine(item);
            }



            PrintHeader("Henter item nr. 2");
            try
            {
                Item item2 = await GetOneItemAsync(2);
                Console.WriteLine("Item = " + item2);
            }
            catch (KeyNotFoundException knfe)
            {
                Console.WriteLine(knfe.Message);
            }
            PrintHeader("Opretter Item");
            Item nyItem = new Item(55, "tester", "good", 5.5);
            await OpretNytItem(nyItem);

            PrintHeader("Henter alle Items");
            allItems = await GetAllItemsAsync();

            foreach (Item item in allItems)
            {
                Console.WriteLine(item);
            }


            PrintHeader("Ændre item nr. 2");
            nyItem.Name = "test";
            await UpDateItem(nyItem);



            PrintHeader("Henter alle Items");
            allItems = await GetAllItemsAsync();

            foreach (Item item in allItems)
            {
                Console.WriteLine(item);
            }

            PrintHeader("Sletter item nr. 55");
            await SletItem(55);

            PrintHeader("Henter alle Items");
            allItems = await GetAllItemsAsync();

            foreach (Item item in allItems)
            {
                Console.WriteLine(item);
            }
        }

      

        private void PrintHeader(string txt)
        {
            Console.WriteLine("=========================");
            Console.WriteLine(txt);
            Console.WriteLine("=========================");

        }

        public async Task<List<Item>> GetAllItemsAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                String json = await client.GetStringAsync(URI);
                List<Item> items = JsonConvert.DeserializeObject<List<Item>>(json);
                return items;
            }
        }

        private async Task<Item> GetOneItemAsync(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(URI + id);

                String content = await resp.Content.ReadAsStringAsync();
                if (resp.IsSuccessStatusCode)
                {
                    Item cItem = JsonConvert.DeserializeObject<Item>(content);
                    return cItem;
                }
                throw new KeyNotFoundException($"Status code = {resp.StatusCode} Message = {content}");

            }
        }
        private async Task OpretNytItem(Item item)
        {
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(
                    JsonConvert.SerializeObject(item),
                    Encoding.UTF8,
                    "application/json");

                HttpResponseMessage resp = await client.PostAsync(URI, content);
                if (resp.IsSuccessStatusCode)
                {
                    return;
                }

                throw new ArgumentException("Opdater fejlede");
            }
        }

        private async Task UpDateItem(Item item)
        {
            using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(
                    JsonConvert.SerializeObject(item),
                    Encoding.UTF8,
                    "application/json");

                HttpResponseMessage resp = await client.PutAsync(URI + item.Id, content);
                if (resp.IsSuccessStatusCode)
                {
                    return;
                }
                throw new ArgumentException("Opdater fejlede");
            }
        }
        private async Task SletItem(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.DeleteAsync(URI + id);
                if (resp.IsSuccessStatusCode)
                {
                    return;
                }
                throw new ArgumentException("Slet Fejlede");
            }
        }

    }
}