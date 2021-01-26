namespace TestCommonUtils
{
    using System.Text.RegularExpressions;
    using OpenQA.Selenium;
    using System;

    public class VerifyText
    {
        public static void VerifyDataFieldIsNotEmpty(string propertyName, string property)
        {
            LoggingHelper.Log($"{propertyName}: {property}");
            if (property == string.Empty)
            {
                LoggingHelper.LogExceptionAndThrow($"FAIL - {propertyName} contained no data");
            }
            else
            {
                LoggingHelper.Log($"PASS - {propertyName} did contain data");
            }
        }

        public static void VerifyUrlPrefix(string url)
        {
            if (!url.StartsWith("https://"))
            {
                LoggingHelper.LogExceptionAndThrow($"FAIL - https:// was not found in tableau url: {url}");
            }
            else
            {
                LoggingHelper.Log("PASS - Url did contain https:// prefix");
            }
        }

        public static void AssertNumber(string actualText)
        {
            if (!Regex.IsMatch(actualText, @"^\d+$"))
            {
                LoggingHelper.LogExceptionAndThrow($"FAIL - The actual text: '{actualText}', is not a number.");
            }
            LoggingHelper.Log($"PASS - The actual text: '{actualText}', is a valid number.");
        }

        public static void AssertElementTextEquals(IWebElement element, string expectedText)
        {
            var elementText = element.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None)[0].TrimEnd(' ').TrimStart(' ');
            AssertExactText(elementText, expectedText);
        }

        public static void AssertStringContains(string fullString, string stringToLocate)
        {
            if (!fullString.ToLower().Contains(stringToLocate.ToLower()))
            {
                LoggingHelper.LogExceptionAndThrow($"FAIL -  '{stringToLocate}' was not contained within : '{fullString}'");
            }
            LoggingHelper.Log($"PASS - '{stringToLocate}' was contained within : '{fullString}'");
        }

        public static void AssertExactText(string actualText, string expectedText)
        {
            if (!string.Equals(actualText, expectedText))
            {
                LoggingHelper.LogExceptionAndThrow($"FAIL - The actual text: '{actualText}', did not equal the expected text: '{expectedText}'.");
            }
            LoggingHelper.Log($"PASS - The actual text: '{actualText}', did equal the expected text: '{expectedText}'.");
        }

        public static void AssertTrue(bool result, string message)
        {
            if (!result)
            {
                LoggingHelper.LogExceptionAndThrow($"FAIL - The result was expected to be true but was false. - {message}");
            }
            LoggingHelper.Log($"PASS - The result was expected to be true and was true'- {message}.");
        }

        public static void AreSame<T>(T expected, T actual)
        {
            if (!expected.Equals(actual))
                LoggingHelper.LogExceptionAndThrow(
                    $"FAIL - The actual value: '{actual}', did not equal the expected value: '{expected}'.");
            LoggingHelper.Log(
                $"PASS - The actual value: '{actual}', did equal the expected value: '{expected}'.");
        }
    }
}