using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Nonograms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateNonogram : ContentPage
    {
        public CreateNonogram()
        {
            InitializeComponent();
        }

        private async void StartButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (Name.Text == "")
                {
                    throw new Exception("Вы не указали название");
                }
                else if (Name.Text.Length > 10)
                {
                    throw new Exception("Вы указали слишком длинное название. Максимальное число символов: 10");
                }
                foreach (char elem in Name.Text)
                {
                    if (!Char.IsLetter(elem) && elem != ' ')
                    {
                        throw new Exception("Название должно состоять только из букв");
                    }
                }
                bool hasLetter = false;
                foreach (char elem in Name.Text)
                {

                    if (Char.IsLetter(elem))
                    {
                        hasLetter = true;
                        break;
                    }
                }

                if (!hasLetter)
                {
                    throw new Exception("Вы не указали название");
                }
                int rows;
                if (CountOfRows.Text == "")
                {
                    throw new Exception("Вы не указали число строк");
                }
                if (int.TryParse(@CountOfRows.Text, out rows) && rows > 0 && rows < 8)
                {
                    int cols;
                    if (CountOfCols.Text == "")
                    {
                        throw new Exception("Вы не указали число столбцов");
                    }
                    if (int.TryParse(@CountOfCols.Text, out cols) && cols > 0 && cols < 8)
                    {
                        await Navigation.PushAsync(new DrawNonogram(Name.Text, rows, cols));
                    }
                    else
                    {
                        throw new Exception("Некорректное число столбцов. Оно должно быть от 1 до 7");
                    }
                }
                else
                {
                    throw new Exception("Некорректное число строк. Оно должно быть от 1 до 7");
                }
            }
            catch (Exception error)
            {
                await DisplayAlert("Ошибка", error.Message, "Ok");
            }
        }
    }
}