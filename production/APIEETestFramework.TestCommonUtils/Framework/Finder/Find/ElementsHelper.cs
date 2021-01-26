namespace TestCommonUtils
{
    using System;
    using System.Collections.Generic;
    using OpenQA.Selenium;

    public class ElementsHelper
    {
        public static List<IWebElement> FoundElementsSuccess(List<IWebElement> elements, DateTime initialTime, string locatorReference)
        {
            var timeToLocateElement = DateTime.UtcNow - initialTime;
            LoggingHelper.Log($"Elements: {locatorReference} - Time to locate: {timeToLocateElement.TotalMilliseconds}ms, Number of Elements Found: {elements.Count}");
            return elements;
        }

        public static void ElementsNotFoundLogAndThrow(string locatorReference, Exception e)
        {
            LoggingHelper.LogExceptionAndThrow(
                   $"Elements: {locatorReference} - was not found due to Exception: {e}");
        }
    }
}