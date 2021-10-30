using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScsLib.Hashing;

namespace ScsLib.Test
{
	[TestClass]
	public class CityHashTest
	{
		[DataTestMethod]
		[DataRow("", 11160318154034397263UL)]
		[DataRow("ö", 10795344492661217258UL)]
		[DataRow("font", 3884001188172982635UL)]
		[DataRow("vehicle/trailer_eu", 1265479589211028179UL)]
		[DataRow("vehicle/trailer_eu/car_transporter/blue.dds", 4839821608980739887UL)]
		[DataRow("326947f5-a3b2-48eb-904b-b97c529f9f3f425df140-7968-46f4-bd4b-db33a4f652aa11ef3fc1-8433-4710-86d1-ab6effbdbfd1", 10176022315815290723UL)]
		public void CityHash64_ReturnCorrectHash(string data, ulong expectedHash)
		{
			ulong hash = new CityHash().CityHash64(data);

			Assert.AreEqual(expectedHash, hash);
		}
	}
}
