using crds_angular.Exceptions;
using crds_angular.Models.Crossroads;
using crds_angular.Services;
using Moq;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;


namespace crds_angular.test.Services
{
    class StripeServiceTest
    {
        private Mock<IRestClient> restClient;
        private StripeService fixture;

        [SetUp]
        public void Setup()
        {
            restClient = new Mock<IRestClient>(MockBehavior.Strict);
            fixture = new StripeService(restClient.Object);
        }

        [Test]
        public void ShouldGetDefaultSource()
        {
            var cust = new StripeCustomer
            {
                sources = new Sources
                {
                    data = new List<SourceData>
                    {
                        new SourceData
                        {
                            id = "456",
                            @object = "bank_account",
                            last4 = "9876",
                            routing_number = "5432",
                        },
                        new SourceData
                        {
                            id = "123",
                            @object = "bank_account",
                            last4 = "1234",
                            routing_number = "5678",
                        },
                        new SourceData
                        {
                            id = "789",
                            @object = "credit_card",
                            brand = "visa",
                            last4 = "0001",
                            exp_month = "01",
                            exp_year = "2023",
                            address_zip = "20202"
                        },
                        new SourceData
                        {
                            id = "123",
                            @object = "credit_card",
                            brand = "mcc",
                            last4 = "0002",
                            exp_month = "2",
                            exp_year = "2024",
                            address_zip = "10101"
                        },
                    }
                },
                default_source = "123",
            };
            var stripeResponse = new Mock<IRestResponse<StripeCustomer>>(MockBehavior.Strict);
            stripeResponse.SetupGet(mocked => mocked.ResponseStatus).Returns(ResponseStatus.Completed).Verifiable();
            stripeResponse.SetupGet(mocked => mocked.StatusCode).Returns(HttpStatusCode.OK).Verifiable();
            stripeResponse.SetupGet(mocked => mocked.Data).Returns(cust).Verifiable();
            restClient.Setup(mocked => mocked.Execute<StripeCustomer>(It.IsAny<IRestRequest>())).Returns(stripeResponse.Object);

            var defaultSource = fixture.GetDefaultSource("token");
            Assert.IsNotNull(defaultSource);

            Assert.AreEqual("5678", defaultSource.routing_number);
            Assert.AreEqual("1234", defaultSource.bank_last4);

            Assert.AreEqual("mcc", defaultSource.brand);
            Assert.AreEqual("0002", defaultSource.last4);
            Assert.AreEqual("02", defaultSource.exp_month);
            Assert.AreEqual("24", defaultSource.exp_year);
            Assert.AreEqual("10101", defaultSource.address_zip);

            restClient.VerifyAll();
            stripeResponse.VerifyAll();
        }

        [Test]
        public void shouldThrowExceptionWhenTokenIsInvalid()
        {
            var stripeResponse = new Mock<IRestResponse<StripeCustomer>>(MockBehavior.Strict);
            stripeResponse.SetupGet(mocked => mocked.ResponseStatus).Returns(ResponseStatus.Completed).Verifiable();
            stripeResponse.SetupGet(mocked => mocked.StatusCode).Returns(HttpStatusCode.BadRequest).Verifiable();
            stripeResponse.SetupGet(mocked => mocked.Content).Returns("{error: {message:'Bad Request'}}").Verifiable();
            restClient.Setup(mocked => mocked.Execute<StripeCustomer>(It.IsAny<IRestRequest>())).Returns(stripeResponse.Object);

            Assert.Throws<StripeException>(() => fixture.CreateCustomer("token"));

            restClient.Verify(mocked => mocked.Execute<StripeCustomer>(
                It.Is<RestRequest>(o =>
                    o.Method == Method.POST
                    && o.Resource.Equals("customers")
                    && parameterMatches("description", "Crossroads Donor #pending", o.Parameters)
                    && parameterMatches("source", "token", o.Parameters)
                    )));
            restClient.VerifyAll();
            stripeResponse.VerifyAll();
        }

