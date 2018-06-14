﻿using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Litle.Sdk;

namespace Litle.Sdk.Test.Functional
{
    [TestFixture]
    class TestSale
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
        public void SimpleSaleWithCard()
        {
            sale saleObj = new sale();
            saleObj.id = "1";
            saleObj.amount = 106;
            saleObj.litleTxnId = 123456;
            saleObj.orderId = "12344";
            saleObj.orderSource = orderSourceType.ecommerce;
            cardType cardObj = new cardType();
            cardObj.type = methodOfPaymentTypeEnum.VI;
            cardObj.number = "4100000000000000";
            cardObj.expDate = "1210";
            saleObj.card = cardObj;

            saleResponse responseObj = litle.Sale(saleObj);
            StringAssert.AreEqualIgnoringCase("Transaction Received", responseObj.message);
        }

        [Test]
        public void SimpleSaleWithMpos()
        {
            sale saleObj = new sale();
            saleObj.id = "1";
            saleObj.amount = 106;
            saleObj.litleTxnId = 123456;
            saleObj.orderId = "12344";
            saleObj.orderSource = orderSourceType.ecommerce;
            mposType mpos = new mposType();
            mpos.ksn = "77853211300008E00016";
            mpos.encryptedTrack = "CASE1E185EADD6AFE78C9A214B21313DCD836FDD555FBE3A6C48D141FE80AB9172B963265AFF72111895FE415DEDA162CE8CB7AC4D91EDB611A2AB756AA9CB1A000000000000000000000000000000005A7AAF5E8885A9DB88ECD2430C497003F2646619A2382FFF205767492306AC804E8E64E8EA6981DD";
            mpos.formatId = "30";
            mpos.track1Status = 0;
            mpos.track2Status = 0; ;
            saleObj.mpos = mpos;

            saleResponse responseObj = litle.Sale(saleObj);
            StringAssert.AreEqualIgnoringCase("Transaction Received", responseObj.message);
        }

        [Test]
        public void SimpleSaleWithPayPal()
        {
            sale saleObj = new sale();
            saleObj.id = "1";
            saleObj.amount = 106;
            saleObj.litleTxnId = 123456;
            saleObj.orderId = "12344";
            saleObj.orderSource = orderSourceType.ecommerce;
            payPal payPalObj = new payPal();
            payPalObj.payerId = "1234";
            payPalObj.token = "1234";
            payPalObj.transactionId = "123456";
            saleObj.paypal = payPalObj;
            saleResponse responseObj = litle.Sale(saleObj);
            StringAssert.AreEqualIgnoringCase("Transaction Received", responseObj.message);
        }
        
        [Test]
        public void SimpleSaleWithDirectDebit()
        {
            var saleObj = new sale
            {
                id = "1",
                amount = 106,
                litleTxnId = 123456,
                orderId = "12344",
                orderSource = orderSourceType.ecommerce,
                sepaDirectDebit = new sepaDirectDebitType
                {
                    mandateProvider = mandateProviderType.Merchant,
                    sequenceType = sequenceTypeType.FirstRecurring,
                    iban = "123456789123456789",
                    preferredLanguage = countryTypeEnum.US
                }
            };

            var responseObj = litle.Sale(saleObj);
            StringAssert.AreEqualIgnoringCase("Transaction Received", responseObj.message);
        }
        
        [Test]
        public void SimpleSaleWithProcessTypeNetIdTranAmt()
        {
            var saleObj = new sale
            {
                id = "1",
                amount = 106,
                litleTxnId = 123456,
                orderId = "12344",
                orderSource = orderSourceType.ecommerce,
                card = new cardType
                {
                    type = methodOfPaymentTypeEnum.VI,
                    number = "4100000000000000",
                    expDate = "1210"
                },

                processingType = processingTypeEnumType.initialRecurring,
                originalNetworkTransactionId = "123456789123456789123456789",
                originalTransactionAmount = 12
            };

            var responseObj = litle.Sale(saleObj);
            StringAssert.AreEqualIgnoringCase("Transaction Received", responseObj.message);
        }

        [Test]
        public void SimpleSaleWithApplepayAndSecondaryAmountAndWallet()
        {
            sale saleObj = new sale();
            saleObj.id = "1";
            saleObj.amount = 110;
            saleObj.secondaryAmount = 50;
            saleObj.litleTxnId = 123456;
            saleObj.orderId = "12344";
            saleObj.orderSource = orderSourceType.ecommerce;
            applepayType applepay = new applepayType();
            applepayHeaderType applepayHeaderType = new applepayHeaderType();
            applepayHeaderType.applicationData = "454657413164";
            applepayHeaderType.ephemeralPublicKey = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
            applepayHeaderType.publicKeyHash = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855";
            applepayHeaderType.transactionId = "1234";
            applepay.header = applepayHeaderType;
            applepay.data = "user";
            applepay.signature = "sign";
            applepay.version = "12345";
            saleObj.applepay = applepay;
            wallet wallet = new Sdk.wallet();
            wallet.walletSourceTypeId = "123";
            wallet.walletSourceType = walletWalletSourceType.MasterPass;
            saleObj.wallet = wallet;

            saleResponse responseObj = litle.Sale(saleObj);
            Assert.AreEqual("Transaction Received", responseObj.message);
            Assert.AreEqual("110", responseObj.applepayResponse.transactionAmount);
        }

        [Test]
        public void SimpleSaleWithInvalidFraudCheck()
        {
            sale saleObj = new sale();
            saleObj.id = "1";
            saleObj.amount = 106;
            saleObj.litleTxnId = 123456;
            saleObj.orderId = "12344";
            saleObj.orderSource = orderSourceType.ecommerce;
            cardType cardObj = new cardType();
            cardObj.type = methodOfPaymentTypeEnum.VI;
            cardObj.number = "4100000000000000";
            cardObj.expDate = "1210";
            saleObj.card = cardObj;
            fraudCheckType cardholderAuthentication = new fraudCheckType();
            cardholderAuthentication.authenticationValue = "123456789012345678901234567890123456789012345678901234567890";
            saleObj.cardholderAuthentication = cardholderAuthentication;

            try
            {
                saleResponse responseObj = litle.Sale(saleObj);
            }
            catch (LitleOnlineException e)
            {
                Assert.True(e.Message.StartsWith("Error validating xml data against the schema"));
            }
        }
    }
}
