namespace TestCommonUtils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    public class Elements
    {
        public static List<IWebElement> FindElements(IWebDriver driver, By locator)
        {
            return FindElements(driver, locator, TimeSpan.FromSeconds(30));
        }

        public static List<IWebElement> FindElements(IWebDriver driver, By locator, TimeSpan maxWaitTime)
        {
            var elements = GetElements(driver, locator, maxWaitTime);
            return elements != null
                ? ElementsHelper.FoundElementsSuccess(elements, DateTime.UtcNow, locator.ToString())
                : null;
        }

        private static List<IWebElement> GetElements(IWebDriver driver, By locator, TimeSpan maxWaitTime)
        {
            try
            {
                return
                    new WebDriverWait(driver, maxWaitTime).Until(
                        ExpectedConditions.PresenceOfAllElementsLocatedBy(locator)).ToList();
            }
            catch (Exception e)
            {
                LoggingHelper.Log($"Exception - '{locator}' - was not found, Exception - '{e}'");
                return null;
            }
        }
    }
}