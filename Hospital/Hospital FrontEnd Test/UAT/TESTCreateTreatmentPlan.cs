using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace HospitalFrontEnd.Data
{
    [TestFixture]
    public class CreateTreatmentPlanTests
    {
        private IWebDriver _driver;
        private string _baseUrl = "https://localhost:7137/";

        [SetUp]
        public void SetUp()
        {
            _driver = new ChromeDriver();
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }

        [Test]
        public void StaffCanCreateNewTreatmentPlan()
        {
            // Navigate to the login page
            //_driver.Navigate().GoToUrl($"{_baseUrl}/login");

            //// Log in as staff
            //var usernameField = _driver.FindElement(By.Id("username"));
            //var passwordField = _driver.FindElement(By.Id("password"));
            //var loginButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

            //usernameField.SendKeys("staff_username");
            //passwordField.SendKeys("staff_password");
            //loginButton.Click();

            // Wait for navigation to dashboard or appropriate page
            System.Threading.Thread.Sleep(2000); // Consider using WebDriverWait instead of Thread.Sleep for better practice

            // Navigate to the create treatment plan page
            _driver.Navigate().GoToUrl($"{_baseUrl}/create-treatment-plan");

            // Fill in the treatment plan details
            var patientIdField = _driver.FindElement(By.Id("patientId"));
            var planDetailsField = _driver.FindElement(By.Id("planDetails"));
            var submitButton = _driver.FindElement(By.CssSelector("button[type='submit']"));

            patientIdField.SendKeys("123"); // Assuming 123 is the patient ID
            planDetailsField.SendKeys("New Treatment Plan Details");
            submitButton.Click();

            // Wait for response
            System.Threading.Thread.Sleep(2000); // Consider using WebDriverWait instead of Thread.Sleep for better practice

            // Verify the creation success message or redirect
            var successMessage = _driver.FindElement(By.CssSelector(".success-message"));
            Assert.That(successMessage!=null);
            Assert.That("Treatment plan created successfully" == successMessage.Text);

            // Optionally, verify the treatment plan in the system by navigating to the patient's treatment plan list page
            _driver.Navigate().GoToUrl($"{_baseUrl}/patient/123/treatment-plans");

            var treatmentPlan = _driver.FindElement(By.XPath("//td[text()='New Treatment Plan Details']"));
            Assert.That(treatmentPlan!=null);
        }
    }
}