        [Test]
        public void shouldReturnSuccessfulCustomerId()
        {
            var customer = new StripeCustomer();
            customer.id = "12345";

            var stripeResponse = new Mock<IRestResponse<StripeCustomer>>(MockBehavior.Strict);
            stripeResponse.SetupGet(mocked => mocked.ResponseStatus).Returns(ResponseStatus.Completed).Verifiable();
            stripeResponse.SetupGet(mocked => mocked.StatusCode).Returns(HttpStatusCode.OK).Verifiable();
            stripeResponse.SetupGet(mocked => mocked.Data).Returns(customer).Verifiable();

            restClient.Setup(mocked => mocked.Execute<StripeCustomer>(It.IsAny<IRestRequest>())).Returns(stripeResponse.Object);

            var response = fixture.CreateCustomer("token");
            restClient.Verify(mocked => mocked.Execute<StripeCustomer>(
                It.Is<IRestRequest>(o =>
                    o.Method == Method.POST
                    && o.Resource.Equals("customers")
                    && parameterMatches("description", "Crossroads Donor #pending", o.Parameters)
                    && parameterMatches("source", "token", o.Parameters)
                    )));
            restClient.VerifyAll();
            stripeResponse.VerifyAll();

            Assert.AreEqual("12345", response);
        }

        [Test]
        public void shouldUpdateCustomerDescription()
        {
            var customer = new StripeCustomer();
            customer.id = "12345";

            var stripeResponse = new Mock<IRestResponse<StripeCustomer>>(MockBehavior.Strict);
            stripeResponse.SetupGet(mocked => mocked.ResponseStatus).Returns(ResponseStatus.Completed).Verifiable();
            stripeResponse.SetupGet(mocked => mocked.StatusCode).Returns(HttpStatusCode.OK).Verifiable();
            stripeResponse.SetupGet(mocked => mocked.Data).Returns(customer).Verifiable();

            restClient.Setup(mocked => mocked.Execute<StripeCustomer>(It.IsAny<IRestRequest>())).Returns(stripeResponse.Object);

            var response = fixture.UpdateCustomerDescription("token", 102030);
            restClient.Verify(mocked => mocked.Execute<StripeCustomer>(
                It.Is<IRestRequest>(o =>
                    o.Method == Method.POST
                    && o.Resource.Equals("customers/token")
                    && parameterMatches("description", "Crossroads Donor #102030", o.Parameters)
                    )));
            restClient.VerifyAll();
            stripeResponse.VerifyAll();

            Assert.AreEqual("12345", response);
        }

