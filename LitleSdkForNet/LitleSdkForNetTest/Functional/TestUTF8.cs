﻿using System.Collections.Generic;
using NUnit.Framework;

namespace Litle.Sdk.Test.Functional
{
    [TestFixture]
    internal class TestUTF8
    {
        private LitleOnline _litle;
        private Dictionary<string, string> _config;

        [TestFixtureSetUp]
        public void SetUpLitle()
        {
            _config = new Dictionary<string, string>
            {
                {"url", Properties.Settings.Default.url},
                {"reportGroup", "Default Report Group"},
                {"username", "DOTNET"},
                {"version", "11.0"},
                {"timeout", "5000"},
                {"merchantId", "101"},
                {"password", "TESTCASE"},
                {"printxml", "true"},
                {"proxyHost", Properties.Settings.Default.proxyHost},
                {"proxyPort", Properties.Settings.Default.proxyPort},
                {"logFile", Properties.Settings.Default.logFile},
                {"neuterAccountNums", "true"}
            };
            
            _litle = new LitleOnline(_config);
        }

        [Test]
        public void SimpleAuthWithUnicode()
        {
            var authorization = new authorization
            {
                reportGroup = "русский中文",
                orderId = "12344",
                amount = 106,
                orderSource = orderSourceType.ecommerce,
                card = new cardType
                {
                    type = methodOfPaymentTypeEnum.VI,
                    number = "4100000000000000",
                    expDate = "1210"
                },
                billToAddress = new contact
                {
                    addressLine1 = "Planetsрусский中文",
                    zip = "01803-3747",
                    firstName = "vvantiv"
                }
            };
            
            var response = _litle.Authorize(authorization);
            Assert.AreEqual("русский中文", response.reportGroup);
        }

    }
}
