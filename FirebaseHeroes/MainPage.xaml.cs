using FirebaseHeroes.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FirebaseHeroes
{
    public sealed partial class MainPage : Page
    {
        public static List<Hero> heroList;

        public MainPage()
        {
            this.InitializeComponent();
            // get data and update UI
            GetHeroes();
        }

        // call the HeroConn method, populate heroList and also update the UI
        public void GetHeroes()
        {
            // create a new List
            heroList = new List<Hero>();
            // and a new connection
            HeroConn conn = new HeroConn();
            // get the data and add it to the heroList
            Task.Run(() => conn.Get()).Wait();

            // clear all HeroItems from the StackPanel
            this.spHeroList.Children.Clear();
            // and add new ones
            foreach (Hero h in heroList)
            {
                this.spHeroList.Children.Add(new HeroItem(h, this));
            }
        }

        public static void UpdateUI(MainPage mp)
        {
            mp.GetHeroes();
        }

        private async void btnAdd_Tapped(object sender, TappedRoutedEventArgs e)
        {
            HeroConn heroConn = new HeroConn();
            Hero hero = new Hero
            {
                Name = this.tbInputName.Text,
                Rating = Convert.ToInt32(this.tbInputRating.Text)
            };
            await heroConn.Insert(hero);
            this.tbOutput.Text = String.Format("Hero {0} added!", hero.Name);

            // get new data and update UI
            GetHeroes();
        }

        private void btnSearch_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var results = heroList.Where(h => h.Name.ToLower().Contains(tbQuery.Text.ToLower().Trim()));
            foreach (var item in results)
            {
                Debug.WriteLine(item.Name);
            }
        }
    }
}