        [Test]
        public void shouldThrowExceptionWhenCustomerUpdateFails()
        {
            var stripeResponse = new Mock<IRestResponse<StripeCustomer>>(MockBehavior.Strict);
            stripeResponse.SetupGet(mocked => mocked.ResponseStatus).Returns(ResponseStatus.Completed).Verifiable();
            stripeResponse.SetupGet(mocked => mocked.StatusCode).Returns(HttpStatusCode.BadRequest).Verifiable();
            stripeResponse.SetupGet(mocked => mocked.Content).Returns("{error: {message:'Invalid Request'}}").Verifiable();

            restClient.Setup(mocked => mocked.Execute<StripeCustomer>(It.IsAny<IRestRequest>())).Returns(stripeResponse.Object);

            try
            {
                fixture.UpdateCustomerDescription("token", 102030);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (StripeException e)
            {
                Assert.AreEqual("Customer update failed", e.Message);
                Assert.IsNotNull(e.DetailMessage);
                Assert.AreEqual("Invalid Request", e.DetailMessage);
            }
        }

        [Test]
        public void shouldChargeCustomer()
        {
            var customer = new StripeCustomer();
            customer.id = "12345";
            customer.default_source = "some card";

            var charge = new StripeCharge();
            charge.id = "90210";

            var stripeResponse = new Mock<IRestResponse<StripeCharge>>(MockBehavior.Strict);
            stripeResponse.SetupGet(mocked => mocked.ResponseStatus).Returns(ResponseStatus.Completed).Verifiable();
            stripeResponse.SetupGet(mocked => mocked.StatusCode).Returns(HttpStatusCode.OK).Verifiable();
            stripeResponse.SetupGet(mocked => mocked.Data).Returns(charge).Verifiable();

            restClient.Setup(mocked => mocked.Execute<StripeCharge>(It.IsAny<IRestRequest>())).Returns(stripeResponse.Object);

            var response = fixture.ChargeCustomer("cust_token", 9090, 98765, "cc");

            restClient.Verify(mocked => mocked.Execute<StripeCharge>(
                It.Is<IRestRequest>(o =>
                    o.Method == Method.POST
                    && o.Resource.Equals("charges")
                    && parameterMatches("amount", 9090 * 100, o.Parameters)
                    && parameterMatches("currency", "usd", o.Parameters)
                    && parameterMatches("customer", "cust_token", o.Parameters)
                    && parameterMatches("description", "Donor ID #98765", o.Parameters)
                    )));

            restClient.VerifyAll();
            stripeResponse.VerifyAll();

            Assert.AreEqual("90210", response);
        }

        private bool parameterMatches(string name, object value, List<Parameter> parms)
        {
            return(parms.Find(p => p.Name.Equals(name) && p.Value.Equals(value)) != null);
        }
        
        [Test]
        public void shouldNotChargeCustomerIfAmountIsInvalid()
        {
            var customer = new StripeCustomer();
            customer.id = "12345";
            customer.default_source = "some card";
            
            var getCustomerResponse = new Mock<IRestResponse<StripeCustomer>>(MockBehavior.Strict);

            getCustomerResponse.SetupGet(mocked => mocked.ResponseStatus).Returns(ResponseStatus.Completed).Verifiable();
            getCustomerResponse.SetupGet(mocked => mocked.StatusCode).Returns(HttpStatusCode.OK).Verifiable();
            getCustomerResponse.SetupGet(mocked => mocked.Data).Returns(customer).Verifiable();
            restClient.Setup(mocked => mocked.Execute<StripeCustomer>(It.IsAny<IRestRequest>())).Returns(getCustomerResponse.Object);


            var chargeResponse = new Mock<IRestResponse<StripeCharge>>(MockBehavior.Strict);
            chargeResponse.SetupGet(mocked => mocked.ResponseStatus).Returns(ResponseStatus.Completed).Verifiable();
            chargeResponse.SetupGet(mocked => mocked.StatusCode).Returns(HttpStatusCode.BadRequest).Verifiable();
            chargeResponse.SetupGet(mocked => mocked.Content).Returns("{error: {message:'Invalid Integer Amount'}}").Verifiable();

            restClient.Setup(mocked => mocked.Execute<StripeCharge>(It.IsAny<IRestRequest>())).Returns(chargeResponse.Object);
            try
            {
                fixture.ChargeCustomer("token", -900, 98765, "cc");
                Assert.Fail("Should have thrown exception");
            }
            catch (StripeException e)
            {
                Assert.AreEqual("Invalid charge request", e.Message);
                Assert.IsNotNull(e.DetailMessage);
                Assert.AreEqual("Invalid Integer Amount", e.DetailMessage);
            }

        }

        [Test]
        public void ShouldUpdateCustomerSource()
        {
            var customer = new StripeCustomer();
            customer.id = "cus_test0618";
            customer.default_source = "platinum card";
           
            customer.sources = new Sources()
            {
                data = new List<SourceData>()
                {
                    new SourceData()
                    {
                        last4 = "8585",
                        brand = "Visa",
                        address_zip = "45454" ,
                        id = "platinum card",
                        exp_month = "01",
                        exp_year = "2020"
                    }
                        
                }
            };

            var stripeResponse = new Mock<IRestResponse<StripeCustomer>>(MockBehavior.Strict);
            stripeResponse.SetupGet(mocked => mocked.ResponseStatus).Returns(ResponseStatus.Completed).Verifiable();
            stripeResponse.SetupGet(mocked => mocked.StatusCode).Returns(HttpStatusCode.OK).Verifiable();
            stripeResponse.SetupGet(mocked => mocked.Data).Returns(customer).Verifiable();

            restClient.Setup(mocked => mocked.Execute<StripeCustomer>(It.IsAny<IRestRequest>())).Returns(stripeResponse.Object);

            var defaultSource = fixture.UpdateCustomerSource("customerToken", "cardToken");
            restClient.Verify(mocked => mocked.Execute<StripeCustomer>(
                It.Is<IRestRequest>(o =>
                    o.Method == Method.POST
                    && o.Resource.Equals("customers/customerToken")
                    && parameterMatches("source", "cardToken",o.Parameters)
                    )));
            restClient.VerifyAll();
            stripeResponse.VerifyAll();
          
            Assert.AreEqual("Visa", defaultSource.brand);
            Assert.AreEqual("8585", defaultSource.last4);
            Assert.AreEqual("45454", defaultSource.address_zip);
        }

    }

}
