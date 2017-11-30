using System;
using System.Threading.Tasks;
using Firebase.Database;
using System.Collections.Generic;
using System.Diagnostics;
using Firebase.Database.Query;

namespace FirebaseHeroes.Data
{
    public class HeroConn
    {
        private const String databaseUrl = DatabaseCredentials_Template.databaseUrl;
        private const String databaseSecret = DatabaseCredentials_Template.databaseSecret;
        private const String node = "heroes/";

        private FirebaseClient firebase;

        private Hero result;

        public HeroConn()
        {
            this.firebase = new FirebaseClient(
                databaseUrl,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(databaseSecret)
                });
        }

        public async Task Insert(Hero hero)
        {
            await firebase.Child(node).PostAsync<Hero>(hero);
        }

        // retrieve the database records and add a new Hero object to 
        // heroList on MainPage.xaml.cs for each object
        public async Task Get()
        {
            var results = await firebase.Child(node).OnceAsync<Hero>();
            foreach (var item in results)
            {
                MainPage.heroList.Add(new Hero
                {
                    Key = item.Key,
                    Name = item.Object.Name,
                    Rating = Convert.ToInt32(item.Object.Rating)
                });
            }
        }

        public async Task Delete(Hero hero)
        {
            await firebase.Child(node).Child(hero.Key).DeleteAsync();
        }

        public async void Update(Hero hero)
        {
            await firebase.Child(node).Child(hero.Key).PutAsync<Hero>(hero);
        }

        public async void Get(string key)
        {
            Hero hero = new Hero();
            var result = await firebase.Child(node).Child(key).OnceAsync<Hero>();
            foreach (var item in result)
            {
                hero.Key = item.Key;
                hero.Name = item.Object.Name;
                hero.Rating = Convert.ToInt32(item.Object.Rating);
            }
            this.result = hero;
        }

        public Hero GetHero()
        {
            return this.result;
        }
    }
}
