namespace TestCommonUtils
{
    using System;
    using System.Drawing;
    using System.Reflection;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.Support.UI;

    public class Click
    {
        private readonly IWebDriver _driver;

        public Click(IWebDriver driver)
        {
            _driver = driver;
        }

        public static void MoveToElement(IWebDriver driver, IWebElement element)
        {
            Actions builder = new Actions(driver);
            builder.MoveToElement(element).Build().Perform();
        }

        public static void ClickElement(IWebDriver driver, IWebElement element, string elementDescription)
        {
            var el = WaitUntilClickable(driver, element, elementDescription);

            Actions builder = new Actions(driver);
            var action = builder.Click(el).Release();
            ActionHandler(action, MethodBase.GetCurrentMethod().Name, elementDescription);
        }

        public static void ClickElementWithOffset(IWebDriver driver, IWebElement element, string elementDescription)
        {
            var el = WaitUntilClickable(driver, element, elementDescription);

            Actions builder = new Actions(driver);
            var action = builder.MoveToElement(el, 1, 1).Click().Release();
            ActionHandler(action, MethodBase.GetCurrentMethod().Name, elementDescription);
        }

        public static void SelectElement(IWebDriver driver, IWebElement element, string elementDescription)
        {
            Point bottomRight = new Point { X = element.Size.Width - 1, Y = element.Size.Height - 1 };
            Point topLeft = new Point { X = 1, Y = 1 };

            Actions builder = new Actions(driver);
            var action =
                builder.MoveToElement(element, bottomRight.X, bottomRight.Y)
                    .ClickAndHold()
                    .MoveToElement(element, topLeft.X, topLeft.Y)
                    .Release();
            ActionHandler(action, MethodBase.GetCurrentMethod().Name, elementDescription);
        }

        public void Hover(IWebElement element, string elementDescription)
        {
            Actions builder = new Actions(_driver);
            var action = builder.MoveToElement(element, 1, 1);
            ActionHandler(action, MethodBase.GetCurrentMethod().Name, elementDescription);
        }

        private static void ActionHandler(Actions builder, string actionText, string element)
        {
            try
            {
                builder.Build().Perform();
            }
            catch (Exception e)
            {
                LoggingHelper.LogExceptionAndThrow(
                    $"FAIL - UnSuccessfull Action '{actionText}' performed on element: '{element}'. Exception: {e}");
            }
            LoggingHelper.Log($"PASS - Action '{actionText}' has been performed on '{element}'");
        }

        private static IWebElement WaitUntilClickable(IWebDriver driver, IWebElement element, string elementDescription)
        {
            try
            {
                AssertVisible(element, elementDescription);
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
                wait.Until(ExpectedConditions.ElementToBeClickable(element));
            }
            catch (Exception e)
            {
                LoggingHelper.LogExceptionAndThrow($"element '{elementDescription}' was not clickable. Exception: {e}");
            }
            LoggingHelper.Log($"element '{elementDescription}' is clickable");
            return element;
        }

        private static void AssertVisible(IWebElement element, string elementDescription)
        {
            LoggingHelper.Log($"Asserting element '{elementDescription}' is displayed");
            var visible = false;
            var maxWait = DateTime.UtcNow.AddSeconds(30);
            while (DateTime.UtcNow < maxWait && !visible)
            {
                visible = element.Displayed;
            }
            if(!visible)
                LoggingHelper.LogExceptionAndThrow($"The element '{elementDescription}' was not displayed with in timeout period");

            LoggingHelper.Log($"Element '{elementDescription}' is displayed");
        }
    }

}