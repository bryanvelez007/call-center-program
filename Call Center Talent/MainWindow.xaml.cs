using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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


namespace ModernUiFlatDesign
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();

          
            Paragraph p = resultInfo.Document.Blocks.FirstBlock as Paragraph;
            Paragraph p2 = txtEmpresas.Document.Blocks.FirstBlock as Paragraph;
            p.LineHeight = 3;
            p2.LineHeight = 3;

           

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Start(Object sender, RoutedEventArgs e)
        {

            Thread t = new Thread(initializeSearch);
            t.Start();
          
        }

        public void initializeSearch()
        {
            String contentTextbox = StringFromRichTextBox(txtEmpresas);
            string[] wordsOfEmpresas = contentTextbox.Split('\n');

          

            foreach (String word in wordsOfEmpresas)
            {
                if (word.Length > 1)
                {
                    string wordsForSearch = word.Replace("&", "%26").Replace(" ", "+");
                    searchPhoneNumer(wordsForSearch);
                }
              
            }

            // searchPhoneNumer();

        }

        public void searchPhoneNumer(String wordSearchs)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            IWebDriver driver = new ChromeDriver(chromeOptions);
            String wordsSearchNumber = wordSearchs +" canada";
            String wordsSearchEmail = wordSearchs + " contact+us";
            String wordsSearchIndeed = wordSearchs.Replace("+", "-").ToLower();


            String validPhoneNumber = "Undefined";
            String validEmail = "Undefined";
            String JobsActiveIndeed = "false";
            String pageBody = "";

            ///////////////////////////////////////////////////////////////////////////
            ///  GO TO PHONE NUMBER
            /////////////////////////////////////////////////////////////////////////////


            driver.Navigate().GoToUrl("https://www.google.com/");
            driver.Navigate().GoToUrl("https://www.google.com/search?tbs=lf:1,lf_ui:9&tbm=lcl&sxsrf=AOaemvLA2kIZqGctPAzk-YphzdzNBQSetA:1642313898755&q=" + wordsSearchNumber);
            driver.Manage().Window.Maximize();

            var elementPresentCompany = true;


            try
            {
                IWebElement spIcon2 = driver.FindElement(By.CssSelector("div[jsname='GZq3Ke']"));
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                var clickableElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div[jsname='GZq3Ke']")));
                driver.FindElement(By.CssSelector("div[jsname='GZq3Ke']")).Click();
            }
            catch (NoSuchElementException spIconNotDisplayed)
            {
                elementPresentCompany = false;
            }

            if (elementPresentCompany)
            {
                driver.FindElement(By.CssSelector("div[jsname='GZq3Ke']")).Click();
            }


            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            WebDriverWait waitNew = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            var elementPresent = true;

            try
            {
                IWebElement spIcon = driver.FindElement(By.CssSelector("span.LrzXr.zdqRlf.kno-fv a span"));
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                var clickableElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("span.LrzXr.zdqRlf.kno-fv a span")));
                validPhoneNumber = driver.FindElement(By.CssSelector("span.LrzXr.zdqRlf.kno-fv a span")).Text;
            }
            catch (NoSuchElementException spIconNotDisplayed)
            {
                elementPresent = false;
            }

            if (elementPresent)
            {
                validPhoneNumber = driver.FindElement(By.CssSelector("span.LrzXr.zdqRlf.kno-fv a span")).Text;
            }




            ///////////////////////////////////////////////////////////////////////////
            ///  GO TO EMAIL
            /////////////////////////////////////////////////////////////////////////////

            driver.Navigate().GoToUrl("https://www.google.com/search?q=" + wordsSearchEmail);

            var wait2 = new WebDriverWait(driver, TimeSpan.FromMinutes(5));

            List<IWebElement> elementListGoogle = new List<IWebElement>();
            elementListGoogle.AddRange(driver.FindElements(By.CssSelector("div.yuRUbf")));
            if (elementListGoogle.Count > 0)
            {
                var clickableElement2 = wait2.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("div.yuRUbf")));
                driver.FindElement(By.CssSelector("div.yuRUbf a")).Click();
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);

                pageBody = driver.FindElement(By.CssSelector("body")).Text;


            } else
            {
                pageBody = "";
            }


            string[] wordsOfPage = pageBody.Split(new string[] { "\r\n", "\r", "\n", " " }, StringSplitOptions.None);

            try
            {
                foreach (String word in wordsOfPage)
                {
                  
                    bool isEmail = IsValidEmail(word);
                    if (isEmail)
                    {
                        validEmail = word;
                        break;
                    }
                    else
                    {
                        validEmail = "Undefined";
                    }
                }
            }
            catch (InvalidCastException e)
            {
                validEmail = "Undefined";
            }


            ///////////////////////////////////////////////////////////////////////////
            ///  GO TO INDEED
            /////////////////////////////////////////////////////////////////////////////

            // Bombardier cmp indeed  -> usar esta busqueda para google


            driver.Navigate().GoToUrl("https://www.google.com/");
            driver.Navigate().GoToUrl("https://www.indeed.com/cmp/"+wordsSearchIndeed+"/jobs");

            try
            {
              
                List<IWebElement> elementList = new List<IWebElement>();
                elementList.AddRange(driver.FindElements(By.CssSelector("a.css-e57c2u.e37uo190")));

                if (elementList.Count > 0)
                {
                    JobsActiveIndeed = "true";
                }
                else
                {
                    JobsActiveIndeed = "false";
                }

            }
            catch(InvalidCastException e)
            {
                JobsActiveIndeed = "false";
            }
            

            this.Dispatcher.Invoke(() =>
            {
                String wordOfCompany = wordSearchs.Replace("+", " ");

                ColorFromRichTextBox(resultInfo, "🏢 ", "#fc6586");
                ColorFromRichTextBox(resultInfo, wordOfCompany+"\n", "#383660");
                ColorFromRichTextBox(resultInfo, "📞 ", "#fc6586");
                ColorFromRichTextBox(resultInfo, validPhoneNumber + "\n", "#383660");
                ColorFromRichTextBox(resultInfo, "📨 ", "#fc6586");
                ColorFromRichTextBox(resultInfo, validEmail + "\n", "#383660");
                ColorFromRichTextBox(resultInfo, "💻 ", "#fc6586");
                ColorFromRichTextBox(resultInfo, JobsActiveIndeed+ "\n\n", "#383660");

               
            });

            driver.Quit();


        }

        public bool IsValidEmail(String email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }

        string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                rtb.Document.ContentStart,
                // TextPointer to the end of content in the RichTextBox.
                rtb.Document.ContentEnd
            );

            // The Text property on a TextRange object returns a string
            // representing the plain text content of the TextRange.
            return textRange.Text;
        }

        void ColorFromRichTextBox(RichTextBox box, string text, string color)
        {
            BrushConverter bc = new BrushConverter();
            TextRange tr = new TextRange(box.Document.ContentEnd, box.Document.ContentEnd);
            tr.Text = text;
            try
            {
                tr.ApplyPropertyValue(TextElement.ForegroundProperty,
                    bc.ConvertFromString(color));
            }
            catch (FormatException) { }
        }

    }
}
