using System;
using System.Collections.Generic;
using NUnit.Framework;
using Expedia;
using Rhino.Mocks;

namespace ExpediaTest
{
	[TestFixture()]
	public class CarTest
	{	
		private Car targetCar;
		private MockRepository mocks;
		
		[SetUp()]
		public void SetUp()
		{
			targetCar = new Car(5);
			mocks = new MockRepository();
		}
		
		[Test()]
		public void TestThatCarInitializes()
		{
			Assert.IsNotNull(targetCar);
		}	
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForFiveDays()
		{
			Assert.AreEqual(50, targetCar.getBasePrice()	);
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForTenDays()
		{
            var target = new Car(10);
			Assert.AreEqual(80, target.getBasePrice());	
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForSevenDays()
		{
			var target = new Car(7);
			Assert.AreEqual(10*7*.8, target.getBasePrice());
		}
		
		[Test()]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestThatCarThrowsOnBadLength()
		{
			new Car(-5);
		}

        [Test()]
        public void TestThatCarDoesGetLocationFromTheDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            String carLocation = "On a whale";
            String anotherCarLocation = "On a raptor ranch";

            using (mocks.Record())
            {
                // The mock will return "On a whale" when the call is made with 24
                mockDatabase.getCarLocation(24);
                LastCall.Return(carLocation);
                // The mock will return "Raptor Wrangler" when the call is made with 1025
                mockDatabase.getCarLocation(1025);
                LastCall.Return(anotherCarLocation);
            }

            var target = new Car(10);
            target.Database = mockDatabase;

            String result;

            result = target.getCarLocation(1025);
            Assert.AreEqual(result, anotherCarLocation);

            result = target.getCarLocation(24);
            Assert.AreEqual(result, carLocation);
        }

        [Test()]
        public void TestThatCarDoesGetMilageFromDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            int Miles = 442;

            mockDatabase.Miles = Miles;

            var target = new Car(10) {Database = mockDatabase};

            int Mileage = target.Mileage;
            Assert.AreEqual(Mileage, Miles);
        }

	    [Test()]
	    public void TestThatBasePriceIsCorrectUsingObjectMother()
	    {
	        var targetBMW = ObjectMother.BMW();
	        var targetSaab = ObjectMother.Saab();
            Assert.AreEqual(10*7*.8, targetSaab.getBasePrice());
	        Assert.AreEqual(80, targetBMW.getBasePrice());

	    }
	}
}
