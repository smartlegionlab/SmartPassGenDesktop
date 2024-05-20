using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

class StringDataMaster
{
    public Random rand = new Random();
    public string numbers = "1234567890";
    public string simbols = "@$!%*#?&-";
    public string smallLetters = "abcdefjhijklmnopqrstuvwxyz";
    public string bigLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public char[] GetNumbers()
    {
        return this.numbers.ToCharArray();
    }

    public char[] GetSimbols()
    {
        return this.simbols.ToCharArray();
    }

    public char[] GetSmallLetters()
    {
        return this.smallLetters.ToCharArray();
    }

    public char[] GetBigLetters()
    {
        return this.bigLetters.ToCharArray();
    }

    public char[] GetAllStrings()
    {
        string letters = this.numbers + this.simbols + this.smallLetters + this.bigLetters;
        return letters.ToCharArray();
    }
}


class BaseRandomGen
{
    StringDataMaster stringDataMaster = new StringDataMaster();

    private string GetRandomString(char[] items, int length)
    {
        string randomString = "";
        for (int i = 1; i <= length; i++)
        {
            int letter_num = this.stringDataMaster.rand.Next(0, items.Length - 1);
            randomString += items[letter_num];
        }

        return randomString;
    }

    public string CreateRandomString(int length)
    {
        char[] allStrings = this.stringDataMaster.GetAllStrings();
        string newString = this.GetRandomString(allStrings, length);
        return newString;
    }

    public string CreateRandomNumbers(int length)
    {
        char[] numbers = this.stringDataMaster.GetNumbers();
        string newString = this.GetRandomString(numbers, length);
        return newString;
    }

    public string CreateRandomSimbols(int length)
    {
        char[] simbols = this.stringDataMaster.GetSimbols();
        string newString = this.GetRandomString(simbols, length);
        return newString;
    }

    public string CreateRandomSmallLetters(int length)
    {
        char[] smallLetters = this.stringDataMaster.GetSmallLetters();
        string newString = this.GetRandomString(smallLetters, length);
        return newString;
    }

    public string CreateRandomBigLetters(int length)
    {
        char[] bigLetters = this.stringDataMaster.GetBigLetters();
        string newString = this.GetRandomString(bigLetters, length);
        return newString;
    }
}


class SmartRandomGen
{

    StringDataMaster stringDataMaster = new StringDataMaster();

    private int GetSeed(string input)
    {
        byte[] hash;
        using (SHA256 sha256 = SHA256.Create())
        {
            hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        }

        int seed = BitConverter.ToInt32(hash, 0);
        return seed;
    }

    private string GetSmartRandomString(char[] items, int length, string seed)
    {
        int seedHash = this.GetSeed(seed);
        Random rand = new Random(seedHash);
        StringBuilder randomString = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            int letter_num = rand.Next(0, items.Length);
            randomString.Append(items[letter_num]);
        }

        return randomString.ToString();
    }

    public string CreateSmartRandomString(int length, string secret)
    {
        char[] allString = this.stringDataMaster.GetAllStrings();
        string newString = this.GetSmartRandomString(allString, length, secret);
        return newString;
    }

}

class SmartPassGen
{
    BaseRandomGen baseRandomGen = new BaseRandomGen();
    SmartRandomGen smartRandomGen = new SmartRandomGen();

    public string CreatePassword(int length)
    {
        return baseRandomGen.CreateRandomString(length);
    }

    public string CreateSmartPassword(int length, string secret)
    {
        return smartRandomGen.CreateSmartRandomString(length, secret);
    }
}

namespace SmartPassGenDesktop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string secretPhrase = secretTextBox.Text;
            int length = Convert.ToInt32(numberSlider.Value);
            SmartPassGen smartPassGen = new SmartPassGen();
            string password = smartPassGen.CreateSmartPassword(length, secretPhrase);
            passwordBox.Text = password;
            resetBtn.IsEnabled = true;
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {

        }

        private void NumberSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (selectedNumberText != null)
            {
                selectedNumberText.Text = numberSlider.Value.ToString();
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            secretTextBox.Text = "";
            numberSlider.Value = 12;
            passwordBox.Text = "Your password will appear here...";
        }
    }
}
