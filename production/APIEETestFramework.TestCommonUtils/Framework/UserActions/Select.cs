namespace TestCommonUtils
{
    using System;
    using System.Threading;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public class Select
    {
        private readonly IWebDriver _driver;

        public Select(IWebDriver driver)
        {
            _driver = driver;
        }
        public static void ElementSelectByIndex(IWebElement element, int index)
        {
            SelectElement s = new SelectElement(element);
            Thread.Sleep(TimeSpan.FromSeconds(2));
            s.SelectByIndex(index);
        }

        public static void ElementSelectByText(IWebDriver driver, IWebElement element, string val)
        {
            SelectElement s = new SelectElement(element);
            Element.WaitForElementToBeClickable(driver, By.XPath("//*[text()='" + val + "']"));
            s.SelectByText(val);
        }

        public static void ElementSelectByValue(IWebDriver driver, IWebElement element, string val)
        {
            SelectElement s = new SelectElement(element);
            Element.WaitForElementToBeClickable(driver, By.CssSelector("[value='" + val + "']"));
            s.SelectByValue(val);
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