using FirebaseHeroes.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FirebaseHeroes
{
    public class HeroItem : StackPanel
    {
        Hero hero;
        MainPage mainPage;

        TextBox tbName;
        TextBox tbRating;

        public HeroItem(Hero hero, MainPage mainPage)
        {
            this.hero = hero;
            this.mainPage = mainPage;

            // some layout options for the HeroItem
            this.Orientation = Orientation.Horizontal;
            this.Padding = new Thickness(10);

            // add the name
            tbName = new TextBox();
            tbName.Width = 150;
            tbName.Text = this.hero.Name;

            // add the rating
            tbRating= new TextBox();
            tbRating.Width = 25;
            tbRating.Text = this.hero.Rating.ToString();

            // add the Update button
            Button btnUpdate = new Button();
            btnUpdate.Content = "Update";
            btnUpdate.Tapped += BtnUpdate_Tapped;

            // add the Delete button
            Button btnDelete = new Button();
            btnDelete.Content = "Delete";
            btnDelete.Tapped += BtnDelete_Tapped;

            // add the elements to this stackpanel
            this.Children.Add(tbName);
            this.Children.Add(tbRating);
            this.Children.Add(btnUpdate);
            this.Children.Add(btnDelete);

        }

        private void BtnUpdate_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            HeroConn conn = new HeroConn();
            this.hero.Name = this.tbName.Text;
            this.hero.Rating = Convert.ToInt32(this.tbRating.Text);

            Task.Run(() => conn.Update(this.hero)).Wait();
            this.mainPage.GetHeroes();
        }

        private void BtnDelete_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            HeroConn conn = new HeroConn();
            Task.Run(() => conn.Delete(this.hero)).Wait();
            this.mainPage.GetHeroes();
        }
    }
}
