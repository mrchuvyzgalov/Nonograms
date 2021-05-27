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
                foreach (char elem in Name.Text)
                {
                    if (!Char.IsLetter(elem))
                    {
                        throw new Exception("Название должно состоять только из букв");
                    }
                }
                int rows;
                if (int.TryParse(@CountOfRows.Text, out rows) && rows > 0 && rows < 11)
                {
                    int cols;
                    if (int.TryParse(@CountOfCols.Text, out cols) && cols > 0 && cols < 11)
                    {
                        HashSet<ColorRGB> colors = new HashSet<ColorRGB>();

                        Stack<string> stack = new Stack<string>();
                        if (Colors.Text.Length > 0 && @Colors.Text[Colors.Text.Length - 1] != ')')
                        {
                            throw new Exception("Неправильный формат ввода цветов");
                        }
                        foreach (char elem in @Colors.Text)
                        {
                            if (elem == '(')
                            {
                                if (stack.Count != 0)
                                {
                                    throw new Exception("Неправильный формат ввода цветов");
                                }
                                stack.Push("" + elem);
                            }
                            else if (elem == ')')
                            {
                                if (stack.Count != 6)
                                {
                                    throw new Exception("Неправильный формат ввода цветов");
                                }

                                int blue;
                                if (int.TryParse(stack.Peek(), out blue) && blue >= 0 && blue <= 255)
                                {
                                    stack.Pop();
                                    if (stack.Peek() == ",")
                                    {
                                        stack.Pop();

                                        int green;
                                        if (int.TryParse(stack.Peek(), out green) && green >= 0 && green <= 255)
                                        {
                                            stack.Pop();
                                            if (stack.Peek() == ",")
                                            {
                                                stack.Pop();

                                                int red;
                                                if (int.TryParse(stack.Peek(), out red) && red >= 0 && red <= 255)
                                                {
                                                    stack.Pop();

                                                    if (stack.Peek() == "(")
                                                    {
                                                        stack.Pop();
                                                        colors.Add(new ColorRGB(red, green, blue));
                                                    }
                                                    else
                                                    {
                                                        throw new Exception("Неправильный формат ввода цветов");
                                                    }
                                                }
                                                else
                                                {
                                                    throw new Exception("Неправильный формат ввода красного цвета");
                                                }
                                            }
                                            else
                                            {
                                                throw new Exception("Неправильный формат ввода цветов");
                                            }
                                        }
                                        else
                                        {
                                            throw new Exception("Неправильный формат ввода зеленого цвета");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Неправильный формат ввода цветов");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Неправильный формат ввода голубого цвета");
                                }
                            }
                            else if (elem == ',')
                            {
                                if (stack.Count < 2)
                                {
                                    throw new Exception("Неправильный формат ввода цветов");
                                }
                                if (!Char.IsDigit(stack.Peek()[0]))
                                {
                                    throw new Exception("Неправильный формат ввода цветов");
                                }
                                stack.Push("" + elem);
                            }
                            else if (Char.IsDigit(elem))
                            {
                                if (stack.Count == 0)
                                {
                                    throw new Exception("Неправильный формат ввода цветов");
                                }
                                if (stack.Peek() == "(" || stack.Peek() == ",")
                                {
                                    stack.Push("" + elem);
                                }
                                else
                                {
                                    string last = stack.Peek();
                                    stack.Pop();
                                    stack.Push(last + elem);
                                }
                            }
                            else
                            {
                                throw new Exception("Некорректный символ: " + elem);
                            }
                        }

                        if (stack.Count != 0)
                        {
                            throw new Exception("Неправильный формат ввода цветов");
                        }

                        await Navigation.PushAsync(new DrawNonogram(Name.Text, rows, cols, colors));
                    }
                    else
                    {
                        throw new Exception("Некорректное число столбцов. Оно должно быть от 1 до 10");
                    }
                }
                else
                {
                    throw new Exception("Некорректное число строк. Оно должно быть от 1 до 10");
                }
            }
            catch (Exception error)
            {
                await DisplayAlert("Ошибка", error.Message, "Ok");
            }
        }
    }
}