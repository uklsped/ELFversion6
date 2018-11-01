using System;
using NUnit.Framework;
using SqlTest;
using System.Transactions;

namespace SandBoxDBTests.Tests
{
    public class UnitUnderTest
    {
        TransactionScope scope;
		SqlTest.SqlTestTarget PhysicsEnergyDev;

        [SetUp]
        public void Setup()
        {
            scope = new TransactionScope();
			PhysicsEnergyDev = new SqlTest.SqlTestTarget("ConnectionString");
        }

        [TearDown]
        public void Teardown()
        {
            if (Transaction.Current.TransactionInformation.Status == TransactionStatus.Active)
            {
                scope.Dispose();
            }
        }

        [Test]
        public void UnitUnderTest_Action_ExpectedOutcome()
        {
            //Arrange
            //dataWarehouseDb.ExecuteAdhoc($"Truncate table / Insert test data;");

            //Act
            dataWarehouseDb.ExecuteAdhoc($"Exec proc;");

            //Assert
            var actual = dataWarehouseDb.GetActual("Select scalar value to test outcome");
            Assert.That(actual, Is.EqualTo("Expected Value"));

            //The teardown method runs after every test and will rollback all actions in one transaction
        }

 
        [TestCase("SomeValue", 1)]
        [TestCase("SomeOtherValue", 2)]
        public void UnitUnderTest_Action_ExpectedOutcome(string param, object expected)
        {
            //Arrange
            dataWarehouseDb.ExecuteAdhoc($"Truncate table / Insert test data;");

            //Act
            dataWarehouseDb.ExecuteAdhoc($"Exec proc @param = {param};");

            //Assert
            var actual = dataWarehouseDb.GetActual("Select to check result");
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
