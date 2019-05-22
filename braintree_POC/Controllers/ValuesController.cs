using Braintree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace braintree_POC.Controllers
{
    public class ValuesController : ApiController
    {

        public BraintreeGateway gateway = new BraintreeGateway
        {
            Environment = Braintree.Environment.SANDBOX,
            MerchantId = "sg9964rb9vrgxxpt",
            PublicKey = "7ynycvfbtb8tkw5j",
            PrivateKey = "a7f6ade0ff49641d2f30bb70bdca2201"
        };
        [HttpGet]
        [Route("GetToken")]
        public string GetToken()
        {


            var clientToken = gateway.ClientToken.Generate(
              new ClientTokenRequest
              {

              }
            );

            return clientToken;
        }
        [HttpPost]
        [Route("TestTrans")]
        public dynamic TestTransaction(String input)
        {
            var request = new TransactionRequest
            {
                Amount = 10.00M,
                PaymentMethodNonce = input,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);

            return result;
        }



        [HttpPost]
        [Route("CustomTrans")]
        public dynamic CustomTransaction(Info input)
        {
            var request = new CustomerRequest
            {
                FirstName = "Mark",
                LastName = "Jones",
                
                PaymentMethodNonce = input.paymentMethodNonce
            };

            Result<Customer> result = gateway.Customer.Create(request);

            bool success = result.IsSuccess();
            // true

            Customer customer = result.Target;
            string customerId = customer.Id;
            // e.g. 160923

            string cardToken = customer.PaymentMethods[0].Token;

            return result;
        }

        [HttpGet]
        [Route("ChargeCustomer")]
        public dynamic ChargeCustomer()
        {

            TransactionRequest request = new TransactionRequest()
            {
                CustomerId= "138721831",
                Amount=11.00M

            };

            Result<Transaction> result = gateway.Transaction.Sale(request);
            return result;
        }


        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        public class Info {

            public string paymentMethodNonce { get; set; }

        }
    }
}
