namespace TestCommonUtils
{
    using System;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;
    using System.Collections.Generic;

    public class Element
    {
        public static int waitSec=250;
        public static IWebElement FindElement(IWebDriver driver, By locator)
        {
            WaitForElementToBeClickable(driver, locator);
            IWebElement webElement = FindElement(driver, locator, TimeSpan.FromSeconds(30));
            highLightElement(driver, webElement);
            return webElement;
        }

        public static void WaitForElementToBeClickable(IWebDriver driver, By locator)
        {
            try
            {
                new WebDriverWait(driver, TimeSpan.FromSeconds(waitSec)).Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(locator));
                new WebDriverWait(driver, TimeSpan.FromSeconds(waitSec)).Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(locator));
                new WebDriverWait(driver, TimeSpan.FromSeconds(waitSec)).Until(ExpectedConditions.ElementToBeClickable(locator));
                new WebDriverWait(driver, TimeSpan.FromSeconds(waitSec)).Until(ExpectedConditions.ElementIsVisible(locator));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public static void WaitForInvisibilityElement(IWebDriver driver, By locator)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(waitSec)).Until(ExpectedConditions.InvisibilityOfElementLocated(locator));
        }

        public static void highLightElement(IWebDriver driver, IWebElement webElement)
        {
            var jsDriver = (IJavaScriptExecutor)driver;
            var highlightJavascript = @"arguments[0].style.cssText = ""border-width: 2px; border-style: solid; border-color: green"";";
            jsDriver.ExecuteScript(highlightJavascript, new object[] { webElement });
        }

        public static IWebElement FindElement(IWebDriver driver, By locator, TimeSpan maxWaitTime)
        {
            return GetElement(driver, locator, maxWaitTime);
        }

        public static bool IsElementPresent(IWebDriver driver, By locator)
        {
            var message = $"checking if '{locator}' is present";
            try
            {
                driver.FindElement(locator);
                LoggingHelper.Log($"Present - {message} - returning true");
                return true;
            }
            catch (Exception)
            {
                LoggingHelper.Log($"Not Present - {message} - returning false");
            }
            return false;
        }

        public static bool AssertElementIsNotDisplayed(IWebDriver driver, By locator, TimeSpan maxWaitTime)
        {
            DateTime currentTime = DateTime.UtcNow;
            var message = $"Expected {locator} to not be displayed";

            while (DateTime.UtcNow < currentTime.Add(maxWaitTime))
            {
                try
                {
                    driver.FindElement(locator);
                    LoggingHelper.LogExceptionAndThrow($"FAIL - {message}");
                }
                catch (NoSuchElementException)
                {
                    LoggingHelper.Log($"PASS - {message}");
                }
            }
            LoggingHelper.Log($"PASS - {message} and was not located within timeout period of {maxWaitTime}");
            return false;
        }
        public static IWebElement GetShadowRoot(IWebDriver driver, List<By> locatorChain, TimeSpan maxWaitTime)
        {
            DateTime currentTime = DateTime.UtcNow;
            IWebElement currentElement=null;
            IWebElement currentRoot = null;
            var message = "";

            foreach(var locator in locatorChain)
            {
                currentElement = null;
                while (DateTime.UtcNow < currentTime.Add(maxWaitTime))
                {
                    message = $"Expected {locator} to be found";
                    LoggingHelper.Log(message);
                    try
                    {
                        currentElement = currentRoot == null ? driver.FindElement(locator) : currentRoot.FindElement(locator);
                        LoggingHelper.Log($"Pass - {message}");
                        currentRoot = ExpandRootElement(driver, currentElement);
                        break;
                    }
                    catch (NoSuchElementException)
                    {}
                }
                if (currentElement == null)
                {
                    LoggingHelper.LogExceptionAndThrow($"FAIL - {message} and was not found within timeout period of {maxWaitTime}");
                }
               
            }
            return currentRoot;
        }


        public static IWebElement ExpandRootElement(IWebDriver driver, IWebElement element)
        {
            var message = $"Looking for shadowRoot for element {element.TagName}";
            var ele = (IWebElement)((IJavaScriptExecutor)driver)
        .ExecuteScript("return arguments[0].shadowRoot", element);
            if (ele == null)
            {
                LoggingHelper.LogExceptionAndThrow($"FAIL - {message}");
            }
            return ele;
        }


        private static IWebElement GetElement(IWebDriver driver, By locator, TimeSpan maxWaitTime)
        {
            try
            {
                DateTime initialTime = DateTime.UtcNow;
                var el = new WebDriverWait(driver, maxWaitTime).Until(ExpectedConditions.ElementExists(locator));
                var timeToLocateElement = DateTime.UtcNow - initialTime;
                LoggingHelper.Log($"Element: {locator} Time to locate: {timeToLocateElement.TotalMilliseconds}ms");
                return el;
            }
            catch (Exception e)
            {
                LoggingHelper.LogExceptionAndThrow(
                    $"Element: {locator} - was not found due to Exception: {e}");
            }
            LoggingHelper.LogExceptionAndThrow($"Element: {locator} - was not found within timeout period: {maxWaitTime}");

            return null;
        }
    }
}