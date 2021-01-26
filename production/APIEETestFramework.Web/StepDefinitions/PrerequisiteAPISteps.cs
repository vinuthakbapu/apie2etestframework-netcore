using UiIntegrationTest.PageElements;
using TechTalk.SpecFlow;

namespace UiIntegrationTest.StepDefinitions
{
    [Binding]
    public class PrerequisiteAPISteps
    {
        private readonly PrerequisiteAPIFunctions _prerequisiteAPIFunctions;
        private readonly CommonAPIFunctions _commonAPIFunctions;

        public PrerequisiteAPISteps(PrerequisiteAPIFunctions prerequisiteAPIFunctions, CommonAPIFunctions commonAPIFunctions)
        {
            _prerequisiteAPIFunctions = prerequisiteAPIFunctions;
            _commonAPIFunctions = commonAPIFunctions;
        }

        [When(@"Create Customer")]
        public void WhenCreateCustomer()
        {
            _prerequisiteAPIFunctions.CreateCustomer();
        }

        [Then(@"Verify the (.*) status code for (.*)")]
        public void ThenVerifyTheStatusCodeForCreateCustomer(int p0, string methodName)
        {
            //_commonAPIFunctions.ValidateResponse(methodName);
        }

        [Then(@"Store CustomerId")]
        public void ThenStoreCustomerId()
        {
            //_commonAPIFunctions.StoreCustomerID();
        }

        [When(@"Load SOM")]
        public void WhenLoadSOM()
        {
            //_prerequisiteAPIFunctions.LoadSOM();
        }

        [When(@"Check SOM")]
        public void WhenCheckSOM()
        {
           // _prerequisiteAPIFunctions.CheckSOM();
        }

        [Then(@"Verify the (.*) status code validate for (.*) status")]
        public void ThenVerifyTheStatusCodeValidateForCOMPLETEStatus(int p0,string status)
        {
            //_prerequisiteAPIFunctions.ValidateCheckSOM(status);
        }

        [When(@"Delete Customer with creted CustomerId")]
        public void WhenDeleteCustomerWithCretedCustomerId()
        {
            _prerequisiteAPIFunctions.DeleteCustomer();
        }
    }
}
