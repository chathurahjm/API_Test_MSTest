using API_Test.model;
using API_Test.testUtils;
using API_Test.util;
using Newtonsoft.Json;
using RestSharp;

namespace API_Test.tests
{
    [TestClass]
    public class addValidProductsTest: TestBase
    {

        [TestMethod]
        [DynamicData(nameof(GetValidProductData1), DynamicDataSourceType.Method)]
        public void validateAddNewPorducts(string nameFromCSV, Data dataFromCSV)
        {

            extentTest = extentReport.CreateTest("Validate Add New Porducts "+ nameFromCSV);
            var endpoint = "/objects";


            var request = new RestRequest(endpoint, Method.Post);

            var newProduct = new ProductData
            {
                name = nameFromCSV,
                data = dataFromCSV
            };

            string jsonBody = JsonConvert.SerializeObject(newProduct);
            request.AddParameter("application/json", jsonBody, ParameterType.RequestBody);

            var response = Client.Execute(request);

            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode, "Wrong status code");

           
            Console.WriteLine(response.Content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                ProductData produtDataObject = JsonConvert.DeserializeObject<ProductData>(response.Content);

                Assert.AreEqual(nameFromCSV, produtDataObject.name);
                Assert.AreEqual(dataFromCSV.Year, produtDataObject.data.Year);
                Assert.AreEqual(dataFromCSV.Price, produtDataObject.data.Price);
                Assert.AreEqual(dataFromCSV.CPUModel, produtDataObject.data.CPUModel);
                Assert.AreEqual(dataFromCSV.Color, produtDataObject.data.Color);
                Assert.AreEqual(dataFromCSV.Capacity, produtDataObject.data.Capacity);
                Assert.IsNotNull(produtDataObject.createdAt);
              



            }
            
        }

        private static IEnumerable<object[]> GetValidProductData1()
        {
            string csvFilePath = Constants.VALID_DATAFILE_PATH;
            return CsvReader.ReadDevicesFromCsv(csvFilePath);
        }

       
    }
}
