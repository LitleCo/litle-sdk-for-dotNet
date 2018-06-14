﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Litle.Sdk.Test.Functional
{
    [TestFixture]
    class TestFraudCheck
    {
        private LitleOnline litle;
        private Dictionary<string, string> config;

        [TestFixtureSetUp]
        public void SetUpLitle()
        {
            config = new Dictionary<string, string>
            {
                {"url", Properties.Settings.Default.url},
                {"reportGroup", "Default Report Group"},
                {"username", "DOTNET"},
                {"version", "10.0"},
                {"timeout", "5000"},
                {"merchantId", "101"},
                {"password", "TESTCASE"},
                {"printxml", "true"},
                {"proxyHost", Properties.Settings.Default.proxyHost},
                {"proxyPort", Properties.Settings.Default.proxyPort},
                {"logFile", Properties.Settings.Default.logFile},
                {"neuterAccountNums", "true"}
            };
            litle = new LitleOnline(config);
        }

        [Test]
        public void TestCustomAttribute7TriggeredRules()
        {
            fraudCheck fraudCheck = new fraudCheck();
            fraudCheck.id = "1";
            fraudCheck.reportGroup = "Planets";
            advancedFraudChecksType advancedFraudCheck = new advancedFraudChecksType();
            fraudCheck.advancedFraudChecks = advancedFraudCheck;
            advancedFraudCheck.threatMetrixSessionId = "123";
            advancedFraudCheck.customAttribute1 = "pass";
            advancedFraudCheck.customAttribute2 = "60";
            advancedFraudCheck.customAttribute3 = "7";
            advancedFraudCheck.customAttribute4 = "jkl";
            advancedFraudCheck.customAttribute5 = "mno";

            fraudCheckResponse fraudCheckResponse = litle.FraudCheck(fraudCheck);

            Assert.NotNull(fraudCheckResponse);
            Assert.AreEqual(60, fraudCheckResponse.advancedFraudResults.deviceReputationScore);
            Assert.AreEqual(7, fraudCheckResponse.advancedFraudResults.triggeredRule.Length);
            Assert.AreEqual("triggered_rule_1", fraudCheckResponse.advancedFraudResults.triggeredRule[0]);
            Assert.AreEqual("triggered_rule_2", fraudCheckResponse.advancedFraudResults.triggeredRule[1]);
            Assert.AreEqual("triggered_rule_3", fraudCheckResponse.advancedFraudResults.triggeredRule[2]);
            Assert.AreEqual("triggered_rule_4", fraudCheckResponse.advancedFraudResults.triggeredRule[3]);
            Assert.AreEqual("triggered_rule_5", fraudCheckResponse.advancedFraudResults.triggeredRule[4]);
            Assert.AreEqual("triggered_rule_6", fraudCheckResponse.advancedFraudResults.triggeredRule[5]);
            Assert.AreEqual("triggered_rule_7", fraudCheckResponse.advancedFraudResults.triggeredRule[6]);
        }

        [Test]
        public void TestFraudCheckWithAddressAndAmount()
        {
            fraudCheck fraudCheck = new fraudCheck();
            fraudCheck.id = "1";
            advancedFraudChecksType advancedFraudCheck = new advancedFraudChecksType();
            contact billToAddress = new contact();
            contact shipToAddresss = new contact();
            fraudCheck.advancedFraudChecks = advancedFraudCheck;
            advancedFraudCheck.threatMetrixSessionId = "123";
            advancedFraudCheck.customAttribute1 = "fail";
            advancedFraudCheck.customAttribute2 = "60";
            advancedFraudCheck.customAttribute3 = "7";
            advancedFraudCheck.customAttribute4 = "jkl";
            advancedFraudCheck.customAttribute5 = "mno";
            billToAddress.firstName = "Bob";
            billToAddress.lastName = "Bagels";
            billToAddress.addressLine1 = "37 Main Street";
            billToAddress.city = "Augusta";
            billToAddress.state = "Wisconsin";
            billToAddress.zip = "28209";
            shipToAddresss.firstName = "P";
            shipToAddresss.lastName = "Sherman";
            shipToAddresss.addressLine1 = "42 Wallaby Way";
            shipToAddresss.city = "Sydney";
            shipToAddresss.state = "New South Wales";
            shipToAddresss.zip = "2127";
            fraudCheck.amount = 51699;
            fraudCheck.billToAddress = billToAddress;
            fraudCheck.shipToAddress = shipToAddresss;

            fraudCheckResponse fraudCheckResponse = litle.FraudCheck(fraudCheck);
            Assert.NotNull(fraudCheckResponse);
            Assert.AreEqual("Call Discover", fraudCheckResponse.message);
            Assert.AreEqual("fail", fraudCheckResponse.advancedFraudResults.deviceReviewStatus);

        }
    }
}
