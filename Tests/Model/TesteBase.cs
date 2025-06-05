using PersonalNotesApp.Model;

namespace PersonalNotesApp.Tests.Model
{
	[TestFixture]
	internal class TesteBase
	{
		[Test]
		public void Nome_QuandoAtribuido_AtivaPropertyChanged()
		{
			//Arrange
			var Base = new Pasta("teste");
			bool propertyChangedRaised = false;
			base.PropertyChanged += (s, e) =>
			{
				if (e.PropertyName == nameof(Base.Nome))
					propertyChangedRaised = true;
			};

			// Act
			base.Nome = "Novo Nome";

			// Assert
			Assert.IsTrue(propertyChangedRaised);
			Assert.AreEqual("Novo Nome", base.Nome);

			//Act

			//Assert

			//Testes

		}

	}
}
