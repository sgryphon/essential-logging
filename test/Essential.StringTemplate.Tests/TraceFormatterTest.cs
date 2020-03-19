using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Essential.Tests.Utility;
using Microsoft.Extensions.Logging;

namespace Essential.Tests
{
    [TestClass]
    public class TraceFormatterTest
    {
        [TestMethod()]
        public void FormatIdAndMessageTest()
        {
            string categoryName = "test";
            LogLevel logLevel = LogLevel.Warning;
            EventId eventId = new EventId(5, "five");
            Exception? ex = null;
            string message = "fnord";
            object[]? scopes = null;
            string template = "{Id}.{Message}";

            var logTemplate = new LogTemplate(template);
            var actual = logTemplate.Bind(categoryName, logLevel, eventId, message, ex, scopes);

            string expected = "5.fnord";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void FormatPrincipalNameTest()
        {
            string categoryName = "test";
            LogLevel logLevel = LogLevel.Warning;
            EventId eventId = new EventId(5, "five");
            Exception? ex = null;
            string message = "fnord";
            object[]? scopes = null;
            string template = "{Id}.{PrincipalName}";

            var logTemplate = new LogTemplate(template);
            string actual = null;
            using (var scope = new UserResetScope("testuser"))
            {
                actual = logTemplate.Bind(categoryName, logLevel, eventId, message, ex, scopes);
            }
            
            string expected = "5.testuser";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void FormatProcessIdTest()
        {
            string categoryName = "test";
            LogLevel logLevel = LogLevel.Warning;
            EventId eventId = new EventId(5, "five");
            Exception? ex = null;
            string message = "fnord";
            object[]? scopes = null;
            string template = "{ProcessId}";

            var logTemplate = new LogTemplate(template);
            var actual = logTemplate.Bind(categoryName, logLevel, eventId, message, ex, scopes);

            string expected = Process.GetCurrentProcess().Id.ToString();
            Assert.AreEqual(expected, actual);
        }

        // [TestMethod()]
        // public void FormatHttpRequestUrlTest()
        // {
        //     var mockHttpTraceContext  =new MockHttpTraceContext();
        //     mockHttpTraceContext.RequestUrl = new Uri("http://test/x");
        //     var traceFormatter = new TraceFormatter();
        //     traceFormatter.HttpTraceContext = mockHttpTraceContext;
        //     string categoryName = "test";
        //     LogLevel logLevel = LogLevel.Warning;
        //     int id = 5;
        //     string message = "fnord";
        //     Guid? relatedActivityId = null;
        //     object[] data = null;
        //     string template = "|{RequestUrl}|";
        //     string expected = "|http://test/x|";
        //
        //     var actual = traceFormatter.Format(template, categoryName, logLevel, id, message, data);
        //
        //     Assert.AreEqual(expected, actual);
        // }

        // [TestMethod()]
        // public void FormatEmptyHttpContextTest()
        // {
        //     // The default is HttpContext.Current, which should be empty when running unit test
        //     var traceFormatter = new TraceFormatter();
        //     string categoryName = "test";
        //     LogLevel logLevel = LogLevel.Warning;
        //     int id = 5;
        //     string message = "fnord";
        //     Guid? relatedActivityId = null;
        //     object[] data = null;
        //     string template = "|{RequestUrl}|";
        //     string expected = "||";
        //
        //     var actual = traceFormatter.Format(template, categoryName, logLevel, id, message, data);
        //
        //     Assert.AreEqual(expected, actual);
        // }

        [TestMethod()]
        public void FormatMessagePrefixAll()
        {
            string categoryName = "test";
            LogLevel logLevel = LogLevel.Warning;
            EventId eventId = new EventId(5, "five");
            Exception? ex = null;
            string message = "Something to say";
            object[]? scopes = null;
            string template = "<{MessagePrefix}>";
            
            var logTemplate = new LogTemplate(template);
            var actual = logTemplate.Bind(categoryName, logLevel, eventId, message, ex, scopes);

            string expected = "<Something to say>";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void FormatMessagePrefixSentinel()
        {
            string categoryName = "test";
            LogLevel logLevel = LogLevel.Warning;
            EventId eventId = new EventId(5, "five");
            Exception? ex = null;
            string message = "Something to say. the rest of the trace.";
            object[]? scopes = null;
            string template = "<{MessagePrefix}>";

            var logTemplate = new LogTemplate(template);
            var actual = logTemplate.Bind(categoryName, logLevel, eventId, message, ex, scopes);

            string expected = "<Something to say>";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void FormatMessagePrefixLength()
        {
            string categoryName = "test";
            LogLevel logLevel = LogLevel.Warning;
            EventId eventId = new EventId(5, "five");
            Exception? ex = null;
            //                1234567890123456789012345678901234567890
            string message = "Something to say Something to say Something to say. the rest of the trace.";
            object[]? scopes = null;
            string template = "<{MessagePrefix}>";

            var logTemplate = new LogTemplate(template);
            var actual = logTemplate.Bind(categoryName, logLevel, eventId, message, ex, scopes);

            string expect = "<Something to say Something to say Som...>";
            Assert.AreEqual(expect, actual);
        }

        [TestMethod()]
        public void FormatMessagePrefixControlCharacter()
        {
            string categoryName = "test";
            LogLevel logLevel = LogLevel.Warning;
            EventId eventId = new EventId(5, "five");
            Exception? ex = null;
            string message = "Something to\tsay";
            object[]? scopes = null;
            string template = "<{MessagePrefix}>";

            var logTemplate = new LogTemplate(template);
            var actual = logTemplate.Bind(categoryName, logLevel, eventId, message, ex, scopes);
            
            string expected = "<Something tosay>";
            Assert.AreEqual(expected, actual);
        }

    }
}
